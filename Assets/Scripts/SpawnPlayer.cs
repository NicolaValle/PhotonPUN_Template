using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using Photon.Pun;

public class SpawnPlayer : MonoBehaviour
{
    [SerializeField] private GameObject player;
    private float edgesX = 5;

    private void Start()
    {
        Vector2 randomPosition = new Vector2(Random.Range(-edgesX, edgesX), 0);
        PhotonNetwork.Instantiate(player.name, randomPosition, Quaternion.identity);
    }
}
