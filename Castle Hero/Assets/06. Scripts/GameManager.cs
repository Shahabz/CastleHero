using UnityEngine;
using System.Collections;

public class GameManager : MonoBehaviour
{
    public enum Scene
    {
        Login = 0,
        Loading,
        Wait,
        Battle,
    }

    NetWorkManager networkManager;
    UIManager uiManager;
    DataManager dataManager;

    void Awake()
    {
        networkManager = GameObject.FindGameObjectWithTag("NetworkManager").GetComponent<NetWorkManager>();
        uiManager = GameObject.FindGameObjectWithTag("UIManager").GetComponent<UIManager>();
        DontDestroyOnLoad(transform.gameObject);
    }

    void OnApplicationQuit()
    {
        networkManager.GameExit();
    }
}