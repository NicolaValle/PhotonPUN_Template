using System.Collections;
using System.Collections.Generic;
using TMPro;
using UnityEngine;
using Photon.Pun;

public class UIManager : MonoBehaviourPunCallbacks
{
    [SerializeField] private int health;
    [SerializeField] private int points;
    private TextMeshProUGUI healthDisplay;
    private TextMeshProUGUI pointsDisplay;

    private void Awake()
    {
        healthDisplay = transform.GetChild(0).GetComponent<TextMeshProUGUI>();
        healthDisplay.text = $"Life: {health}";
        pointsDisplay = transform.GetChild(1).GetComponent<TextMeshProUGUI>();
        pointsDisplay.text = $"Points: {points}";
    }

    [PunRPC]
    public void SetHealthRPC(int health) 
    {
        healthDisplay.text = $"Life: {health}";
        if (health == 0)
        {
            Debug.Log("Sei morto");
        }
    }


    [PunRPC]
    public void GetPointsRPC(int points)
    {
        if (PhotonNetwork.IsMasterClient) 
        {
            pointsDisplay.text = $"Points: {points}";
        }
    }
}
