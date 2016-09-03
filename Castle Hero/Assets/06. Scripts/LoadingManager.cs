using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.SceneManagement;

public class LoadingManager : MonoBehaviour
{
    public enum Scene
    {
        Wait,
        Battle,
    }

    public const int waitData = 7;

    bool[] dataCheck;
    bool loadFinished;

    NetWorkManager networkManager;

    public delegate void RecvNotifier();
    private Dictionary<int, RecvNotifier> m_notifier = new Dictionary<int, RecvNotifier>();

    void Awake()
    {
        networkManager = GameObject.FindGameObjectWithTag("NetWorkManager").GetComponent<NetWorkManager>();
        DontDestroyOnLoad(transform.gameObject);

        m_notifier.Add((int)Scene.Wait, LoadWaitScene);
    }

    void Start()
    {
        dataCheck = new bool[waitData];
        InitializeDataCheck();
        loadFinished = false;
    }

    public void InitializeDataCheck()
    {
        for(int i =0; i< waitData; i++)
        {
            dataCheck[i] = false;
        }
    }

    public IEnumerator DataCheck()
    {
        yield return null;
        for (int i = 0; i < waitData; i++)
        {
            if(dataCheck[i] != false)
            {

            }
        }

        loadFinished = true;
    }

    public void LoadWaitScene()
    {
        networkManager.DataRequest(ClientPacketId.HeroDataRequest);
        networkManager.DataRequest(ClientPacketId.SkillDataRequest);
        networkManager.DataRequest(ClientPacketId.ItemDataRequest);
        networkManager.DataRequest(ClientPacketId.UnitDataRequest);
        networkManager.DataRequest(ClientPacketId.ResourceDataRequest);
        networkManager.DataRequest(ClientPacketId.BuildingDataRequest);
        networkManager.DataRequest(ClientPacketId.UpgradeDataRequest);
    }

    public IEnumerator LoadScene(int index, float delayTime, Scene scene)
    {
        yield return new WaitForSeconds(delayTime);
        SceneManager.LoadScene(index);
        m_notifier[(int)Scene.Wait]();
    }
}