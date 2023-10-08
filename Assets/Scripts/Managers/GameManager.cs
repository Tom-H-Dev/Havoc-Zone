using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.SceneManagement;

public class GameManager : MonoBehaviour
{
    public static GameManager instance;
    public List<GameObject> players;

    public bool devAccess;

    private void Awake()
    {
        if (instance != null)
            Destroy(instance);
        else instance = this;

        DontDestroyOnLoad(gameObject);
        players = new List<GameObject>();
    }
}
