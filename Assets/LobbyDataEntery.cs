using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using Steamworks;
using TMPro;
using UnityEngine.UI;

public class LobbyDataEntery : MonoBehaviour
{
    public CSteamID lobbyID;
    public string lobbyName;
    public TextMeshProUGUI lobbyNameText;

    public void SetLobbyData()
    {
        if (lobbyName == "")
        {
            lobbyNameText.text = "Empty";
        }
        else
        {
            lobbyNameText.text = lobbyName;
        }

    }

    public void JoinLobby()
    {
        SteamLobby.instance.JoinLobby(lobbyID);
    }
}
