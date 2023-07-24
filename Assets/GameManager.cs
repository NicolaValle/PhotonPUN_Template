using ExitGames.Client.Photon;
using Photon.Pun;
using Photon.Realtime;
using System.Collections.Generic;
using TMPro;
using UnityEngine;

[Photon.Pun.PunRPC]
public class GameManager : MonoBehaviourPunCallbacks
{
    public static GameManager Instance;
    public PhotonView photonView;
    [SerializeField] internal UIManager uiManagerPlayer1;
    [SerializeField] internal UIManager uiManagerPlayer2;

    // Dizionario per mappare il playerNum ai GameObjects
    private Dictionary<int, GameObject> playerGameObjectMap = new Dictionary<int, GameObject>();

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
        photonView = gameObject.GetComponent<PhotonView>();
    }

    [PunRPC]
    public void IncreaseScoreRPC(int playerNumber)
    {
        GameObject playerGameObject = GetPlayerGameObject(playerNumber);
        PlayerController player = playerGameObject.GetComponent<PlayerController>();
        player.points++;
        UIManager.Instance.UpdatePointsUI(player.points, playerNumber);
    }

    [PunRPC]
    // Metodo per aggiungere il GameObject del giocatore al dizionario
    public void AddPlayerGameObjectRPC(int playerNum, int viewID)
    {
        if (!playerGameObjectMap.ContainsKey(playerNum))
        {
            playerGameObjectMap.Add(playerNum, PhotonView.Find(viewID).gameObject);
        }
    }

    // Metodo per ottenere il GameObject del giocatore dal playerNum
    public GameObject GetPlayerGameObject(int playerNum)
    {
        if (playerGameObjectMap.ContainsKey(playerNum))
        {
            return playerGameObjectMap[playerNum];
        }
        else 
        {
            return null;
        }
    }
}
