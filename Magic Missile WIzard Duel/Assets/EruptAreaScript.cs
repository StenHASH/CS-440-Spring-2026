using UnityEngine;

public class EruptAreaScript : MonoBehaviour
{
    public float damage = 40f;
    public float lifetime = 0.5f;

    void Start()
    {
        Destroy(gameObject, lifetime);
    }

    void OnTriggerEnter2D(Collider2D other)
    {
        WizardEnemyScript enemy = other.GetComponent<WizardEnemyScript>();
        if (enemy != null)
        {
            enemy.TakeDamage(damage);
        }

        WizardPlayerScript player = other.GetComponent<WizardPlayerScript>();
        if (player != null)
        {
            player.TakeDamage(damage);
        }
    }
}