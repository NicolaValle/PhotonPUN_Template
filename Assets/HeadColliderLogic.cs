using Photon.Pun;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class HeadColliderLogic : MonoBehaviour
{
    [SerializeField] private PhotonView photonView;
    [SerializeField] private Enemy enemy;

    private void OnCollisionEnter2D(Collision2D collision)
    {
        PlayerController player = collision.gameObject.GetComponent<PlayerController>();
        int playerNum = player.playerNum;
        if (player) 
        {
            //Debug.Log(enemy.players[0]);
            //Debug.Log(enemy.players[1]);
            Debug.Log("PlayerNum = " + playerNum);
            photonView.RPC("GetDamagedRPC", RpcTarget.All, playerNum);
        }
    }
}
