using UnityEngine;
using UnityEngine.UI;
using System;
using System.Collections;

public class UIManager : MonoBehaviour
{
    [SerializeField] NetworkManager networkManager;
    [SerializeField] DataManager dataManager;

    UnitUIManager unitUIManager;
    public UnitUIManager UnitUIManager { get { return unitUIManager; } }
    BuildingUIManager buildingUIManager;
    public BuildingUIManager BuildingUIManager { get { return buildingUIManager; } }

    //로그인씬 패널
    public GameObject loginPanel;
    public GameObject createAccountPanel;
    public GameObject deleteAccountPanel;
    public GameObject lastChoicePanel;
    public static GameObject dialog;

    //대기씬 패널
    public GameObject unitStateScroll;
    public GameObject informationPanel;
    public GameObject currentPanel;
    public GameObject statusPanel;
    public GameObject itemPanel;
    public GameObject equipmentPanel;
    public GameObject inventoryPanel;
    public GameObject unitPanel;
    public GameObject buildingPanel;

    //로그인씬 버튼
    public Button loginButton;
    public Button createButton;
    public Button deleteButton;
    public Button exitButton;
    public Button loginAccountButton;
    public Button loginCancelButton;
    public Button createAccountButton;
    public Button createCancelButton;
    public Button deleteAccountButton;
    public Button deleteCancelButton;
    public Button deleteYesButton;
    public Button deleteNoButton;

    //대기씬 버튼
    public Button logoutButton;
    public Button statusButton;
    public Button equipmentButton;
    public Button skillButton;
    public Button unitButton;
    public Button createUnitButton;
    public Button buildingButton;
    public Button upgradeButton;
    public Button quitButton;

    //로그인씬 텍스트
    public Text loginId;
    public Text loginPw;
    public Text createId;
    public Text createPw;
    public Text deleteId;
    public Text deletePw;

    //대기씬 텍스트
    public Text castleState;
    public Text heroState;
    public Text resource;
    public Text unitCreateName;
    public Text unitCreateTime;
    public Text buildName;
    public Text buildTime;
    public Text level;
    public Text experience;
    public Text health;
    public Text mana;
    public Text attack;
    public Text defense;
    public Text magicDefense;
    public Text moveSpeed;
    public Text attackSpeed;

    //대기씬 이미지
    public GameObject[] equipment;
    public GameObject[] inventory;

    public BuildingId currentBuilding;

    void Awake()
    {
        tag = "UIManager";
        DontDestroyOnLoad(transform.gameObject);
    }

    void Start()
    {
        SetLoginUIObject();
        LoginSceneAddListener();
        currentBuilding = BuildingId.None;
    }

    //매니저 초기화
    public void ManagerInitialize()
    {
        networkManager = GameObject.FindGameObjectWithTag("NetworkManager").GetComponent<NetworkManager>();
        dataManager = GameObject.FindGameObjectWithTag("DataManager").GetComponent<DataManager>();

        unitUIManager = new UnitUIManager();
        unitUIManager.ManagerInitialize();
        buildingUIManager = new BuildingUIManager();
        buildingUIManager.ManagerInitialize();
    }

    //UI매니저 초기화
    public void SetUIManager()
    {
        unitUIManager.SetUIObject();        
        buildingUIManager.SetUIObject();
    }

    //로그인씬 패널, 버튼, 텍스트 설정
    void SetLoginUIObject()
    {
        loginPanel = GameObject.Find("LoginPanel");
        createAccountPanel = GameObject.Find("CreateAccountPanel");
        deleteAccountPanel = GameObject.Find("DeleteAccountPanel");
        lastChoicePanel = GameObject.Find("LastChoicePanel");
        dialog = GameObject.Find("Dialog");

        loginButton = GameObject.Find("LoginButton").GetComponent<Button>();
        createButton = GameObject.Find("CreateButton").GetComponent<Button>();
        deleteButton = GameObject.Find("DeleteButton").GetComponent<Button>();
        exitButton = GameObject.Find("ExitButton").GetComponent<Button>();
        loginAccountButton = GameObject.Find("LoginAccountButton").GetComponent<Button>();
        loginCancelButton = GameObject.Find("LoginCancelButton").GetComponent<Button>();
        createAccountButton = GameObject.Find("CreateAccountButton").GetComponent<Button>();
        createCancelButton = GameObject.Find("CreateCancelButton").GetComponent<Button>();
        deleteAccountButton = GameObject.Find("DeleteAccountButton").GetComponent<Button>();
        deleteCancelButton = GameObject.Find("DeleteCancelButton").GetComponent<Button>();
        deleteYesButton = GameObject.Find("DeleteYesButton").GetComponent<Button>();
        deleteNoButton = GameObject.Find("DeleteNoButton").GetComponent<Button>();

        loginId = GameObject.Find("LoginId").GetComponent<Text>();
        loginPw = GameObject.Find("LoginPw").GetComponent<Text>();
        createId = GameObject.Find("CreateId").GetComponent<Text>();
        createPw = GameObject.Find("CreatePw").GetComponent<Text>();
        deleteId = GameObject.Find("DeleteId").GetComponent<Text>();
        deletePw = GameObject.Find("DeletePw").GetComponent<Text>();

        loginPanel.SetActive(false);
        createAccountPanel.SetActive(false);
        deleteAccountPanel.SetActive(false);
        lastChoicePanel.SetActive(false);
        dialog.SetActive(false);
    }

