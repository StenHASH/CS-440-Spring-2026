using UnityEngine;

public class FreezeSpellScript : MonoBehaviour
{
    [Header("References")]
    public GameObject freezeProjectilePrefab;
    public PlayerEnergyScript playerEnergy;
    public Transform firePoint;

    [Header("Settings")]
    public float energyCost = 35f;
    public float projectileSpeed = 12f;

    void Update()
    {
        if (Input.GetKeyDown(KeyCode.F))
        {
            CastFreeze();
        }
    }

    void CastFreeze()
    {
        if (!playerEnergy.UseEnergy(energyCost))
        {
            Debug.Log("Not enough energy for Freeze!");
            return;
        }

        Vector3 mousePos = Camera.main.ScreenToWorldPoint(Input.mousePosition);
        mousePos.z = 0f;

        Vector3 spawnPos = firePoint != null ? firePoint.position : transform.position;
        Vector3 direction = (mousePos - spawnPos).normalized;

        GameObject proj = Instantiate(freezeProjectilePrefab, spawnPos, Quaternion.identity);
        Rigidbody2D rb = proj.GetComponent<Rigidbody2D>();
        if (rb != null)
        {
            rb.linearVelocity = direction * projectileSpeed;
        }
    }
}