using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using Mirror;
using Steamworks;

public class PlayerObjectController : NetworkBehaviour
{
    //Player Data
    [SyncVar] public int connectionID;
    [SyncVar] public int playerIDNumber;
    [SyncVar] public ulong playerSteamID;
    [SyncVar(hook = nameof(PlayerNameUpdate))] public string playerName;
    [SyncVar(hook = nameof(ReadyPlayerUpdate))] public bool ready;

    private CustomNetworkManager _manager;

    private CustomNetworkManager Manager
    {
        get
        {
            if (_manager != null)
                return _manager;

            return _manager = CustomNetworkManager.singleton as CustomNetworkManager;
        }
    }

    private void Start()
    {
        DontDestroyOnLoad(this.gameObject);
        GameManager.instance.players.Add(gameObject);
    }

    private void ReadyPlayerUpdate(bool l_oldValue, bool l_newValue)
    {
        if (isServer)
        {
            this.ready = l_newValue;
        }
        if (isClient)
        {
            LobbyController.instance.UpdatePlayerList();
        }
    }

    [Command]
    private void CmdSetPlayerReady()
    {
        this.ReadyPlayerUpdate(this.ready, !this.ready);
    }

    public void ChangeReady()
    {
        if (hasAuthority)
            CmdSetPlayerReady();
    }

    public override void OnStartAuthority()
    {
        CmdSetPlayerName(SteamFriends.GetPersonaName().ToString());
        gameObject.name = "LocalGamePlayer";
        LobbyController.instance.FindLocalPlayer();
        LobbyController.instance.UpdateLobbyName();
    }

    public override void OnStartClient()
    {
        Manager.gamePlayers.Add(this);
        LobbyController.instance.UpdateLobbyName();
        LobbyController.instance.UpdatePlayerList();
    }

    public override void OnStopClient()
    {
        Manager.gamePlayers.Remove(this);
        LobbyController.instance.UpdatePlayerList();
    }

    [Command]
    private void CmdSetPlayerName(string l_playerName)
    {
        this.PlayerNameUpdate(this.playerName, l_playerName);
    }

    public void PlayerNameUpdate(string l_oldValue, string l_newValue)
    {
        if (isServer) // Host
            this.playerName = l_newValue;
        if (isClient)
            LobbyController.instance.UpdatePlayerList();
    }

    // Start Game
    public void CanStartGame(string l_sceneName)
    {
        if (hasAuthority)
        {
            CmdCanStartGame(l_sceneName);
        }
    }

    [Command]
    public void CmdCanStartGame(string l_sceneName)
    {
        _manager.StartGame(l_sceneName);
    }
}
