using System.Collections;
using System.Collections.Generic;
using TMPro;
using UnityEngine;
using Photon.Pun;

public class UIManager : MonoBehaviourPunCallbacks
{
    public static UIManager Instance;
    [SerializeField] internal TextMeshProUGUI pointsDisplay1;
    [SerializeField] internal TextMeshProUGUI pointsDisplay2;

    private void Awake()
    {
        if (Instance == null)
        {
            Instance = this;
        }
        else
        {
            Destroy(gameObject);
        }
        pointsDisplay1.text = $"Points: {0}";
        pointsDisplay2.text = $"Points: {0}";
    }

    public void UpdatePointsUI(int points, int playerNum)
    {
        if (playerNum == 0)
        {
            pointsDisplay1.text = $"Points: {points}";
        }
        else 
        {
            pointsDisplay2.text = $"Points: {points}";
        }
    }
}
