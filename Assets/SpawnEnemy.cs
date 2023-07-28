using Photon.Pun;
using UnityEngine;

public class SpawnEnemy : MonoBehaviour
{
    private float spawnEdgesX = 12;
    private float spawnY = 5;

    [SerializeField] private GameObject enemy;
    private float startTimeBetweenSpawns = 5;
    [SerializeField] private float timeBetweenSpawns;


    private void Update()
    {
        if (Input.GetKeyDown(KeyCode.Q) && PhotonNetwork.IsMasterClient) 
        {
            TestFunction();
        }
        //if (PhotonNetwork.IsMasterClient == false || PhotonNetwork.CurrentRoom.PlayerCount != 2)
        //{
        //    return;
        //}

        //if (timeBetweenSpawns <= 0)
        //{
        //    float spawnCoordinateX = Random.Range(-spawnEdgesX, spawnEdgesX);
        //    Vector3 spawnPos = new Vector3(spawnCoordinateX, spawnY, 40);
        //    PhotonNetwork.Instantiate(enemy.name, spawnPos, Quaternion.identity);
        //    timeBetweenSpawns = startTimeBetweenSpawns;
        //}
        //else
        //{
        //    timeBetweenSpawns -= Time.deltaTime;
        //}
    }

    private void TestFunction()
    {
        //float spawnCoordinateX = Random.Range(-spawnEdgesX, spawnEdgesX);
        PhotonNetwork.Instantiate(enemy.name, new Vector2(-8, 0), Quaternion.identity);
    }
}
