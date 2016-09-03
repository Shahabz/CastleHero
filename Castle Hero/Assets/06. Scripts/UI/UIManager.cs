using UnityEngine;
using UnityEngine.UI;

using System.Collections;

public class UIManager : MonoBehaviour
{
    public Text loginId;
    public Text loginPw;
    public Text createId;
    public Text createPw;
    public Text deleteId;
    public Text deletePw;

    NetWorkManager networkManager;

    public GameObject createAccountPanel;
    public GameObject deleteAccountPanel;
    public GameObject lastChoicePanel;
    public GameObject dialog;

    void Start()
    {
        networkManager = GameObject.FindGameObjectWithTag("NetworkManager").GetComponent<NetWorkManager>();
    }

    //가입하기
    public void OnClickCreateButton()
    {
        createAccountPanel.SetActive(true);
    }

    //아이디, 비밀번호 입력후 가입하기
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
    public void OnClickCreateCancleButton()
    {
        createAccountPanel.SetActive(false);
    }

    //탈퇴하기
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
    public void OnClickDeteleCancleButton()
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

    //로그인
    public void OnClickLoginButton()
    {
        networkManager.Login(loginId.text, loginPw.text);
    }

    //게임종료
    public void OnClickExitGame()
    {
        Application.Quit();
    }

    public IEnumerator DialogCtrl(float value)
    {
        if (!dialog.activeSelf)
        {
            dialog.SetActive(true);

            yield return new WaitForSeconds(value);

            dialog.SetActive(false);
        }
    }
}