    //대기씬 패널, 버튼, 텍스트 설정
    public void SetWaitUIObject()
    {
        informationPanel = GameObject.Find("InformationPanel");
        statusPanel = GameObject.Find("StatusPanel");
        itemPanel = GameObject.Find("ItemPanel");
        equipmentPanel = GameObject.Find("EquipmentPanel");
        inventoryPanel = GameObject.Find("InventoryPanel");
        unitPanel = GameObject.Find("UnitPanel");
        buildingPanel = GameObject.Find("BuildingPanel");

        logoutButton = GameObject.Find("LogoutButton").GetComponent<Button>();
        statusButton = GameObject.Find("StatusButton").GetComponent<Button>();
        equipmentButton = GameObject.Find("EquipmentButton").GetComponent<Button>();
        skillButton = GameObject.Find("SkillButton").GetComponent<Button>();
        unitButton = GameObject.Find("UnitButton").GetComponent<Button>();
        buildingButton = GameObject.Find("BuildingButton").GetComponent<Button>();
        upgradeButton = GameObject.Find("UpgradeButton").GetComponent<Button>();
        quitButton = GameObject.Find("QuitButton").GetComponent<Button>();

        castleState = GameObject.Find("CastleState").GetComponent<Text>();
        heroState = GameObject.Find("HeroState").GetComponent<Text>();
        resource = GameObject.Find("Resource").GetComponent<Text>();
        unitCreateName = GameObject.Find("UnitCreateName").GetComponent<Text>();
        unitCreateTime = GameObject.Find("UnitCreateTime").GetComponent<Text>();
        buildName = GameObject.Find("BuildName").GetComponent<Text>();
        buildTime = GameObject.Find("BuildTime").GetComponent<Text>();
        level = GameObject.Find("Level").GetComponent<Text>();
        experience = GameObject.Find("Experience").GetComponent<Text>();
        health = GameObject.Find("Health").GetComponent<Text>();
        mana = GameObject.Find("Mana").GetComponent<Text>();
        attack = GameObject.Find("Attack").GetComponent<Text>();
        defense = GameObject.Find("Defense").GetComponent<Text>();
        magicDefense = GameObject.Find("MagicDefense").GetComponent<Text>();
        moveSpeed = GameObject.Find("MoveSpeed").GetComponent<Text>();
        attackSpeed = GameObject.Find("AttackSpeed").GetComponent<Text>();

        CreateEquipmentSlot();
        CreateInventorySlot();
        
        statusPanel.SetActive(false);
        itemPanel.SetActive(false);
        unitPanel.SetActive(false);
        buildingPanel.SetActive(false);
        informationPanel.SetActive(false);
    }

    //로그인씬 버튼 이벤트 설정
    public void LoginSceneAddListener()
    {
        loginButton.onClick.AddListener(() => OnClickLoginButton());
        createButton.onClick.AddListener(() => OnClickCreateButton());
        deleteButton.onClick.AddListener(() => OnClickDeleteButton());
        exitButton.onClick.AddListener(() => OnClickExitButton());
        loginAccountButton.onClick.AddListener(() => OnClickLoginAccountButton());
        loginCancelButton.onClick.AddListener(() => OnClickLoginCancelButton());
        createAccountButton.onClick.AddListener(() => OnClickCreateAccountButton());
        createCancelButton.onClick.AddListener(() => OnClickCreateCancelButton());
        deleteAccountButton.onClick.AddListener(() => OnClickDeleteAccountButton());
        deleteCancelButton.onClick.AddListener(() => OnClickDeleteCancelButton());
        deleteYesButton.onClick.AddListener(() => OnClickDeleteYesButton());
        deleteNoButton.onClick.AddListener(() => OnClickDeleteNoButton());
    }

