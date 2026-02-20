using UnityEngine;
using UnityEngine.InputSystem;

public class Move1 : MonoBehaviour
{
    public float speed;// Speed the player moves at. Will increase until it reaches target speed.
    public float targetspeed;// Max speed the player can reach
    public Vector2 inputvector;
    Rigidbody2D rb;
    public float jumpforce;
    // Start is called once before the first execution of Update after the MonoBehaviour is created
    void Start()
    {
    rb = GetComponent<Rigidbody2D>();
    }

    // Update is called once per frame
    void Update()
    {
        
        Move();
    }

    public void Move()
    {
        //FYI: remember that the Vector is only reading the ".x" variable so only left or right.
        Vector2 movementDelta = new Vector2 (inputvector.x * speed * Time.deltaTime,0);
        transform.Translate(movementDelta);
    }
    public void GetInputstuff(InputAction.CallbackContext context){
        inputvector = context.ReadValue<Vector2>(); 
    }
    public void Jump(InputAction.CallbackContext context)
    {
        
        //TODO: Jump buffer and coyote time
        if (context.performed)
        {
            rb.linearVelocity = new Vector2(inputvector.x, jumpforce);
        }
        

        
    }
    
        
        
        

    
}
