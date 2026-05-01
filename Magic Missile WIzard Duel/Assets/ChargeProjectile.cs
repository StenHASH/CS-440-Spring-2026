using UnityEditorInternal;
using UnityEngine;

public class ChargeProjectileScript : MonoBehaviour
{
    // Projectile variables
    public float lifetime = 5f;
    private float damage = 25f;

    // Explosion settings
    public Sprite explosionSprite;
    public float explosionDuration = 0.5f;
    private bool hasExploded = false;

    // Object settings
    private Rigidbody2D rb;
    private Collider2D col;
    private SpriteRenderer spriteRenderer;

    // Start is called once before the first execution of Update after the MonoBehaviour is created
    void Start()
    {
        // Attribute object components
        rb = GetComponent<Rigidbody2D>();
        col = GetComponent<Collider2D>();
        spriteRenderer = GetComponent<SpriteRenderer>();

        // Projectile is removed from screen after given lifetime in seconds
        Destroy(gameObject, lifetime);
    }

    // Update is called once per frame
    void Update()
    {
        
    }

    // Runs when the projectile touches another object on scene
    private void OnCollisionEnter2D(Collision2D collision)
    {
        // Prevents multiple explosion triggers
        if (hasExploded) return; 
        hasExploded = true;

        // If projectile collides with the enemy wizard, does damage
        WizardEnemyScript enemy = collision.gameObject.GetComponent<WizardEnemyScript>();
        if (enemy != null)
        {
            enemy.TakeDamage(damage);
        }
        // If projectile collides with the player wizard, does damage
        WizardPlayerScript player = collision.gameObject.GetComponent<WizardPlayerScript>();
        if (player != null)
        {
            player.TakeDamage(damage);
        }

        // Trigger explosion effect
        Explode();
    }

    // Displays explosion effects before destruction of object
    void Explode()
    {
        // Stop projectile movement
        if (rb != null)
        {
            rb.linearVelocity = Vector2.zero;
            rb.angularVelocity = 0f;
            rb.bodyType = RigidbodyType2D.Kinematic;
        }

        // Disable object collider
        if (col != null)
        {
            col.enabled = false;
        }

        // Change sprite to explosion
        spriteRenderer.sprite = explosionSprite;

        // Destroys projectile after explosion duration
        Destroy(gameObject, explosionDuration);
    }    
}