    //대기 씬 버튼 이벤트 추가
    public void WaitSceneAddListener()
    {
        logoutButton.onClick.AddListener(() => OnClickLogoutButton());
        statusButton.onClick.AddListener(() => OnCLickStatusButton());        
        equipmentButton.onClick.AddListener(() => OnClickItemButton());
        unitButton.onClick.AddListener(() => OnClickUnitButton());
        buildingButton.onClick.AddListener(() => OnClickBuildingButton());
        quitButton.onClick.AddListener(() => OnClickQuitButton());
        buildingUIManager.OnClickAddListener();
    }

    //가입하기버튼
    public void OnClickCreateButton()
    {
        createAccountPanel.SetActive(true);
    }

    //아이디, 비밀번호 입력 후 가입하기
    public void OnClickCreateAccountButton()
    {
        if (createId.text.Length >= 4 && createPw.text.Length >= 6)
        {
            networkManager.CreateAccount(createId.text, createPw.text);
        }
        else
        {
            Debug.Log("아이디 4글자 이상. 비밀번호 6글자 이상");
        }
    }

    //가입하기 취소
    public void OnClickCreateCancelButton()
    {
        createAccountPanel.SetActive(false);
    }

    //탈퇴하기버튼
    public void OnClickDeleteButton()
    {
        deleteAccountPanel.SetActive(true);
    }

    //아이디, 비밀번호 입력 후 탈퇴하기
    public void OnClickDeleteAccountButton()
    {
        lastChoicePanel.SetActive(true);
    }

    //탈퇴하기 취소
    public void OnClickDeleteCancelButton()
    {
        deleteAccountPanel.SetActive(false);
    }

    //탈퇴 & 취소 선택시 탈퇴
    public void OnClickDeleteYesButton()
    {
        networkManager.DeleteAccount(deleteId.text, deletePw.text);
        lastChoicePanel.SetActive(false);
    }

    //탈퇴 & 취소 선택시 취소
    public void OnClickDeleteNoButton()
    {
        lastChoicePanel.SetActive(false);
    }

    //로그인 버튼
    public void OnClickLoginButton()
    {
        loginPanel.SetActive(true);
    }

    //로그인 취소
    public void OnClickLoginCancelButton()
    {
        loginPanel.SetActive(false);
    }

    //아이디, 비밀번호 입력 후 로그인하기
    public void OnClickLoginAccountButton()
    {
        if (loginId.text.Length >= 4 && loginPw.text.Length >= 6)
        {
            networkManager.Login(loginId.text, loginPw.text);
        }
        else
        {
            Debug.Log("아이디 4글자 이상. 비밀번호 6글자 이상");
        }
    }    

    //로그아웃
    public void OnClickLogoutButton()
    {
        Debug.Log(networkManager.ToString());
        networkManager.Logout();
    }

    //x버튼
    public void OnClickQuitButton()
    {
        currentPanel = null;
        statusPanel.SetActive(false);
        itemPanel.SetActive(false);
        informationPanel.SetActive(false);
    }

    //스텟 버튼
    public void OnCLickStatusButton()
    {
        if(currentPanel != statusPanel)
        {
            SetPanel();
            informationPanel.SetActive(true);
            statusPanel.SetActive(true);
            currentPanel = statusPanel;
            SetStatus();
        }
    }

    //장비 버튼
    public void OnClickItemButton()
    {
        if(currentPanel != itemPanel)
        {
            SetPanel();
            informationPanel.SetActive(true);
            itemPanel.SetActive(true);
            currentPanel = itemPanel;
            SetItemSlot();
        }
    }

    //유닛 버튼
    public void OnClickUnitButton()
    {
        if (currentPanel != unitPanel)
        {
            SetPanel();
            informationPanel.SetActive(true);
            unitPanel.SetActive(true);
            currentPanel = unitPanel;
            unitUIManager.SetUnitLevel();
        }
    }

    //건물 버튼
    public void OnClickBuildingButton()
    {
        if(currentPanel != buildingPanel)
        {
            SetPanel();
            informationPanel.SetActive(true);
            buildingPanel.SetActive(true);
            currentPanel = buildingPanel;
            buildingUIManager.SetBuilding();
        }
    }

    //패널 끄기
    public void SetPanel()
    {
        if (currentPanel != null)
        {
            if (currentPanel == buildingPanel)
            {
                buildingUIManager.buildingState.SetActive(false);
            }
            else if (currentPanel == unitPanel)
            {
                unitUIManager.unitState.SetActive(false);
            }

            currentPanel.SetActive(false);
        }
    }

