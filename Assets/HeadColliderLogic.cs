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
<<<<<<< HEAD
            photonView.RPC("GetDamagedRPC", RpcTarget.All, playerNum);
=======
            enemy.GetDamaged(playerNum);
>>>>>>> 3c2dd09023cc3ac803ef18f32f26dfba998bb4f1
        }
    }
}
