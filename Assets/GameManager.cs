using ExitGames.Client.Photon;
using Photon.Pun;
using Photon.Realtime;
using System;
using System.Collections.Generic;
using System.Linq;
using TMPro;
using UnityEngine;

[Photon.Pun.PunRPC]
public class GameManager : MonoBehaviourPunCallbacks
{
    public static GameManager Instance;
    public PhotonView photonView;
    [SerializeField] internal List<PhotonView> playersList;
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

    public void AddPlayerToArray(PhotonView player) 
    {
        playersList.Add(player);
        player.RPC("AssignPlayerNumberRPC", RpcTarget.All);
    }

    public int AssignPlayerNumber(int photonViewID) 
    {
        int index = 0;
        Debug.Log("PhotonViewID = " + photonViewID);
        foreach (var i in playersList) 
        {
            Debug.Log("PlayersList contain : " + i);
        }
        var list = playersList
            .Select((player, index) => new KeyValuePair<PhotonView, int>(player, index));

        foreach (var i in list)
        {
            Debug.Log("list contain : " + i);
            index = i.Value;
        }
        return index;
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