    //스크롤뷰 셋팅
    public void SetUnitScrollView()
    {
        unitStateScroll = GameObject.Find("UnitStateScroll");

        unitStateScroll.GetComponent<RectTransform>().localPosition = Vector3.zero;

        if (dataManager.Unit.Length > 7)
        {
            float yMax = 350 + (50 * (dataManager.Unit.Length - 7));
            unitStateScroll.GetComponent<RectTransform>().sizeDelta = new Vector2(200, yMax);
        }
        else
        {
            unitStateScroll.GetComponent<RectTransform>().sizeDelta = new Vector2(200, 350);
        }

        for (int i = 0; i < dataManager.Unit.Length; i++)
        {
            GameObject unit = Instantiate(Resources.Load("/Prefabs/Unit")) as GameObject;
            unit.transform.SetParent(unitStateScroll.transform);
            float posY = 100 - (i * 50);
            unit.GetComponent<RectTransform>().localPosition = new Vector3(0, posY, 0);
        }
    }

    //상태창 셋팅
    public void SetState()
    {
        SetHeroState();
        SetCastleState();
        SetResource();
    }

    //영웅 상태 셋팅
    public void SetHeroState()
    {
        heroState = GameObject.Find("HeroState").GetComponent<Text>();

        if (dataManager.HState == DataManager.HeroState.Stationed)
        {
            heroState.text = "주둔중";
        }
        else if (dataManager.HState == DataManager.HeroState.Attack)
        {
            heroState.text = "공격중";
        }
        else if (dataManager.HState == DataManager.HeroState.Return)
        {
            heroState.text = "복귀중";
        }
        else if (dataManager.HState == DataManager.HeroState.Dead)
        {
            heroState.text = "사망";
        }
    }

    //성 상태 셋팅
    public void SetCastleState()
    {
        castleState = GameObject.Find("CastleState").GetComponent<Text>();

        if (dataManager.CState == DataManager.CastleState.Peace)
        {
            castleState.text = "평화로움";
        }
        else if (dataManager.CState == DataManager.CastleState.Famine)
        {
            castleState.text = "기근";
        }
        else if (dataManager.CState == DataManager.CastleState.BeingAttacked)
        {
            castleState.text = "공격당하는중";
        }
        else if (dataManager.CState == DataManager.CastleState.Attacked)
        {
            castleState.text = "공격당함";
        }
    }
    
    //건설 상태 셋팅
    public void SetBuildState()
    {
        Building building = BuildingDatabase.Instance.GetBuildingData(dataManager.BuildBuilding);

        if (building.ID != BuildingId.None)
        {
            buildName.text = building.Name;
            buildTime.text = BuildingDatabase.Instance.buildingData[dataManager.BuildBuilding].BuildingData[dataManager.Building[(int)currentBuilding]].BuildTime.ToString();
        }
        else
        {
            buildName.text = "None";
            buildTime.text = "00:00:00";
        }        
    }

    //자원 셋팅
    public void SetResource()
    {
        resource = GameObject.Find("Resource").GetComponent<Text>();
        resource.text = dataManager.Resource.ToString();
    }

    //스텟 셋팅
    public void SetStatus()
    {
        level.text = dataManager.HeroData.Leveldata[0].Level.ToString();
        experience.text = dataManager.HeroData.Leveldata[0].Experience.ToString();
        health.text = dataManager.HeroData.Leveldata[0].Health.ToString();
        mana.text = dataManager.HeroData.Leveldata[0].Mana.ToString();
        attack.text = dataManager.HeroData.Leveldata[0].Attack.ToString();
        defense.text = dataManager.HeroData.Leveldata[0].Defense.ToString();
        magicDefense.text = dataManager.HeroData.Leveldata[0].MagicDefense.ToString();
        moveSpeed.text = dataManager.HeroData.Leveldata[0].MoveSpeed.ToString();
        attackSpeed.text = dataManager.HeroData.Leveldata[0].AttackSpeed.ToString();
    }

    //장비 UI 셋팅
    public void SetItemSlot()
    {
        for (int i = 0; i < DataManager.equipNum; i++)
        {
            equipment[i].transform.FindChild("Item").GetComponent<Image>().sprite = Resources.Load<Sprite>("Icon/" + ItemDatabase.Instance.itemData[dataManager.Equipment[i]].Name) as Sprite;
        }

        for (int i = 0; i < DataManager.invenNum; i++)
        {
            inventory[i].transform.FindChild("Item").GetComponent<Image>().sprite = Resources.Load<Sprite>("Icon/" + ItemDatabase.Instance.itemData[dataManager.InventoryId[i]].Name) as Sprite;
        }
    }

