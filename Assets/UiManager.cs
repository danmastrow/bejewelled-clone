using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class UiManager : MonoBehaviour
{
    public GemGridManager gemGridManager;
    public Text redKillCountText, blueKillCountText, greenKillCountText, purpleKillCountText, yellowKillCountText, crystalKillCountText;

    private void Awake()
    {
        gemGridManager.RedKillCountUpdated += RedKillCountUpdated;
        gemGridManager.BlueKillCountUpdated += BlueKillCountUpdated;
        gemGridManager.GreenKillCountUpdated += GreenKillCountUpdated;
        gemGridManager.PurpleKillCountUpdated += PurpleKillCountUpdated;
        gemGridManager.YellowKillCountUpdated += YellowKillCountUpdated;
        gemGridManager.CrystalCountUpdated += CrystalKillCountUpdated;
    }

    private void CrystalKillCountUpdated(int newCount)
    {
        crystalKillCountText.text = $"{newCount}";
    }
    private void RedKillCountUpdated(int newCount)
    {
        redKillCountText.text = $"{newCount}";
    }

    private void BlueKillCountUpdated(int newCount)
    {
        blueKillCountText.text = $"{newCount}";
    }


    private void GreenKillCountUpdated(int newCount)
    {
        greenKillCountText.text = $"{newCount}";
    }

    private void PurpleKillCountUpdated(int newCount)
    {
        purpleKillCountText.text = $"{newCount}";
    }

    private void YellowKillCountUpdated(int newCount)
    {
        yellowKillCountText.text = $"{newCount}";
    }


    private void OnDestroy()
    {
        gemGridManager.RedKillCountUpdated -= RedKillCountUpdated;
        gemGridManager.BlueKillCountUpdated -= BlueKillCountUpdated;
        gemGridManager.GreenKillCountUpdated -= GreenKillCountUpdated;
        gemGridManager.PurpleKillCountUpdated -= PurpleKillCountUpdated;
        gemGridManager.YellowKillCountUpdated -= YellowKillCountUpdated;
        gemGridManager.CrystalCountUpdated -= CrystalKillCountUpdated;

    }
}
