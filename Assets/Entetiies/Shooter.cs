using UnityEngine;

[CreateAssetMenu(fileName = "Tier1Guard", menuName = "Entities/Enemies/Tier1Guard")]
public class Tier1Guard : ScriptableObject
{
    public int health;
    public int damage;
    public float speed;
    public enum firingMode {//Enum allows us to create a list of options. Later when the enemy needs the data, we can
    // Reference the firingmode and then later define what the modes actually mean, however im not doing that just yet.
        Auto,
        Semi,
        Burst
    }
    public float range;
    public GameObject bullet;
    public Transform muzzle;
}