    //슬롯 생성
    public void CreateEquipmentSlot()
    {
        equipment = new GameObject[DataManager.equipNum];

        for (int i = 0; i < DataManager.equipNum; i++)
        {
            equipment[i] = Instantiate(Resources.Load("Prefabs/Slot")) as GameObject;
            equipment[i].transform.SetParent(equipmentPanel.transform);
        }

        equipment[0].name = "Helmet";
        equipment[0].GetComponent<RectTransform>().localPosition = new Vector3(-160, 180, 0);
        equipment[1].name = "Weapon";
        equipment[1].GetComponent<RectTransform>().localPosition = new Vector3(-235, -50, 0);
        equipment[2].name = "Armor";
        equipment[2].GetComponent<RectTransform>().localPosition = new Vector3(-160, 20, 0);
        equipment[3].name = "Gloves";
        equipment[3].GetComponent<RectTransform>().localPosition = new Vector3(-85, -50, 0);
        equipment[4].name = "Shoes";
        equipment[4].GetComponent<RectTransform>().localPosition = new Vector3(-160, -150, 0);
        equipment[5].name = "Ring";
        equipment[5].GetComponent<RectTransform>().localPosition = new Vector3(-85, 60, 0);
        equipment[6].name = "Necklace";
        equipment[6].GetComponent<RectTransform>().localPosition = new Vector3(-160, 100, 0);
    }

    //슬롯 생성
    public void CreateInventorySlot()
    {
        inventory = new GameObject[DataManager.invenNum];

        for (int i = 0; i < DataManager.invenNum; i++)
        {
            inventory[i] = Instantiate(Resources.Load("Prefabs/Slot")) as GameObject;
            inventory[i].transform.SetParent(inventoryPanel.transform);
            inventory[i].name = "Slot " + (1 + i);
            inventory[i].GetComponent<RectTransform>().localPosition = new Vector3(15 + (65 * (i % 4)), 35 - (65 * (i / 4)), 0);
        }
    }

    //건물 시간 체크
    public IEnumerator BuildTimeCheck()
    {
        while (dataManager.BuildBuilding != DataManager.buildingNum)
        {
            yield return new WaitForFixedUpdate();
            if (dataManager.BuildBuilding != (int)BuildingId.None)
            {
                buildName.text = BuildingDatabase.Instance.buildingData[dataManager.BuildBuilding].Name;
            }
            else
            {
                buildName.text = "None";
            }

            buildTime.text = (dataManager.BuildTime - DateTime.Now).Hours.ToString("00") + ":" + (dataManager.BuildTime - DateTime.Now).Minutes.ToString("00") + ":" + (dataManager.BuildTime - DateTime.Now).Seconds.ToString("00");

            if (dataManager.BuildTime < DateTime.Now)
            {
                networkManager.BuildComplete();
                dataManager.BuildBuilding = 6;
                dataManager.BuildTime = DateTime.Now;
                SetBuildState();
                break;
            }
        }
    }

    //유닛 생산 시간 체크
    public IEnumerator UnitCreateTimeCheck()
    {
        

        while (dataManager.CreateUnitKind != 0)
        {
            yield return new WaitForFixedUpdate();

            unitCreateName.text = "";

            for (int i=0; i < dataManager.CreateUnit.Length; i++)
            {
                if (dataManager.CreateUnit[i].Id != 0)
                {
                    unitCreateName.text += UnitDatabase.Instance.unitData[dataManager.CreateUnit[i].Id].Name + " x ";
                    unitCreateName.text += dataManager.CreateUnit[i].num + ", ";
                }
            }

            unitCreateTime.text = (dataManager.UnitCreateTime - DateTime.Now).Hours.ToString("00") + ":" + (dataManager.UnitCreateTime - DateTime.Now).Minutes.ToString("00") + ":" + (dataManager.UnitCreateTime - DateTime.Now).Seconds.ToString("00");

            if (dataManager.UnitCreateTime < DateTime.Now)
            {
                networkManager.UnitCreateComplete();
                dataManager.BuildBuilding = 6;
                dataManager.BuildTime = DateTime.Now;
                SetBuildState();
                break;
            }
        }
    }

    //게임종료
    public void OnClickExitButton()
    {
        Application.Quit();
    }

    public static IEnumerator DialogCtrl(float value, string text)
    {
        if (!dialog.activeSelf)
        {
            dialog.SetActive(true);

            dialog.transform.FindChild("Text").GetComponent<Text>().text = text;

            yield return new WaitForSeconds(value);

            dialog.SetActive(false);
        }
    }
}