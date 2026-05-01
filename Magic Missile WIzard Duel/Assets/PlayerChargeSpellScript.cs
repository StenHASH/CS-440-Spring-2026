using UnityEngine;
using UnityEngine.UI;
using TMPro;

public class PlayerChargeSpellScript : MonoBehaviour
{
    // Charge UI objects
    [Header("Charge UI")]
    public GameObject chargeBarContainer;
    public Image chargeBarFill;
    public TextMeshProUGUI chargeText;

    // Projectile related objects
    [Header("Projectile")]
    public GameObject projectilePrefab;
    public Transform firePoint;

    // Spawn offset
    [Header("Spawn Offset")]
    public float spawnDistance = 0.5f;

    // Charge settings
    [Header("Charge Settings")]
    public float maxChargeTime = 2f;
    public float maxLaunchForce = 2f;

    // Charge variables
    private float currentCharge = 0f;
    private bool isCharging = false;
    public PlayerEnergyScript playerEnergy;
    public float energyCost = 20f;


    void Update()
    {
        HandleInput();
    }

    // Handles the process between starting charge, charging and firing
    void HandleInput()
    {
        // Start charging
        if (Input.GetMouseButtonDown(0))
        {
            isCharging = true;
            currentCharge = 0f;
            chargeBarContainer.SetActive(true);
        }

        // While holding mouse button
        if (Input.GetMouseButton(0) && isCharging)
        {
            currentCharge += Time.deltaTime;
            currentCharge = Mathf.Clamp(currentCharge, 0f, maxChargeTime);

            float percent = currentCharge / maxChargeTime;

            chargeBarFill.fillAmount = percent;
            chargeText.text = Mathf.RoundToInt(percent * 100f) + "%";
        }

        // Release mouse button ? Fire
        if (Input.GetMouseButtonUp(0) && isCharging)
        {
            Fire();
            isCharging = false;
            chargeBarContainer.SetActive(false);
        }
    }

    // Creates and launches projectile object
    void Fire()
    {

        if (!playerEnergy.UseEnergy(energyCost))
        {
            return;
        }

        float chargePercent = currentCharge / maxChargeTime;
        float launchForce = chargePercent * maxLaunchForce;

        // Get mouse position in world
        Vector3 mouseWorld = Camera.main.ScreenToWorldPoint(Input.mousePosition);
        mouseWorld.z = 0f;

        // Calculate direction from fire point to mouse
        Vector2 direction = (mouseWorld - firePoint.position).normalized;

        // Apply spawn offset distance
        Vector2 spawnPosition = (Vector2)firePoint.position + direction * spawnDistance;

        // Instantiate projectile
        GameObject projectile = Instantiate(projectilePrefab, spawnPosition, Quaternion.identity);

        // Tags projectile as Player's
        projectile.tag = "PlayerAttack";

        // Launch projectile
        Rigidbody2D rb = projectile.GetComponent<Rigidbody2D>();
        rb.linearVelocity = direction * launchForce * 10f;

        // Optional spin effect
        rb.angularVelocity = 720f;
    }
}
