using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class TeamManager : MonoBehaviour
{
    [SerializeField] private List<GameObject> _playersTeamBlue;
    [SerializeField] private List<GameObject> _playersTeamRed;


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
    }
}
