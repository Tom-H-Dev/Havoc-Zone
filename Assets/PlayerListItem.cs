using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using TMPro;
using Steamworks;
using UnityEngine.UI;
using TMPro.Examples;

public class PlayerListItem : MonoBehaviour
{
    public string playerName;
    public int connectionID;
    public ulong playerSteamID;
    private bool _avatarRecieved;

    public TextMeshProUGUI playerNameText;
    public RawImage playerIcon;

    public TextMeshProUGUI playerReadyText;
    public bool ready;

    protected Callback<AvatarImageLoaded_t> p_imageLoaded;


    private void Start()
    {
        p_imageLoaded = Callback<AvatarImageLoaded_t>.Create(OnImageLoaded);
    }

    public void ChangeReadyStatus()
    {
        if (ready) // If ready 
        {
            playerReadyText.text = "Ready";
            playerReadyText.color = Color.green;
        }
        else // If not ready
        {
            playerReadyText.text = "Unready";
            playerReadyText.color = Color.red;
        }
    }

    private void GetPlayericon()
    {
        int l_imageID = SteamFriends.GetLargeFriendAvatar((CSteamID)playerSteamID);
        if (l_imageID == -1)
            return;
        playerIcon.texture = GetSteamImageAsTexture(l_imageID);
    }

    public void SetPlayerValues()
    {
        playerNameText.text = playerName;
        ChangeReadyStatus();
        if (!_avatarRecieved)
            GetPlayericon();
    }

    private Texture2D GetSteamImageAsTexture(int l_iImage)
    {
        Texture2D l_texture = null;

        bool l_isVallid = SteamUtils.GetImageSize(l_iImage, out uint width, out uint height);
        if (l_isVallid)
        {
            byte[] l_image = new byte[width * height * 4];
            l_isVallid = SteamUtils.GetImageRGBA(l_iImage, l_image, (int)(width * height * 4));

            if (l_isVallid)
            {
                l_texture = new Texture2D((int)width, (int)height, TextureFormat.RGBA32, false, true);
                l_texture.LoadRawTextureData(l_image);
                l_texture.Apply();
            }
        }
        _avatarRecieved = true;
        return l_texture;
    }

    public void OnImageLoaded(AvatarImageLoaded_t l_callback)
    {
        if (l_callback.m_steamID.m_SteamID == playerSteamID) // Us
        {
            playerIcon.texture = GetSteamImageAsTexture(l_callback.m_iImage);
        }
        else // Another player
        {
            return;
        }
    }
}
