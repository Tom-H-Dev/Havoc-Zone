using System;
using System.Collections;
using System.Collections.Generic;
using Unity.VisualScripting;
using UnityEngine;

public class TeamManager : MonoBehaviour
{
    [Header("Player Teams")]
    [SerializeField] private List<GameObject> _playersTeamBlue;
    [SerializeField] private List<GameObject> _playersTeamRed;

    [Header("Spawn Areas")]
    [SerializeField] private List<GameObject> _spawnPointsTeamRed = new List<GameObject>();
    [SerializeField] private List<GameObject> _spawnPointsTeamBlue = new List<GameObject>();


    private void Start()
    {
        _playersTeamBlue = new List<GameObject>();
        _playersTeamRed = new List<GameObject>();

        System.Random random = new System.Random();
        int n = GameManager.instance.players.Count;
        while (n > 1)
        {
            n--;
            int k = random.Next(n + 1);
            GameObject temp = GameManager.instance.players[k];
            GameManager.instance.players[k] = GameManager.instance.players[n];
            GameManager.instance.players[n] = temp;
        }
        int countPerList = GameManager.instance.players.Count / 2;

        for (int i = 0; i < GameManager.instance.players.Count; i++)
        {
            if (i < countPerList)
            {
                _playersTeamBlue.Add(GameManager.instance.players[i]);
            }
            else
            {
                _playersTeamRed.Add(GameManager.instance.players[i]);
            }
        }

        SpawnPlayersInPositions();
    }

    public void SpawnPlayersInPositions()
    {
        int l_latestIndexPointBlue = 0;
        int l_latestIndexPointRed = 0;
        foreach (GameObject l_playerBlue in _playersTeamBlue)
        {
            l_playerBlue.transform.position = _spawnPointsTeamBlue[l_latestIndexPointBlue].transform.position;
            l_playerBlue.GetComponent<PlayerHealth>().localSpawnPosition = _spawnPointsTeamBlue[l_latestIndexPointBlue].transform;
            l_latestIndexPointBlue++;
        }
        foreach (GameObject l_playerRed in _playersTeamRed)
        {
            l_playerRed.transform.position = _spawnPointsTeamRed[l_latestIndexPointRed].transform.position;
            l_playerRed.GetComponent<PlayerHealth>().localSpawnPosition = _spawnPointsTeamRed[l_latestIndexPointRed].transform;
            l_latestIndexPointRed++;
        }
    }
}
