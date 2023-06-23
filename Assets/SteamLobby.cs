using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using Mirror;
using Steamworks;
using TMPro;

public class SteamLobby : MonoBehaviour
{
    public static SteamLobby instance;

    //Callbacks
    protected Callback<LobbyCreated_t> p_lobbyCreated;
    protected Callback<GameLobbyJoinRequested_t> p_joinRequest;
    protected Callback<LobbyEnter_t> p_lobbyEnter;

    //Lobby Callbacks
    protected Callback<LobbyMatchList_t> p_lobbyList;
    protected Callback<LobbyDataUpdate_t> p_lobbyDataUpdate;

    public List<CSteamID> lobbyIDs = new List<CSteamID>();

    //Variables
    public ulong currentLobbyID;
    private const string _hostAdressKey = "HostAddress";
    private CustomNetworkManager _networkManager;


    private void Start()
    {
        if (!SteamManager.Initialized)
            return;
        if (instance != null)
            Destroy(instance);
        else instance = this;

        _networkManager = GetComponent<CustomNetworkManager>();

        p_lobbyCreated = Callback<LobbyCreated_t>.Create(OnLobbyCreated);
        p_joinRequest = Callback<GameLobbyJoinRequested_t>.Create(OnJoinRequest);
        p_lobbyEnter = Callback<LobbyEnter_t>.Create(OnLobbyEntered);

        p_lobbyList = Callback<LobbyMatchList_t>.Create(OnGetLobbyList);
        p_lobbyDataUpdate = Callback<LobbyDataUpdate_t>.Create(OnGetLobbyData);
    }

    public void HostLobby()
    {
        SteamMatchmaking.CreateLobby(ELobbyType.k_ELobbyTypePublic, _networkManager.maxConnections);
    }


    private void OnLobbyCreated(LobbyCreated_t l_callback)
    {
        if (l_callback.m_eResult != EResult.k_EResultOK)
            return;

        Debug.Log("Lobby created successfully");
        _networkManager.StartHost();

        SteamMatchmaking.SetLobbyData(new CSteamID(l_callback.m_ulSteamIDLobby), _hostAdressKey, SteamUser.GetSteamID().ToString());
        SteamMatchmaking.SetLobbyData(new CSteamID(l_callback.m_ulSteamIDLobby), "name", SteamFriends.GetPersonaName().ToString() + "'s LOBBY!");
    }

    private void OnJoinRequest(GameLobbyJoinRequested_t l_callback)
    {
        Debug.Log("Request To Join Lobby");
        SteamMatchmaking.JoinLobby(l_callback.m_steamIDLobby);
    }

    /// <summary>
    /// This gets called every time someone joins the lobby this includes the host
    /// </summary>
    /// <param name="l_callback"></param>
    private void OnLobbyEntered(LobbyEnter_t l_callback)
    {
        //Everyone
        Debug.Log("Entered lobby");
        currentLobbyID = l_callback.m_ulSteamIDLobby;

        //Clients
        if (NetworkServer.active)
            return;

        _networkManager.networkAddress = SteamMatchmaking.GetLobbyData(new CSteamID(l_callback.m_ulSteamIDLobby), _hostAdressKey);

        _networkManager.StartClient();
    }

    public void JoinLobby(CSteamID l_lobbyID)
    {
        SteamMatchmaking.JoinLobby(l_lobbyID);
    }

    public void GetLobbiesList()
    {
        if (lobbyIDs.Count > 0)
            lobbyIDs.Clear();

        SteamMatchmaking.AddRequestLobbyListResultCountFilter(60);
        SteamMatchmaking.RequestLobbyList();
    }

    public void OnGetLobbyList(LobbyMatchList_t l_callback)
    {
        if (LobbiesListManager.instance.listOfLobbies.Count > 0)
            LobbiesListManager.instance.DestroyLobbies();

        for (int i = 0; i < l_callback.m_nLobbiesMatching; i++)
        {
            CSteamID l_lobbyID = SteamMatchmaking.GetLobbyByIndex(i);
            lobbyIDs.Add(l_lobbyID);
            SteamMatchmaking.RequestLobbyData(l_lobbyID);
        }
    }

    public void OnGetLobbyData(LobbyDataUpdate_t l_callback)
    {
        LobbiesListManager.instance.DisplayLobbies(lobbyIDs, l_callback);
    }
}
