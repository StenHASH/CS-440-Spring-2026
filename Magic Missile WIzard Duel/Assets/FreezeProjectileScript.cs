using UnityEngine;

public class FreezeProjectileScript : MonoBehaviour
{
    public float freezeDuration = 3f;
    public float lifetime = 4f;
    private bool hasHit = false;

    void Start()
    {
        Destroy(gameObject, lifetime);
    }

    void OnTriggerEnter2D(Collider2D other)
{
    if (other.gameObject.GetComponent<WizardPlayerScript>() != null) return;
    if (other.gameObject.GetComponentInParent<WizardPlayerScript>() != null) return;

    if (hasHit) return;
    hasHit = true;

    WizardEnemyScript enemy = other.gameObject.GetComponentInParent<WizardEnemyScript>();
    if (enemy != null)
    {
        Debug.Log("Enemy frozen!");
        enemy.Freeze(freezeDuration);
    }

    Destroy(gameObject);
}
}