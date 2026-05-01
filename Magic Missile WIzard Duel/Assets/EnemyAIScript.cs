using UnityEngine;

public class EnemyAIScript : MonoBehaviour
{
    // Enemy Pause Timing
    [Header("Timing")]
    private float minDelay = 1f;
    private float maxDelay = 2f;

    // Enemy Charge Times
    [Header("Charge Time")]
    private float minChargeTime = 0.75f;
    private float maxChargeTime = 3f;

    // Enemy ChargeSpell connection
    [Header("EnemyChargeSpell")]
    public EnemyChargeSpellScript chargeSpell;

    // Enemy WallSpell connection
    [Header("EnemyWallSpell")]
    public EnemyWallDefenseScript wallSpell;

    // Timing variables for AI
    [Header("EnemyTimer")]
    private float actionTimer;
    private bool isCharging = false;
    private float chargeTimer;

    // Enemy state variables
    [Header("EnemyState")]
    private bool isDead;

    // Movement variables
    private float minX = 2f;
    private float maxX = 7.5f;
    public float moveSpeed = 10f;

    public Rigidbody2D rb;
    private Vector3 targetPosition;
    private bool isMoving = false;

    // Start is called once before the first execution of Update after the MonoBehaviour is created
    void Start()
    {
        isDead = false;
        ResetTimer();
    }

    // Update is called once per frame
    void Update()
    {
        if (isDead) return;

        if (isCharging)
        {
            // Stop movement while charging
            isMoving = false;

            chargeTimer -= Time.deltaTime;

            if (chargeTimer <= 0)
            {
                chargeSpell.ReleaseCharge();
                isCharging = false;
                ResetTimer();
            }

            // Cannot do anything else while recharging
            return; 
        }

        // Handle movement
        if (isMoving)
        {
            rb.MovePosition(Vector2.MoveTowards(
                rb.position,
                targetPosition,
                moveSpeed * Time.deltaTime
                ));

            if (Vector2.Distance(rb.position, targetPosition) < 0.05f)
            {
                isMoving = false;
            }
        }

        // Handle decision timer
        actionTimer -= Time.deltaTime;
        if (actionTimer <= 0)
        {
            DecideAction();
        }
    }

    void DecideAction()
    {
        // Roll a number from 0 - 10
        float roll = Random.value;

        // Attack
        if (roll <= 0.33f)
        {
            StartCharge();
        }
        // Move
        else if (roll <= 0.66f)
        {
            MoveRandom();
        }      
        // Defend
        else
        {
            TryWall();
        }
    }

    void TryWall()
    {
        if (wallSpell != null)
        {
            wallSpell.TryCastWall();
        }

        // Immediately go back to waiting after casting
        ResetTimer();
    }

    void StartCharge()
    {
        chargeSpell.StartCharging();

        isCharging = true;
        chargeTimer = Random.Range(minChargeTime, maxChargeTime);
    }

    void ResetTimer()
    {
        actionTimer = Random.Range(minDelay, maxDelay);
    }

    public void KillAI()
    {
        isDead = true;
    }

    void MoveRandom()
    {
        float randomX = Random.Range(minX, maxX);

        targetPosition = new Vector3(
            randomX,
            transform.position.y,
            transform.position.z 
            );

        isMoving = true;

        // After deciding to move, reset action timer
        ResetTimer();
    }
}
