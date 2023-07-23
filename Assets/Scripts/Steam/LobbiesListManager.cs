using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using Steamworks;

public class LobbiesListManager : MonoBehaviour
{
    public static LobbiesListManager instance;

    public GameObject lobbiesMenu;
    public GameObject lobbiesDataItemPrefab;
    public GameObject lobbiesListContent;

    public GameObject lobbiesButton, hostButton;

    public List<GameObject> listOfLobbies = new List<GameObject>();

    private void Awake()
    {
        if (instance != null)
            Destroy(instance);
        else instance = this;
    }

    public void DestroyLobbies()
    {
        foreach (GameObject l_lobbies in listOfLobbies)
        {
            Destroy(l_lobbies);
        }
        listOfLobbies.Clear();
    }

    public void DisplayLobbies(List<CSteamID> l_lobbyIDs, LobbyDataUpdate_t l_result)
    {
        for (int i = 0; i < l_lobbyIDs.Count; i++)
        {
            if (l_lobbyIDs[i].m_SteamID == l_result.m_ulSteamIDLobby)
            {
                GameObject l_createdItem = Instantiate(lobbiesDataItemPrefab);

                l_createdItem.GetComponent<LobbyDataEntery>().lobbyID = (CSteamID)l_lobbyIDs[i].m_SteamID;
                l_createdItem.GetComponent<LobbyDataEntery>().lobbyName = SteamMatchmaking.GetLobbyData((CSteamID)l_lobbyIDs[i].m_SteamID, "name");

                l_createdItem.GetComponent<LobbyDataEntery>().SetLobbyData();

                l_createdItem.transform.SetParent(lobbiesListContent.transform);
                l_createdItem.transform.localScale = Vector3.one;

                listOfLobbies.Add(l_createdItem);
            }
        }
    }

    public void GetListOfLobbies()
    {
        lobbiesButton.SetActive(false);
        hostButton.SetActive(false);

        lobbiesMenu.SetActive(true);

        SteamLobby.instance.GetLobbiesList();
    }
}
