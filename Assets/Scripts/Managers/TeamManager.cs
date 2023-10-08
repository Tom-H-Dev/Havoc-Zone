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
    }
}
