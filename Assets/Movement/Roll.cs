using UnityEngine;
using UnityEngine.InputSystem;
//VELOCITY = DISTANCE/TIME
public class Roll : MonoBehaviour
{
    private Rigidbody2D rb;
    private bool isRolling;
    [Header("ROLL")]
    [SerializeField] private float rollVelocity;
    [SerializeField] private float rollDistance;
    [SerializeField] private float rollTime;
    [SerializeField] private float SetRollTime;
    [SerializeField] private float movementForce;
    
    [SerializeField] private float dashforce = 20f;
    // Start is called once before the first execution of Update after the MonoBehaviour is created
    void Start()
    {
        rb = GetComponent<Rigidbody2D>();
    }

    // Update is called once per frame
    void FixedUpdate()
    {
        if (isRolling)
        {
          //set timer for 24 frames  
          //VELOCITY = DISTANCE/TIME
          rb.linearVelocity = new Vector2(rollVelocity, rb.linearVelocity.y);
          //rb.AddForce(transform.right * movementForce, ForceMode2D.Impulse);
          //rb.linearVelocity = new Vector2(rollDistance/rollDuration, rb.linearVelocity.y);
          rollTime -= Time.deltaTime;
          if (rollTime <= 0)
          {
            
            RollEnd();
          }

        }
    }
    public void Onroll(InputAction.CallbackContext context) {
        if (context.performed && !isRolling) {
            //roll, will last 24 frames during that time, the player's hitbox will be disabled and will move
            // var col = GetComponent<Collider2D>();
            // //whilst rolling
            // movementForce = 20f;
            // col.enabled = false;
            // //after rolling
            // col.enabled = true;
            RollStart();
            
            
        }
    }

    private void RollStart() {
        var col = GetComponent<Collider2D>();
        //col.isTrigger = true; I will do this later, its going to work by cooperating with the health script or maybe turning off a seperate hitbox 
        //movementForce = Mathf.Sign(rb.linearVelocity.x) * dashforce;//I need to lock the direction to one time
        //start timer for 24 frames
        isRolling = true;
        //VELOCITY = DISTANCE/TIME
        //rb.linearVelocity = new Vector2(((rollDistance/rollDuration) * Mathf.Sign(rb.linearVelocity.x)), rb.linearVelocity.y);
        rollVelocity = (rollDistance/rollTime) * Mathf.Sign(rb.linearVelocity.x);
        
    }

    private void RollEnd() {
        var col = GetComponent<Collider2D>();
        //col.isTrigger = false;
        isRolling = false;
        //movementForce =  0;
        rollTime = SetRollTime;//To be honest I could just set the float as a variable but i cant really be asked :)
        //we need to slowly translition to the roll speed to the normal speed
    }
}
/*The roll dont work, pressing the movement button overrides the roll since the application of movement is constant I need to have it so 
that the roll disables movement temperarily. */
