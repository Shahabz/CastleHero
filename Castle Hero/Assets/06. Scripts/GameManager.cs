using UnityEngine;

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

    void Awake()
    {
        networkManager = GameObject.FindGameObjectWithTag("NetworkManager").GetComponent<NetWorkManager>();
        uiManager = GameObject.FindGameObjectWithTag("UIManager").GetComponent<UIManager>();
        DontDestroyOnLoad(transform.gameObject);
    }

    void FixedUpdate()
    {
        networkManager.DataHandle();
    }

    void LateUpdate()
    {
        networkManager.DataSend();
    }

    void OnApplicationQuit()
    {
        networkManager.GameExit();
    }
}