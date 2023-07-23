using Photon.Pun;
using System.Collections;
using TMPro;
using UnityEngine;

public class Enemy : MonoBehaviour
{
    internal PlayerController[] players;
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


    }

    private void Update()
    {
        //Debug.Log($"Il numero di player e' {players.Length}");
        //if (this.isActiveAndEnabled) 
        //{
        //    for (int i = 0; i == players.Length; i++)
        //    {
        //        Debug.Log(players[i].name);
        //    }
        //}


        lifeText.text = life.ToString();
        if (players.Length > 1) 
        {
            GameObject player0 = players[0].gameObject;
            GameObject player1 = players[1].gameObject;
            float distanceOne = Vector2.Distance(transform.position, player0.transform.position);
            float distanceTwo = Vector2.Distance(transform.position, player1.transform.position);

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

            //if (headBC.IsTouching(player0.GetComponent<BoxCollider2D>()))
            //{
            //    Debug.Log("Damage goblin");
            //    this.GetComponent<PhotonView>().RPC("GetDamagedRPC", RpcTarget.All, players[0]);
            //}
            //else if (headBC.IsTouching(player1.GetComponent<BoxCollider2D>())) 
            //{
            //    Debug.Log("Damage goblin");
            //    this.GetComponent<PhotonView>().RPC("GetDamagedRPC", RpcTarget.All, players[1]);
            //}

            //if (bodyBC.IsTouching(player0.GetComponent<BoxCollider2D>()))
            //{
            //    Debug.Log("Damage player");
            //    player0.GetComponent<PhotonView>().RPC("SetDamageTakenRPC", RpcTarget.All, life);
            //}
            //else if (bodyBC.IsTouching(player1.GetComponent<BoxCollider2D>()))
            //{
            //    Debug.Log("Damage player");
            //    player1.GetComponent<PhotonView>().RPC("SetDamageTakenRPC", RpcTarget.All, life);
            //}
        }
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
    private void GetDamagedRPC(int playerNumber)
    {
        PlayerController playerWhoHit = players[playerNumber];
        Debug.Log("Damage goblin");
        life--;
        lifeText.text = life.ToString();
        sprite.color = Color.red;
        StartCoroutine(ResetColor());

        if (life == 0)
        {
            PhotonView playerInstance = playerWhoHit.GetComponent<PhotonView>();

            //playerWhoHit.GetComponent<Rigidbody2D>().AddForce(Vector2.up * 3, ForceMode2D.Impulse);
            playerInstance.RPC("GetPointsRPC", RpcTarget.All);
            Debug.Log($"Goblin killed by Player{playerNumber}");
            if (photonView.IsMine) 
            {
                PhotonNetwork.Destroy(this.gameObject);
            }
        }
    }

    private IEnumerator ResetColor()
    {
        yield return new WaitForSeconds(1);
        sprite.color = Color.white;
    }
}
