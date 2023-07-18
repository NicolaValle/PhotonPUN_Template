using Photon.Pun;
using Photon.Realtime;
using System.Collections;
using System.Collections.Generic;
using TMPro;
using UnityEngine;
using UnityEngine.UI;

public class RoomSelection : MonoBehaviourPunCallbacks
{
    [SerializeField] private TMP_InputField createRoomInputField;
    [SerializeField] private TMP_InputField joinRoomInputField;
    [SerializeField] private Button createRoomButton;
    [SerializeField] private Button joinRoomButton;

    private void Awake()
    {
        createRoomButton.onClick.AddListener(CreateRoom);
        joinRoomButton.onClick.AddListener(JoinRoom);
    }

    public void CreateRoom() 
    {
        RoomOptions roomOptions = new RoomOptions();
        roomOptions.MaxPlayers = 2;
        PhotonNetwork.CreateRoom(createRoomInputField.text, roomOptions);
    }

    public void JoinRoom() 
    {
        PhotonNetwork.JoinRoom(joinRoomInputField.text);
    }

    public override void OnJoinedRoom()
    {
        //Called automatically, call the function underneath instead of SceneManage
        // whenever there are other players in the scene 
        PhotonNetwork.LoadLevel("GameScene");
    }
}
