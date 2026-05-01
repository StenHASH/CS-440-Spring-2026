using TMPro;
using UnityEngine.UI;
using UnityEngine;

public class EnemyEnergyScript : MonoBehaviour
{
    [Header("Energy")]
    [SerializeField] private float maxEnergy = 100f;
    [SerializeField] private float currentEnergy = 0f;
    [SerializeField] private float energyRegenPerSecond = 10f;

    [Header("UI")]
    [SerializeField] private Image energyBarFill;
    [SerializeField] private TextMeshProUGUI energyBarText;
    public float CurrentEnergy => currentEnergy;
    public float MaxEnergy => maxEnergy;

    // Start is called once before the first execution of Update after the MonoBehaviour is created
    void Start()
    {
        // Stops energy from exceeding 100 or becoming negative
        currentEnergy = Mathf.Clamp(currentEnergy, 0f, maxEnergy);
        UpdateEnergyUI();
    }

    // Update is called once per frame
    void Update()
    {
        RegenerateEnergy();
    }

    // Replenishes the current energy available to the user
    private void RegenerateEnergy()
    {
        if (energyRegenPerSecond <= 0f) return;
        if (currentEnergy >= maxEnergy) return;

        currentEnergy += energyRegenPerSecond * Time.deltaTime;
        currentEnergy = Mathf.Clamp(currentEnergy, 0f, maxEnergy);

        UpdateEnergyUI();
    }

    // Call this from any spell script. Returns true if energy was spent and false if 
    //  there is not enough energy available to spend on spell
    public bool UseEnergy(float amount)
    {
        if (amount <= 0f) return true;

        if (currentEnergy < amount)
        {
            Debug.Log("Enemy: Not enough energy!");
            return false;
        }

        currentEnergy -= amount;
        currentEnergy = Mathf.Clamp(currentEnergy, 0f, maxEnergy);

        UpdateEnergyUI();
        return true;
    }

    // Updates the UI (fills and drains the energy bar)
    private void UpdateEnergyUI()
    {
        if (energyBarFill != null)
            energyBarFill.fillAmount = currentEnergy / maxEnergy;

        if (energyBarText != null)
            energyBarText.text = $"{Mathf.CeilToInt(currentEnergy)} / {Mathf.CeilToInt(maxEnergy)}";
    }
}
