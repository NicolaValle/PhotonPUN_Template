using Photon.Pun;
using Photon.Realtime;
using System;
using System.Collections;
using System.Collections.Generic;
using TMPro;
using UnityEngine;

public class Enemy : MonoBehaviour
{
    //internal List<Player> players;
    private PlayerController nearestPlayer;
    private SpriteRenderer sprite;
    private bool checkForGround;
    private PhotonView photonView;

    [SerializeField] private float speed;
    [SerializeField] internal int life = 2;
    [SerializeField] internal TextMeshProUGUI lifeText;

    public void Start()
    {
        photonView = GetComponentInParent<PhotonView>();
        sprite = GetComponentInChildren<SpriteRenderer>();
        //players = GetPlayerList();
    }

    //private List<Player> GetPlayerList() 
    //{
    //    List<Player> playersList = new List<Player>(PhotonNetwork.PlayerList);
    //    Debug.Log(playersList);
    //    return playersList;
    //}

    private void Update()
    {
        lifeText.text = life.ToString();
        //if (players.Count > 1)
        //{

        //    float distanceOne = Vector2.Distance(transform.position, player0.transform.position);
        //    float distanceTwo = Vector2.Distance(transform.position, player1.transform.position);

        //    if (checkForGround)
        //    {
        //        if (distanceOne < distanceTwo)
        //        {
        //            nearestPlayer = players[0];
        //        }
        //        else
        //        {
        //            nearestPlayer = players[1];
        //        }

        //        if (nearestPlayer != null)
        //        {
        //            transform.position = Vector2.MoveTowards(transform.position, nearestPlayer.transform.position, speed * Time.deltaTime);
        //        }
        //    }
        //}
    }

    private void OnTriggerEnter2D(Collider2D collision)
    {
        if (collision.gameObject.name == "Ground") 
        {
            checkForGround = true;
        }

        if (collision.gameObject.GetComponent<PlayerController>())
        {
            Debug.Log("Damage player");
            collision.gameObject.GetComponent<PhotonView>().RPC("SetDamageTakenRPC", RpcTarget.All, life);
        }
    }

    [PunRPC]
    public void GetDamagedRPC(int playerNumber)
    {
        Debug.Log("Goblin hitted by Player" + playerNumber);
        //GameObject playerWhoHit = GameManager.Instance.GetPlayerGameObject(playerNumber);
        //Debug.Log($"Goblin damaged by {playerWhoHit.GetComponent<PlayerController>().playerNum}");
        //life--;
        //lifeText.text = life.ToString();
        //sprite.color = Color.red;
        //StartCoroutine(ResetColor());

        //if (life == 0)
        //{
        //    PhotonView playerNetworkInstance = playerWhoHit.GetComponent<PhotonView>();

        //    Debug.Log($"{playerNetworkInstance.ViewID} get the points");
        //    playerWhoHit.GetComponent<PlayerController>().GetPoints();
        //    Debug.Log($"Goblin killed by Player{playerNumber}");
        //    if (photonView.IsMine)
        //    {
        //        PhotonNetwork.Destroy(this.gameObject);
        //    }
        //}
    }

    private IEnumerator ResetColor()
    {
        yield return new WaitForSeconds(1);
        sprite.color = Color.white;
    }
}
