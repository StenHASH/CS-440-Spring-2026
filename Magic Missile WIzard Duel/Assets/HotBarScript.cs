using UnityEngine;
using UnityEngine.UI;

public class HotbarScript : MonoBehaviour
{
    // Spell References
    [Header("Spells")]
    public PlayerChargeSpellScript chargeSpell;
    public HealSpell healSpell;
    public WallDefenseSpellScript wallSpell;
    public EruptSpellScript eruptSpell;

    // Hotbar Settings
    [Header("Hotbar Settings")]
    public int slotCount = 4;

    // UI References
    [Header("UI References")]
    public Image[] slotImages;
    public Sprite emptySlotSprite;

    // Selection
    [Header("Selection")]
    public int selectedSlot = 0;
    public Color selectedColor = Color.yellow;
    public Color defaultColor = Color.white;

    // Slot 0 = Charge Spell
    // Slot 1 = Heal Spell
    // Slot 2 = Wall Spell
    // Slot 3 = Erupt Spell

    void Start()
    {
        RefreshAllSlots();
        ApplySelectedSlot();
    }

    void Update()
    {
        HandleHotkeyInput();
        HandleScrollInput();
        HandleCastInput();
    }

    void HandleHotkeyInput()
    {
        if (Input.GetKeyDown(KeyCode.Alpha1)) SelectSlot(0);
        if (Input.GetKeyDown(KeyCode.Alpha2)) SelectSlot(1);
        if (Input.GetKeyDown(KeyCode.Alpha3)) SelectSlot(2);
        if (Input.GetKeyDown(KeyCode.Alpha4)) SelectSlot(3);
    }

    void HandleScrollInput()
    {
        float scroll = Input.GetAxis("Mouse ScrollWheel");
        if (scroll > 0f) SelectSlot((selectedSlot - 1 + slotCount) % slotCount);
        else if (scroll < 0f) SelectSlot((selectedSlot + 1) % slotCount);
    }

    void HandleCastInput()
    {
        // Erupt uses hold-to-aim on left click
        if (selectedSlot == 3 && eruptSpell != null)
        {
            if (Input.GetMouseButtonDown(0)) eruptSpell.StartAiming();
            if (Input.GetMouseButtonUp(0)) eruptSpell.CastErupt();
            return;
        }

        // All other spells activate on left click down
        if (!Input.GetMouseButtonDown(0)) return;

        switch (selectedSlot)
        {
            case 0:
                // Charge spell self-handles input while enabled
                break;
            case 1:
                if (healSpell != null) healSpell.CastHeal();
                break;
            case 2:
                if (wallSpell != null) wallSpell.TryCastWall();
                break;
        }
    }

    void SelectSlot(int index)
    {
        if (index < 0 || index >= slotCount) return;
        if (index == selectedSlot) return;

        // If switching away from erupt while aiming, cancel it
        if (selectedSlot == 3 && eruptSpell != null && eruptSpell.isAiming)
        {
            eruptSpell.CancelAim();
        }

        selectedSlot = index;
        ApplySelectedSlot();
    }

    void ApplySelectedSlot()
    {
        if (chargeSpell != null) chargeSpell.enabled = false;
        if (healSpell != null) healSpell.enabled = false;
        if (wallSpell != null) wallSpell.enabled = false;
        if (eruptSpell != null) eruptSpell.enabled = false;

        switch (selectedSlot)
        {
            case 0: if (chargeSpell != null) chargeSpell.enabled = true; break;
            case 1: if (healSpell != null) healSpell.enabled = true; break;
            case 2: if (wallSpell != null) wallSpell.enabled = true; break;
            case 3: if (eruptSpell != null) eruptSpell.enabled = true; break;
        }

        RefreshHighlight();
    }

    void RefreshHighlight()
    {
        for (int i = 0; i < slotCount; i++)
        {
            if (slotImages == null || i >= slotImages.Length || slotImages[i] == null) continue;
            slotImages[i].color = (i == selectedSlot) ? selectedColor : defaultColor;
        }
    }

    void RefreshAllSlots()
    {
        for (int i = 0; i < slotCount; i++)
        {
            if (slotImages == null || i >= slotImages.Length || slotImages[i] == null) continue;
            slotImages[i].color = defaultColor;
        }
    }
}