using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using Mirror;
using Steamworks;
using TMPro;
using System.Linq;
using UnityEngine.UI;

public class LobbyController : MonoBehaviour
{
    public static LobbyController instance;

    // UI
    public TextMeshProUGUI lobbyNameText;

    // Player Data
    public GameObject playerListViewContentBlue;
    public GameObject playerListItemPrefab;
    public GameObject localPlayerObject;

    // Other data
    public ulong currentLobbyID;
    public bool playerItemCreated = false;
    private List<PlayerListItem> _playerListItems = new List<PlayerListItem>();
    public PlayerObjectController localPlayerController;

    //Ready
    public Button startGameButton;
    public TextMeshProUGUI readyButtonText;

    // Manager
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
    private void Awake()
    {
        if (instance != null)
            Destroy(instance);
        else instance = this;
    }

    public void ReadyPlayer()
    {
        localPlayerController.ChangeReady();
    }

    public void UpdateButton()
    {
        if (localPlayerController.ready)
        {
            readyButtonText.text = "Unready";
        }
        else
        {
            readyButtonText.text = "Ready";
        }
    }

    public void CheckIfAllReady()
    {
        bool l_allReady = false;

        foreach (PlayerObjectController l_player in Manager.gamePlayers)
        {
            if (l_player.ready)
            {
                l_allReady = true;
            }
            else
            {
                l_allReady = false;
                break;
            }
        }

        if (l_allReady)
        {
            if (localPlayerController.playerIDNumber == 1)
            {
                startGameButton.interactable = true;
            }
            else
            {
                startGameButton.interactable = false;
            }
        }
        else
        {
            startGameButton.interactable = false;
        }
    }

    public void UpdateLobbyName()
    {
        currentLobbyID = Manager.GetComponent<SteamLobby>().currentLobbyID;
        lobbyNameText.text = SteamMatchmaking.GetLobbyData(new CSteamID(currentLobbyID), "name");
    }

    public void UpdatePlayerList()
    {
        if (!playerItemCreated) // Host
            CreateHostPlayerItem();
        if (_playerListItems.Count < Manager.gamePlayers.Count) // Client
            CreateClientPlayerItem();
        if (_playerListItems.Count > Manager.gamePlayers.Count)
            RemovePlayerItem();
        if (_playerListItems.Count == Manager.gamePlayers.Count)
            UpdatePlayerItem();
    }

    public void FindLocalPlayer()
    {
        localPlayerObject = GameObject.Find("LocalGamePlayer");
        localPlayerController = localPlayerObject.GetComponent<PlayerObjectController>();
    }

    public void CreateHostPlayerItem()
    {
        foreach (PlayerObjectController l_player in Manager.gamePlayers)
        {
            //NotDestoryBedug.instance.debugText.text = "Host";
            GameObject l_newPlayerItem = Instantiate(playerListItemPrefab) as GameObject;
            PlayerListItem l_newPlayerItemSrcipt = l_newPlayerItem.GetComponent<PlayerListItem>();

            l_newPlayerItemSrcipt.playerName = l_player.playerName;
            l_newPlayerItemSrcipt.connectionID = l_player.connectionID;
            l_newPlayerItemSrcipt.playerSteamID = l_player.playerSteamID;
            l_newPlayerItemSrcipt.ready = l_player.ready;
            l_newPlayerItemSrcipt.SetPlayerValues();

            l_newPlayerItemSrcipt.transform.SetParent(playerListViewContentBlue.transform);
            l_newPlayerItemSrcipt.transform.localScale = Vector3.one;
        }
        playerItemCreated = true;
    }

    public void CreateClientPlayerItem()
    {
        foreach (PlayerObjectController l_player in Manager.gamePlayers)
        {
            if (!_playerListItems.Any(b => b.connectionID == l_player.connectionID))
            {
                GameObject l_newPlayerItem = Instantiate(playerListItemPrefab) as GameObject;
                PlayerListItem l_newPlayerItemSript = l_newPlayerItem.GetComponent<PlayerListItem>();

                l_newPlayerItemSript.playerName = l_player.playerName;
                l_newPlayerItemSript.connectionID = l_player.connectionID;
                l_newPlayerItemSript.playerSteamID = l_player.playerSteamID;
                l_newPlayerItemSript.ready = l_player.ready;
                l_newPlayerItemSript.SetPlayerValues();

                l_newPlayerItemSript.transform.SetParent(playerListViewContentBlue.transform);
                l_newPlayerItemSript.transform.localScale = Vector3.one;

                _playerListItems.Add(l_newPlayerItemSript);
            }
        }
    }

    public void UpdatePlayerItem()
    {
        foreach (PlayerObjectController l_player in Manager.gamePlayers)
        {
            foreach (PlayerListItem l_playerListItemScript in _playerListItems)
            {
                if (l_playerListItemScript.connectionID == l_player.connectionID)
                {
                    l_playerListItemScript.playerName = l_player.playerName;
                    l_playerListItemScript.ready = l_player.ready;
                    l_playerListItemScript.SetPlayerValues();
                    if (l_player == localPlayerController)
                    {
                        UpdateButton();
                    }
                }
            }
        }
        CheckIfAllReady();
    }

    public void RemovePlayerItem()
    {
        List<PlayerListItem> l_playerListItemToRemove = new List<PlayerListItem>();

        foreach (PlayerListItem l_playerListItem in _playerListItems)
        {
            if (!Manager.gamePlayers.Any(b => b.connectionID == l_playerListItem.connectionID))
            {
                l_playerListItemToRemove.Add(l_playerListItem);
            }
        }
        if (l_playerListItemToRemove.Count > 0)
        {
            foreach (PlayerListItem l_playerListItemToRemoveLoop in l_playerListItemToRemove)
            {
                GameObject l_objectToRemove = l_playerListItemToRemoveLoop.gameObject;
                _playerListItems.Remove(l_playerListItemToRemoveLoop);
                Destroy(l_objectToRemove);
                l_objectToRemove = null;

            }
        }
    }

    public void StartGame(string l_sceneName)
    {
        localPlayerController.CanStartGame(l_sceneName);
    }
}