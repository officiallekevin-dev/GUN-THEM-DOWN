using UnityEngine;
using UnityEngine.InputSystem;

public class BaseGun : MonoBehaviour
{
    [SerializeField] public Gun gun;
    [SerializeField] private GameObject muzzle;
    [SerializeField] private Rigidbody2D playerRb;
    
    
    
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
            GameObject bullet = Instantiate(gun.prefab , muzzle.transform.position, muzzle.transform.rotation);
            //Rigidbody rb = bullet.GetComponent<Rigidbody>();
            if (bullet.TryGetComponent<Rigidbody2D>(out Rigidbody2D rb)) {
                rb.AddForce(transform.right * gun.bulletSpeed);
                Debug.Log(gun.bulletSpeed);
                Debug.Log(rb);
            }
            else  {
                Debug.Log("Bullet Rigidbody2D not found");
            }
            //apply recoil to player by pushing the player backwards
            
            playerRb.AddForce(-transform.right * gun.bulletRecoil, ForceMode2D.Impulse);
            
            //-transform.right * gun.bulletRecoil;
            
        }
        //Instantiate(gun.bulletPrefab, transform.position, transform.rotation);
        //Note: bullet prefabs will have basic damage value, whilst 
    }
}
