using Photon.Pun;
using System.Collections;
using UnityEngine;

public class PlayerController : MonoBehaviour
{
    [SerializeField] private float speed;
    [SerializeField] private UIManager uiManager;
    [SerializeField] internal int playerNum = 100;
    [SerializeField] private int health;
    [SerializeField] internal int points;
    [SerializeField] private float jumpforce;

    internal PhotonView photonView;
    private Animator anim;
    private Rigidbody2D rb;
    private SpriteRenderer sprite;
    private const string WALKING = "isWalking";
    private void Awake()
    {
        photonView = GetComponent<PhotonView>();
        anim = GetComponentInChildren<Animator>();
        rb = GetComponent<Rigidbody2D>();
        sprite = GetComponentInChildren<SpriteRenderer>();
        //photonView.RPC("AssignPlayerNumberRPC", RpcTarget.AllBuffered);
        GameManager.Instance.photonView.RPC("AddPlayerGameObjectRPC", RpcTarget.AllBuffered, playerNum, photonView.ViewID);
        uiManager = CheckForPlayerUI();
        if (uiManager != null)
        {
            uiManager.GetComponent<PhotonView>().RPC("SetHealthRPC", RpcTarget.All, health);
        }
    }

    //[PunRPC]
    //private void AssignPlayerNumberRPC() 
    //{
    //    if (PhotonNetwork.IsMasterClient)
    //    {
    //        this.playerNum = 0;
    //    }
    //    else 
    //    {
    //        this.playerNum = 1;
    //    }
    //    Debug.Log($"Hey, player {playerNum} is in town!");
    //}

    private void Start()
    {
        //Creating a custom property hashtable doesn't derive from System.Collections
        // but from ExitGames.Client.Photon;
        ExitGames.Client.Photon.Hashtable playerProperties = new ExitGames.Client.Photon.Hashtable();
        playerProperties.Add("PlayerNum", playerNum);
        playerProperties.Add("Points", points);
        PhotonNetwork.LocalPlayer.SetCustomProperties(playerProperties);


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
            uiManager.GetComponent<PhotonView>().RPC("SetHealthRPC", RpcTarget.AllBuffered, health);
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

    public void GetPoints() 
    {
        if (photonView.IsMine) 
        {
            GameManager.Instance.photonView.RPC("IncreaseScoreRPC", RpcTarget.AllBuffered, playerNum);
        }
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
