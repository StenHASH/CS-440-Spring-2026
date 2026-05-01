using UnityEngine;

public class EruptSpellScript : MonoBehaviour
{
    [Header("References")]
    public GameObject eruptPrefab;
    public GameObject markerPrefab;
    public PlayerEnergyScript playerEnergy;

    [Header("Settings")]
    public float energyCost = 40f;
    public float markerSpeed = 6f;
    public float markerMinX = -7.5f;
    public float markerMaxX = 7.5f;
    public float groundY = -3f;

    public bool isAiming = false;
    private float markerX = 0f;
    private int markerDirection = 1;
    private GameObject activeMarker;

    void Update()
    {
        if (isAiming)
        {
            UpdateMarker();
        }
    }

    public void StartAiming()
    {
        isAiming = true;
        markerX = transform.position.x;
        markerDirection = 1;
        activeMarker = Instantiate(markerPrefab, new Vector3(markerX, groundY, 0f), Quaternion.identity);
    }

    public void UpdateMarker()
    {
        markerX += markerDirection * markerSpeed * Time.deltaTime;

        if (markerX >= markerMaxX)
        {
            markerX = markerMaxX;
            markerDirection = -1;
        }
        else if (markerX <= markerMinX)
        {
            markerX = markerMinX;
            markerDirection = 1;
        }

        if (activeMarker != null)
        {
            activeMarker.transform.position = new Vector3(markerX, groundY, 0f);
        }
    }

    public void CastErupt()
    {
        isAiming = false;

        if (activeMarker != null)
        {
            Destroy(activeMarker);
            activeMarker = null;
        }

        if (!playerEnergy.UseEnergy(energyCost))
        {
            Debug.Log("Not enough energy for Erupt!");
            return;
        }

        Vector3 spawnPos = new Vector3(markerX, groundY, 0f);
        Instantiate(eruptPrefab, spawnPos, Quaternion.identity);
    }
    public void CancelAim()
{
    isAiming = false;
    if (activeMarker != null)
    {
        Destroy(activeMarker);
        activeMarker = null;
    }
}
}
