using TMPro;
using UnityEngine;
using UnityEngine.UI;

public class EnemyChargeSpellScript : MonoBehaviour
{
    // Charge UI objects
    [Header("Charge UI")]
    public GameObject enemyChargeBarContainer;
    public Image enemyChargeBarFill;
    public TextMeshProUGUI enemyChargeText;

    // Projectile related objects
    [Header("Projectile")]
    public GameObject projectilePrefab;
    public Transform enemyFirePoint;

    // Spawn offset
    [Header("Spawn Offset")]
    public float spawnDistance = 0.5f;

    // Charge settings
    [Header("Charge Settings")]
    public float maxChargeTime = 2f;
    public float maxLaunchForce = 2f;

    // Energy variables
    [Header("Energy")]
    public EnemyEnergyScript enemyEnergy;
    private float energyCost = 20f;

    // Spell Control variables
    [Header("Control")]
    private float currentCharge = 0f;
    private bool isCharging = false;

    [Header("Targeting")]
    public Transform target;

    void Update()
    {
        if (isCharging)
        {
            UpdateCharge();
        }
    }

    // Starts the charging state of the Enemy's energy bar
    public void StartCharging()
    {
        isCharging = true;
        currentCharge = 0f;

        enemyChargeBarContainer.SetActive(true);
    }

    // Updates the charge of the Enemy's energy bar
    void UpdateCharge()
    {
        currentCharge += Time.deltaTime;
        currentCharge = Mathf.Clamp(currentCharge, 0f, maxChargeTime);

        float percent = currentCharge / maxChargeTime;

        enemyChargeBarFill.fillAmount = percent;
        enemyChargeText.text = Mathf.RoundToInt(percent * 100f) + "%";
        
    }

    // Triggers firing and resets energy bar
    public void ReleaseCharge()
    {
        if (!isCharging) return;

        Fire();
        isCharging = false;
        enemyChargeBarContainer.SetActive(false);
    }

    // Creates and launches projectile object
    void Fire()
    {
        // Check if enough energy
        if (!UseEnemyEnergy()) return;

        // launch force calculation
        float chargePercent = currentCharge / maxChargeTime;
        float launchForce = chargePercent * maxLaunchForce;
        
        // Check if target is assigned
        if (target == null)
        {
            Debug.LogWarning("No target assigned for AI!");
            return;
        }

        // Aiming calculation
        Vector2 baseDirection = (target.position - enemyFirePoint.position).normalized;

        // Add some upward direction between a range to account for bullet drop
        float yAdd = Random.Range(0.2f, 0.4f);
        Vector2 direction = new Vector2(baseDirection.x, baseDirection.y + yAdd).normalized; 

        // Spawn and launch projectile
        LaunchProjectile(direction, launchForce);
    }

    // Spawns the projectile and launches from the appropriate firepoint
    void LaunchProjectile(Vector2 direction, float launchForce)
    {
        // Apply spawn offset distance
        Vector2 spawnPosition = (Vector2)enemyFirePoint.position + direction * spawnDistance;
        
        // Instantiate projectile
        GameObject projectile = Instantiate(projectilePrefab, spawnPosition, Quaternion.identity);

        // Launch projectile
        Rigidbody2D rb = projectile.GetComponent<Rigidbody2D>();
        rb.linearVelocity = direction * launchForce * 10f;

        // Spin effect
        rb.angularVelocity = 720f;
    }

    // Checks if the enemy wizard's energy is enough to cast the spell
    bool UseEnemyEnergy()
    {
        if (enemyEnergy != null)
        {
            return enemyEnergy.UseEnergy(energyCost);
        }
        return false;
    }
}
