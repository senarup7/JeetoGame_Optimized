using Com.BigWin.WebUtils;
using LobbyScripts;
using System;
using System.Collections;
using System.Collections.Generic;
using System.Linq;
using UnityEngine;
using UnityEngine.SceneManagement;
using UnityEngine.UI;
using LobbyScripts;
using Newtonsoft.Json;
public class HomeScript : MonoBehaviour
{
    public static HomeScript Instance;
    public GameObject HomePanel;
    public Text NameTxt;
    public Text IDTxt;
    public Image Profile;
    public Text Balance;
    AsyncOperation DragonScene;
    AsyncOperation sevenupScene;
    public Image LobbyAnimpnel;
    public Sprite[] Lobbyframe;
    [SerializeField]
    Text LoadingWaitText;
    [SerializeField]
    List<Button> ButtonList = new List<Button>();

    private void Awake()
    {
        Instance = this;
    }
    private void Start()
    {

        StartCoroutine(Loading());
    }
    IEnumerator Loading()
    {
        foreach (var item in Lobbyframe)
        {
            LobbyAnimpnel.sprite = item;
            yield return new WaitForEndOfFrame();
        }
        StartCoroutine(Loading());
    }

    IEnumerator LoadYourAsyncScene(string scene)
    {
        // The Application loads the Scene in the background as the current Scene runs.
        // This is particularly good for creating loading screens.
        // You could also load the Scene by using sceneBuildIndex. In this case Scene2 has
        // a sceneBuildIndex of 1 as shown in Build Settings.

        AsyncOperation asyncLoad = SceneManager.LoadSceneAsync(scene);
        LoadingWaitText.gameObject.SetActive(true);
        // Wait until the asynchronous scene fully loads
        while (!asyncLoad.isDone)
        {
            LoadingWaitText.text = "Loading...Please Wait";
            Debug.Log("Loading........");
            yield return null;
        }
        LoadingWaitText.gameObject.SetActive(false);
    }

    public void ShowHomeUI()
    {
        //DragonScene = SceneManager.LoadSceneAsync("DragonScene");
        //DragonScene.allowSceneActivation = false;
        //sevenupScene = SceneManager.LoadSceneAsync("7updown");
        //sevenupScene.allowSceneActivation = false;
        Balance.text = UserDetail.Balance.ToString();
        Profile.sprite = ProfileScript.Instance.ProfileSprite[UserDetail.ProfileId];
        NameTxt.text = UserDetail.Name;
        IDTxt.text = "id:" + UserDetail.ID;
        HomePanel.SetActive(true);
        SetButton_Interactable(true);
        User user = new User() { 
        user_id = UserDetail.UserId, version_code = 1, language = "en" 
        };
        WebRequestHandler.instance.Post(Constants.ProfileURL,JsonUtility.ToJson(user), (data, status) => {
            if (!status) return;

            Profile profile = JsonConvert.DeserializeObject<Profile>(data);
            //print("balance is " + profile.response.data.chip_balance);
            Balance.text = profile.response.data.chip_balance.ToString();
            UserDetail.Balance = int.Parse(profile.response.data.chip_balance.ToString());
            print("Profile Data " + data);
        });

       
    }

    void SetButton_Interactable(bool val)
    {
        for (int i=0; i < ButtonList.Count; i++)
        {
            ButtonList[i].interactable = val;
        }
    }
    

    public void DragonBtn()
    {
        SetButton_Interactable(false);
        AndroidToastMsg.ShowAndroidToastMessage("Loading");
        //sevenupScene.allowSceneActivation = false;
        //DragonScene.allowSceneActivation = true;
        AndroidToastMsg.ShowAndroidToastMessage("Loading");
        //SceneManager.LoadScene("DragonScene");
        StartCoroutine(LoadYourAsyncScene("DragonScene"));
    }
    public void SevenUpBtn()
    {
        SetButton_Interactable(false);
        AndroidToastMsg.ShowAndroidToastMessage("Loading");
        //DragonScene.allowSceneActivation = false;
        //sevenupScene.allowSceneActivation = true;
        //SceneManager.LoadScene("7updown");
        StartCoroutine(LoadYourAsyncScene("7updown"));
    }
    public void AndarBaharBtn()
    {
        SetButton_Interactable(false);
        AndroidToastMsg.ShowAndroidToastMessage("Loading");
       // SceneManager.LoadScene("AndarBahar");
        StartCoroutine(LoadYourAsyncScene("AndarBahar"));
    }
    public void ShopBtn()
    {
        ShopScript.Instance.ShowShopUI();
    }
    public void ProfileBtn()
    {
        ProfileScript.Instance.ShowProfileUI();
    }
    public void settingBtn()
    {
        SettingScript.Instance.ShowSettingUI();
    }
    public void SupportBtn()
    {
        SupportScript.Instance.ShowSupportUI();
    }
    public void NoticeBtn()
    {
        NoticeScript.Instance.ShowNoticeUI();
    }
    public void MailBtn()
    {
        MailScript.Instance.ShowMailUI();
    }
    public void ShareBtn()
    {
        ReferAndEarnScript.Instance.ShowReferAndEarnUI();
    }
    public void SafeBtn()
    {
        SafeScript.Instance.ShowSafeUI();
    }
    public void RankBtn()
    {
        RankScript.Instance.ShowRankUI();
    }
    public void WithdrawBtn()
    {
        WithDrawScript.Instance.ShowWithDrawUI();
    }

    public void DiamondBtn()
    {
        //DiamondScript.Instance.ShowDiamondUI();
        DiamondOffer.Instance.ShowDiamondUI();
    }

    public void AgentBtn()
    {
        AgentScript.Instance.ShowDiamondUI();
    }
    public static string RandomString(int length)
    {
        string element = "ABCDEFGHIJKLMNOPQRSTUVWXYZ0123456789";
        System.Random random = new System.Random();
        return new string((from s in Enumerable.Repeat(element, length)
                           select s[random.Next(s.Length)]).ToArray());
    }
}
public class LobbyData
{
    public string playerId;
}

public class ProfileData
{
    public string username;
                  public int chip_balance ;
                  public int safe_balance ;
                     public int vip_level ;
         public string invite_friend_code ;
             public int team_name_updated ;
                      public string image ;
    public List<object> refered_to_friend ;
                     public string gender ;
              public string referal_bonus ;
             public bool account_verified ;
               public string Signup_bonus ;
             public string referral_bonus ;
             public int refered_by_status ;
               public string version_code ;
                    public string apk_url ;
}

public class ProfileResponse
{
    public bool status;
    public string message;
    public ProfileData data;
}

[Serializable]
public class Profile
{
    public ProfileResponse response;
}

public class User
{
    public int user_id;
    public int version_code;
    public string language;
}
