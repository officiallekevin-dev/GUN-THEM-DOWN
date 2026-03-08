using UnityEngine;

public class EnemyHP : MonoBehaviour
{
    [SerializeField] private Tier1Guard T1;
    [SerializeField] private int CurrentHealth;
    [SerializeField] private GameObject itself;// After it dies, we need to reference itself so it can destroy it self
    // Start is called once before the first execution of Update after the MonoBehaviour is created
    void Start()
    {
        GetComponent<SpriteRenderer>().sprite = T1.sprite;
        CurrentHealth = T1.health;
    }

    // Update is called once per frame
    void Update()
    {
        
    }
    public void TakeDamage(int damage)
    {
        CurrentHealth -= damage;
        if (CurrentHealth <= 0)
        {
                
            Die();
        }
    }
    public void Die() {
        Destroy(itself);
    }
}
