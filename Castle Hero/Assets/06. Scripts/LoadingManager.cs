using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.SceneManagement;

public class LoadingManager : MonoBehaviour
{
    public const int waitData = 7;

    public bool[] dataCheck;
    [SerializeField] bool loadFinished;

    NetWorkManager networkManager;

    void Awake()
    {
        networkManager = GameObject.FindGameObjectWithTag("NetworkManager").GetComponent<NetWorkManager>();
        DontDestroyOnLoad(transform.gameObject);
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

    public IEnumerator DataCheck(GameManager.Scene scene)
    {
        while (true)
        {
            yield return new WaitForSeconds(1f);

            for (int i = 0; i < waitData; i++)
            {
                if (dataCheck[i])
                {
                    loadFinished = true;
                }
                else
                {
                    loadFinished = false;
                    yield return null;
                }
            }

            if (loadFinished)
            {
                StopAllCoroutines();
                StartCoroutine(LoadScene(scene, 1.0f));
            }
        }
    }

    public void LoadWaitScene()
    {
        StartCoroutine(loadHeroData());
        StartCoroutine(loadSkillData());
        StartCoroutine(loadItemData());
        StartCoroutine(loadUnitData());
        StartCoroutine(loadBuildingData());
        StartCoroutine(loadUpgradeData());
        StartCoroutine(loadResourceData());
        StartCoroutine(DataCheck(GameManager.Scene.Wait));
    }

    public IEnumerator loadHeroData()
    {
        while (!dataCheck[(int) ServerPacketId.HeroData - 4])
        {
            networkManager.DataRequest(ClientPacketId.HeroDataRequest);
            yield return new WaitForSeconds(1f);
        }
    }

    public IEnumerator loadSkillData()
    {
        while (!dataCheck[(int)ServerPacketId.SkillData - 4])
        {
            networkManager.DataRequest(ClientPacketId.SkillDataRequest);
            yield return new WaitForSeconds(1f);
        }
    }

    public IEnumerator loadItemData()
    {
        while (!dataCheck[(int)ServerPacketId.ItemData - 4])
        {
            networkManager.DataRequest(ClientPacketId.ItemDataRequest);
            yield return new WaitForSeconds(1f);
        }
    }

    public IEnumerator loadUnitData()
    {
        while (!dataCheck[(int)ServerPacketId.UnitData - 4])
        {
            networkManager.DataRequest(ClientPacketId.UnitDataRequest);
            yield return new WaitForSeconds(1f);
        }
    }

    public IEnumerator loadBuildingData()
    {
        while (!dataCheck[(int)ServerPacketId.BuildingData - 4])
        {
            networkManager.DataRequest(ClientPacketId.BuildingDataRequest);
            yield return new WaitForSeconds(1f);
        }
    }

    public IEnumerator loadUpgradeData()
    {
        while (!dataCheck[(int)ServerPacketId.UpgradeData - 4])
        {
            networkManager.DataRequest(ClientPacketId.UpgradeDataRequest);
            yield return new WaitForSeconds(1f);
        }
    }

    public IEnumerator loadResourceData()
    {
        while (!dataCheck[(int)ServerPacketId.ResourceData - 4])
        {
            networkManager.DataRequest(ClientPacketId.ResourceDataRequest);
            yield return new WaitForSeconds(1f);
        }
    }

    public IEnumerator LoadScene(GameManager.Scene scene, float delayTime)
    {
        yield return new WaitForSeconds(delayTime);
        SceneManager.LoadScene((int) scene);
        LoadWaitScene();
    }
}