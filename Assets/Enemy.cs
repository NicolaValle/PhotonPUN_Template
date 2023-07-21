using Photon.Pun;
using System.Collections;
using System.Collections.Generic;
using TMPro;
using UnityEngine;

public class Enemy : MonoBehaviour
{
    private PlayerController[] players;
    private PlayerController nearestPlayer;
    [SerializeField] private float speed;
    [SerializeField] internal int life = 2;
    [SerializeField] internal TextMeshProUGUI lifeText;
    private bool checkForGround;
    private PhotonView photonView;

    public void Start()
    {
        players = FindObjectsOfType<PlayerController>();
        photonView = GetComponentInParent<PhotonView>();
    }

    private void Update()
    {
        lifeText.text = life.ToString();
        float distanceOne = Vector2.Distance(transform.position, players[0].transform.position);
        float distanceTwo = Vector2.Distance(transform.position, players[1].transform.position);

        if (checkForGround)
        {
            if (distanceOne < distanceTwo)
            {
                nearestPlayer = players[0];
            }
            else
            {
                nearestPlayer = players[1];
            }

            if (nearestPlayer != null)
            {
                transform.position = Vector2.MoveTowards(transform.position, nearestPlayer.transform.position, speed * Time.deltaTime);
            }
        }
    }

    private void OnCollisionEnter2D(Collision2D collision)
    {
        PhotonView hittedPlayer = collision.gameObject.GetComponent<PhotonView>();
        if (hittedPlayer) 
        {
            hittedPlayer.RPC("SetDamageTakenRPC", RpcTarget.All, life);
        }

        if (collision.gameObject.name == "Ground") 
        {
            checkForGround = true;
        }
    }
}
