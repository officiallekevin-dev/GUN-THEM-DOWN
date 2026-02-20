using UnityEngine;
using UnityEngine.InputSystem;

public class BaseGun : MonoBehaviour
{
    [SerializeField] public Gun gun;
    
    
    // Start is called once before the first execution of Update after the MonoBehaviour is created
    void Start()
    {
        
    }

    // Update is called once per frame
    void Update()
    {
        
    }
    public void Shoot(InputAction.CallbackContext context)
    {
        if (context.performed)
        {
            Debug.Log(gun.gunName);
            GameObject bullet = Instantiate(gun.prefab , transform.position, transform.rotation);
            //Rigidbody rb = bullet.GetComponent<Rigidbody>();
            if (bullet.TryGetComponent<Rigidbody2D>(out Rigidbody2D rb)) {
                rb.AddForce(transform.right * gun.bulletForce);
                Debug.Log(gun.bulletForce);
                Debug.Log(rb);
            }
            else  {
                Debug.Log("Bullet Rigidbody2D not found");
            }
        }
        //Instantiate(gun.bulletPrefab, transform.position, transform.rotation);
        //Note: bullet prefabs will have basic damage value, whilst 
    }
}
