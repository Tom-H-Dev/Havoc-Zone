using System.Collections;
using System.Collections.Generic;
using TMPro;
using UnityEngine;

public class NotDestoryBedug : MonoBehaviour
{
    public static NotDestoryBedug instance;

    public TextMeshProUGUI debugText;
    private void Awake()
    {
        if (instance != null)
            Destroy(instance);
        else instance = this;
        DontDestroyOnLoad(gameObject);
    }
}
