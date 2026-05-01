using UnityEngine;

public class HealSpell : MonoBehaviour
{
    public float healAmount = 25f;
    public float cost = 30f;

    public WizardPlayerScript player;
    public PlayerEnergyScript playerEnergy;

    // void Update()
    // {
    //     if (Input.GetKeyDown(KeyCode.H))
    //     {
    //         CastHeal();
    //     }
    // }

    public void CastHeal()
    {
        if (player == null || playerEnergy == null)
        {
            Debug.LogError("Missing references!");
            return;
        }

        if (!playerEnergy.UseEnergy(cost))
        {
            Debug.Log("Not enough energy!");
            return;
        }

        player.Heal(healAmount);

        Debug.Log("Heal spell cast!");

    }
}