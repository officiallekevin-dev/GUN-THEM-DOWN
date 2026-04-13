using UnityEngine;
using UnityEngine.InputSystem;


[RequireComponent(typeof(Rigidbody2D))]
public class Movement2 : MonoBehaviour
{
    

    [Header("Movement Settings")]
    [SerializeField] private float maxSpeed = 10f;
    [SerializeField] private float accelerationTime = 0.2f;
    [SerializeField] private float decelerationTime = 0.1f;
    [SerializeField] private float jumpHeight = 3f;
    [SerializeField] private float fallGravityMultiplier = 1.5f;
    [SerializeField] private float defaultGravityMultiplier = 1.5f;
    [SerializeField] private float movementForce;

    [Header("Ground Detection")]
    [SerializeField] private LayerMask groundLayer;
    [SerializeField] private Vector2 groundCheckSize = new Vector2(0.8f, 0.2f);
    [SerializeField] private float groundCheckDistance = 0.05f;
    //[SerializeField] private float movementForce;
    //[SerializeField] private float speedDif;
    //[SerializeField] private float rateTime;

    // Internal State
    private Rigidbody2D rb;
    private Vector2 moveInput;
    private bool isGrounded;
    private bool jumpRequested;
    

    private void Awake()
    {
        rb = GetComponent<Rigidbody2D>();
    }

    private void FixedUpdate()
    {
        CheckGrounded();//Always check if we are grounded in case if we want to jump
        ApplyMovement();//Always apply the force to the player. It won't matter if the player isnt inputting anything
        ApplyGravityModifiers();//Always apply the gravity modifiers
        
        if (jumpRequested)//If we want to jump
        {
            ApplyJump();//Jump
            jumpRequested = false;//Reset
        }
        
        
    }

   
    public void OnMove(InputAction.CallbackContext context)
    {
        //Get Direction of movement e.g left or right
        moveInput = context.ReadValue<Vector2>();
    }

    private void ApplyMovement()
    {
        // Calculate target velocity
        float targetSpeed = moveInput.x * maxSpeed;
        
        // Calculate current speed difference
        float currentSpeed = rb.linearVelocity.x;
        float speedDif = targetSpeed - currentSpeed;

        // Determine acceleration rate based on whether we are accelerating or decelerating
        float rateTime;

        // We are trying to stop or reverse direction -> Decel
        //Mathf.sign - > returns -1, 0, or 1 based on the sign of the value. If currentSpeed was 10 it would give 1, if it was -10 it would give -1
        // Math.Abs - > Gives the number without the -, if currentSpeed was -10 it would give. The value is called an absolute value
        // If the absolute value o targetSpeed less than 0.01 OR targetSpeed and currentSpeed have different signs, 
        
        if (Mathf.Abs(targetSpeed) < 0.01f || Mathf.Sign(targetSpeed) != Mathf.Sign(currentSpeed))
        {
             rateTime = Mathf.Max(decelerationTime, 0.001f); // Avoid div/0
        }
        else
        {
             rateTime = Mathf.Max(accelerationTime, 0.001f);
        }

        // Calculate max acceleration allowed
        float maxAccel = maxSpeed / rateTime;

        // Calculate the acceleration needed to reach the target velocity INSTANTLY in this frame
        float neededAccel = speedDif / Time.fixedDeltaTime;

        // Clamp the needed acceleration to the max acceleration allowed
        // This prevents overshooting (jitter) when close to the target speed
        // (Value that is clamped, minimum value, maximum value)
        float appliedAccel = Mathf.Clamp(neededAccel, -maxAccel, maxAccel);

        // F = m * a -> Force = mass * acceleration 
        movementForce = appliedAccel * rb.mass;
        
        //Directly causes the playerr to move. It just adds the force to the player
        rb.AddForce(movementForce * Vector2.right);
    }

    
    public void OnJump(InputAction.CallbackContext context)
    {
        if (context.performed)//If space or whatever button to jump is pressed
        {
            // We only buffer a jump if we are somewhat close to the ground usually, 
            // but for simplicity we just flag it. 
            // Better: use a jump buffer time.
            if (isGrounded)//If we arer on the ground
            {
                jumpRequested = true;//Allow a jump
            }
        }
        
        // Optional: Variable jump height (release button to fall faster)
        if (context.canceled && rb.linearVelocity.y > 0)
        {
             // Cut velocity to allow short hops
             Vector2 velocity = rb.linearVelocity;
             velocity.y *= 0.5f;
             rb.linearVelocity = velocity;
        }
    }

    

    private void ApplyJump()
    {
        if (isGrounded)
        {
            // v = sqrt(2 * g * h)
            float jumpVelocity = Mathf.Sqrt(2f * Mathf.Abs(Physics2D.gravity.y * rb.gravityScale) * jumpHeight);
            
            // Set velocity directly for consistent jump height regardless of current y velocity
            Vector2 velocity = rb.linearVelocity;
            velocity.y = jumpVelocity;
            rb.linearVelocity = velocity;
        }
    }
    

    private void CheckGrounded()
    {
        // Position: Feet of the player
        // We assume the pivot is center. If pivot is feet, adjust accordingly.
        // Let's assume standard capsule/box with center pivot.
        
        Vector2 origin = (Vector2)transform.position + (Vector2.down * 0.5f); // Adjust 0.5f based on your sprite size!
        // Improved: use collider bounds if possible, but user asked for logic, not auto-finding.
        // Let's try to get collider to be safe.
        var col = GetComponent<Collider2D>();
        if (col != null)
        {
            origin = new Vector2(col.bounds.center.x, col.bounds.min.y);
        }

        RaycastHit2D hit = Physics2D.BoxCast(origin, groundCheckSize, 0f, Vector2.down, groundCheckDistance, groundLayer);
        
        bool wasGrounded = isGrounded;
        isGrounded = hit.collider != null;

        // Visual debug
        Color color = isGrounded ? Color.green : Color.red;
        Debug.DrawRay(origin, Vector2.down * groundCheckDistance, color);
    }
    
    private void ApplyGravityModifiers()
    {
        // Makes falling feel heavier/less floaty
        if (rb.linearVelocity.y < 0)// If the player is starting to fall or the force we add when we jump runs out
        {
            rb.gravityScale = fallGravityMultiplier;//Make our target gravity the actual gravity
        }
        else//otherwise
        {
            rb.gravityScale = defaultGravityMultiplier; // Reset to default (assuming default is 1, adjust if you set it elsewhere)
        }
    }

    
    private void OnDrawGizmosSelected()
    {
        // Draw Ground Check Box
        var col = GetComponent<Collider2D>();
        Vector2 origin = transform.position;
        if (col != null)
        {
             origin = new Vector2(col.bounds.center.x, col.bounds.min.y);
        }
        else 
        {
            // Fallback estimation
             origin = (Vector2)transform.position + Vector2.down * 0.5f; 
        }

        Gizmos.color = Color.yellow;
        // Gizmos.DrawWireCube(origin + Vector2.down * groundCheckDistance, groundCheckSize); 
        // Note: BoxCast origin is the center of the box at the start, then it sweeps.
        // So the check area is effectively below the Origin.
        Gizmos.DrawWireCube(origin + Vector2.down * (groundCheckDistance / 2f), groundCheckSize);
    }

    
}


