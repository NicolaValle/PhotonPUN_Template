using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using Photon.Pun;

public class NetworkSpawner : MonoBehaviour
{
    [SerializeField] private GameObject player;
    private float minX = -8, maxX = 8;

    private void Start()
    {
        Vector2 randomPosition = new Vector2(Random.Range(minX, maxX), 0);
        PhotonNetwork.Instantiate(player.name, randomPosition, Quaternion.identity);
    }
}
