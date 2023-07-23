using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using Mirror;
using UnityEngine.SceneManagement;
using Steamworks;

public class CustomNetworkManager : NetworkManager
{
    [SerializeField] private PlayerObjectController _gamePlayerPrefab;
    public List<PlayerObjectController> gamePlayers { get; } = new List<PlayerObjectController>();

    public override void OnServerAddPlayer(NetworkConnectionToClient conn)
    {
        if (SceneManager.GetActiveScene().name == "Lobby")
        {
            PlayerObjectController l_gamePlayerInstance = Instantiate(_gamePlayerPrefab);
            l_gamePlayerInstance.connectionID = conn.connectionId;
            l_gamePlayerInstance.playerIDNumber = gamePlayers.Count + 1;
            l_gamePlayerInstance.playerSteamID = (ulong)SteamMatchmaking.GetLobbyMemberByIndex((CSteamID)SteamLobby.instance.currentLobbyID, gamePlayers.Count);

            NetworkServer.AddPlayerForConnection(conn, l_gamePlayerInstance.gameObject);
        }
    }

    public void StartGame(string l_sceneName)
    {
        ServerChangeScene(l_sceneName);
    }
}
