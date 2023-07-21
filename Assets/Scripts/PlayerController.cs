using Photon.Pun;
using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class PlayerController : MonoBehaviour
{
    [SerializeField] private float speed;
    [SerializeField] private UIManager uiManager;
    [SerializeField] internal int playerNum;
    [SerializeField] private int health;
    [SerializeField] private int points;
    [SerializeField] private float jumpforce;

    private PhotonView photonView;
    private Animator anim;
    private Rigidbody2D rb;
    private BoxCollider2D bc;
    private SpriteRenderer sprite;
    private const string WALKING = "isWalking";
    private void Awake()
    {
        photonView = GetComponent<PhotonView>();
        anim = GetComponentInChildren<Animator>();
        rb = GetComponent<Rigidbody2D>();
        sprite = GetComponentInChildren<SpriteRenderer>();
        bc = GetComponent<BoxCollider2D>();
        if (PhotonNetwork.IsMasterClient)
        {
            playerNum = 0;
        }
        else 
        {
            playerNum = 1;
        }
        uiManager = CheckForPlayerUI();
        uiManager.GetComponent<PhotonView>().RPC("SetHealthRPC", RpcTarget.All, health);
    }

    private UIManager CheckForPlayerUI()
    {
        UIManager healthUI = null;

        if (playerNum == 0)
        {
            const string UI_PLAYER_1 = "UIPlayer1";
            GameObject masterUI = GameObject.Find(UI_PLAYER_1);
            if (masterUI.name == UI_PLAYER_1) 
            {
                healthUI = masterUI.GetComponent<UIManager>();
            }

        }
        else if (playerNum == 1)
        {
            const string UI_PLAYER_2 = "UIPlayer2";
            GameObject clientUI = GameObject.Find(UI_PLAYER_2);
            if (clientUI.name == UI_PLAYER_2)
            {
                healthUI = clientUI.GetComponent<UIManager>();
            }
        }
        return healthUI;
    }

    private void Update()
    {
        if (photonView.IsMine) 
        {
            HorizontalMovement();
            Jump();
        }
    }

    [PunRPC]
    public void SetDamageTakenRPC(int damage) 
    {
        if (photonView.IsMine) 
        {
            sprite.color = Color.red;
            //bc.enabled = false;
            StartCoroutine(EnableBC());
            Debug.Log($"Tolgo vita a Player n.{this.playerNum}");
            health -= damage;
            uiManager.GetComponent<PhotonView>().RPC("SetHealthRPC", RpcTarget.All, health);
        }
    }

    private IEnumerator EnableBC()
    {
        yield return new WaitForSeconds(1.5f);
        //bc.enabled = true;
        photonView.RPC("ChangeColorBack", RpcTarget.All);
    }

    [PunRPC]
    private void ChangeColorBack() 
    {
        sprite.color = Color.white;
    }

    [PunRPC]
    public void GetPointsRPC() 
    {
        points++;
        uiManager.GetComponent<PhotonView>().RPC("GetPointsRPC", RpcTarget.All, points);
    }

    private void HorizontalMovement() 
    {
        Vector2 moveInput = new Vector2(Input.GetAxisRaw("Horizontal"), 0);
        Vector2 moveSpeed = moveInput.normalized * speed * Time.deltaTime;
        transform.position += (Vector3)moveSpeed;

        if (moveInput == Vector2.zero)
        {
            anim.SetBool(WALKING, false);
        }
        else
        {
            anim.SetBool(WALKING, true);
        }
    }

    private void Jump()
    {
        bool jumpInput = Input.GetKeyDown(KeyCode.Space);

        if (jumpInput) 
        {
            rb.AddForce(Vector2.up * jumpforce, ForceMode2D.Impulse);
        }
    }


    //private void OnCollisionEnter2D(Collision2D collision)
    //{
    //    if (photonView.IsMine) 
    //    {
    //        if (collision.gameObject.GetComponent<Enemy>())
    //        {
    //            uiManager.GetComponent<PhotonView>().RPC("SetHealthRPC", RpcTarget.All, health);
    //        }
    //    }
    //}
}
