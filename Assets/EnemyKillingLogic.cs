using Photon.Pun;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class EnemyKillingLogic : MonoBehaviour
{
    private Enemy enemyLogic;
    private PhotonView photonView;

    private void Awake()
    {
        enemyLogic = GetComponentInChildren<Enemy>();
        photonView = GetComponent<PhotonView>();
    }

    private void OnTriggerEnter2D(Collider2D collision)
    {
        PhotonView playerWhoJumpOn = collision.gameObject.GetComponent<PhotonView>();
        if (playerWhoJumpOn)
        {
            photonView.RPC("GetDamagedRPC", RpcTarget.All, playerWhoJumpOn.gameObject);
        }
    }

    [PunRPC]
    private void GetDamagedRPC(GameObject playerWhoJumpOn)
    {
        enemyLogic.life--;
        enemyLogic.lifeText.text = enemyLogic.life.ToString();
        if (enemyLogic.life == 0)
        {
            int playerNum = playerWhoJumpOn.GetComponent<PlayerController>().playerNum;
            PhotonView playerInstance = playerWhoJumpOn.GetComponent<PhotonView>();

            playerWhoJumpOn.GetComponent<Rigidbody2D>().AddForce(Vector2.up * 3, ForceMode2D.Impulse);
            playerInstance.RPC("GetPointsRPC", RpcTarget.All);
            Debug.Log($"Goblin killed by Player{playerNum}");
            PhotonNetwork.Destroy(gameObject);

        }
    }
}
