using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;
using Photon.Pun;
using Photon.Realtime;
using UnityEngine.SceneManagement;

[System.Serializable]
public class SpawnPoint
{
    public int code;
    public List<GameObject> spawnPoints;

    public SpawnPoint(int code, List<GameObject> spawnPoints)
    {
        this.code = code;
        this.spawnPoints = spawnPoints;
    }
}

public class Spawner : MonoBehaviourPunCallbacks
{
    [SerializeField] GameObject playerPrefab;
    [SerializeField] Transform spawnParent;

    [SerializeField] List<Transform> spawnParents = new List<Transform>();
    List<SpawnPoint> spawnPointsList = new List<SpawnPoint>();

    GameObject nowMarbleOb;
    int nowWayPoint;

    private void Awake()
    {
        for (int i = 0; i < spawnParents.Count; i++)
        {
            List<GameObject> nowSpawnPoints = new List<GameObject>();

            foreach (Transform child in spawnParents[i])
            {
                nowSpawnPoints.Add(child.gameObject);
            }

            spawnPointsList.Add(new SpawnPoint(i, nowSpawnPoints));
        }
    }

    void Start()
    {
        if (!PhotonNetwork.IsConnected)
        {
            SceneManager.LoadScene("Title");
            return;
        }

        foreach (SpawnPoint spawnPoint in spawnPointsList)
        {
            foreach (GameObject point in spawnPoint.spawnPoints)
            {
                point.SetActive(false);
            }
        }

        Transform spawnTransform = GetSpawnTransform(PhotonNetwork.LocalPlayer);
        nowMarbleOb = PhotonNetwork.Instantiate(playerPrefab.name, spawnTransform.position, spawnTransform.rotation);

        nowWayPoint = 0;
    }


    Transform GetSpawnTransform(Player player)
    {
        Debug.Log("player.ActorNumber = " + player.ActorNumber);

        if (player.ActorNumber < 0 || player.ActorNumber > spawnPointsList[0].spawnPoints.Count)
        {
            return spawnPointsList[0].spawnPoints[0].transform;
        }

        return spawnPointsList[0].spawnPoints[player.ActorNumber].transform;
    }

    public Transform GetRespawnTransform()
    {
        return spawnPointsList[nowWayPoint].spawnPoints[Random.Range(0, spawnPointsList[nowWayPoint].spawnPoints.Count)].transform; 
    }

    public void SetWayPoint(int value)
    {
        if (nowWayPoint < value)
        {
            nowWayPoint = value;
        }
    }
}
