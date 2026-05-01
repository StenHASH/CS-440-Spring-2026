using UnityEngine;

public class EnemyWallDefenseScript : MonoBehaviour
{
    // Wall Object
    [Header("Wall Prefab")]
    public GameObject wallPrefab;

    // Spawn variables
    [Header("Spawn Settings")]
    public Transform castPoint;          // where the wall spawns from (near wizard)
    public float distanceInFront = 2.5f; // how far in front of wizard
    public float wallDuration = 3f;

    // Cooldown variables
    [Header("Cooldown")]
    public float cooldown = 2f;
    private float nextCastTime = 0f;

    // Checks for cooldown on wall spell
    public void TryCastWall()
    {
        if (Time.time < nextCastTime) return;
        nextCastTime = Time.time + cooldown;

        CastWall();
    }

    // Gathers all positions of all existing projectiles fired by player and finds the closest
    Transform FindNearestProjectile()
    {
        GameObject[] projectiles = GameObject.FindGameObjectsWithTag("PlayerAttack");

        Transform closest = null;
        float minDist = Mathf.Infinity;

        foreach (GameObject projectile in projectiles)
        {
            float dist = Vector2.Distance(transform.position, projectile.transform.position);

            // Ignores projectiles that have flown past the map
            if ((dist < minDist) && (projectile.transform.position.x < 15))
            {
                minDist = dist;
                closest = projectile.transform;
            }
        }
        return closest;
    }

    // Spawns the wall object to block projectiles
    void CastWall()
    {
        Transform projectile = FindNearestProjectile();

        Vector2 direction;

        // If a player projectile exists, aim towards it
        if (projectile != null)
        {
            direction = (projectile.position - castPoint.position).normalized;
        }
        // Otherwise cancel wall cast
        else { return; }

        // Spawn infront of enemy wizard
        Vector3 spawnPos = castPoint.position + (Vector3)(direction * distanceInFront);

        // Optional rotate wall to face direction
        float angle = Mathf.Atan2(direction.y, direction.x) * Mathf.Rad2Deg;
        Quaternion rot = Quaternion.Euler(0f, 0f, angle);

        GameObject wall = Instantiate(wallPrefab, spawnPos, rot);

        // Set lifetime on the wall
        WallDefenseScript ws = wall.GetComponent<WallDefenseScript>();
        if (ws != null)
        {
            ws.lifetime = wallDuration;
        }
    }
}
