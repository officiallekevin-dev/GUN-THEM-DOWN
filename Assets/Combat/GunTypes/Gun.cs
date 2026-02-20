using UnityEngine;

[CreateAssetMenu(fileName = "Gun", menuName = "Weapons/Gun")]
public class Gun : ScriptableObject
{
    public string gunName;
    public string gunDescription;
    public Sprite gunImage;
    public int magazineSize;
    public float reloadTime;
    public GameObject prefab;//ammo used -> it will be instantiated when the gun is fired 
    public float bulletForce;
    //Firing mode
    //The actual gun image
    //magazine size
    //reload time
}
