using UnityEngine;

public class GiveDamage : MonoBehaviour
{
    /// <summary>
    /// The base damage amount to be passed to the hit object.
    /// </summary>
    public int BaseDamage = 10;
    [SerializeField] private GameObject bullet;

    private void OnCollisionEnter2D(Collision2D col)
    {
        if(col == null) {
            Debug.Log("No collision");
        }
        else {
            Debug.Log(col.gameObject.name);
        }
        // 1. Pass the BaseDamage variable over to the hit object in a separate part of the script.
        TransferDamage(col.gameObject);

        // 2. The GameObject deletes itself afterwards. 
        // This covers hitting a surface with the layer "Ground", as well as hitting objects that receive damage.
        Destroy(bullet);
    }

    /// <summary>
    /// Passes the BaseDamage variable over to the specified target object.
    /// </summary>
    /// <param name="target">The object that was hit by this GameObject.</param>
    private void TransferDamage(GameObject target)
    {
      
        // Using SendMessage as a decoupled placeholder since the receiving health component is not implemented yet.
        // DontRequireReceiver ensures no errors are thrown if the target (e.g., Ground) doesn't have a TakeDamage method.
        //In the enemy's health script, there will need to be a method called "TakeDamage" along with a bracket thing 
        // like in TransferDamage, BaseDamage will be the value that the TakeDamage method "talks" about.
        target.SendMessage("TakeDamage", BaseDamage, SendMessageOptions.DontRequireReceiver);
    }
   
    
    
}
