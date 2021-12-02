using Com.BigWin.WebUtils;
using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class LoginScript : MonoBehaviour
{
    public static LoginScript Instance;
    public GameObject LoginPanel;
    public GameObject LoginPanel1;
    public InputField Password;
    public InputField Mobile;
    string LoginURL = "https://jeetogame.in/jeeto_game/WebServices/loginPassword";    
    string GuestURL = "https://jeetogame.in/jeeto_game/WebServices/Guestlogin";

    private void Awake()
    {
        Instance = this;
    }
    public void ShowLoginUI()
    {
        LoginPanel.SetActive(true);
        LoginPanel1.SetActive(false);
    }
    public void LoginBtn()
    {
        if (Mobile.text != "" && Password.text != "")
        {
            LoginForm form = new LoginForm(Mobile.text, Password.text, "en");
            WebRequestHandler.instance.Post(LoginURL, JsonUtility.ToJson(form), OnLoginRequestProcessed);
        }
    }
    private void OnLoginRequestProcessed(string json, bool success)
    {
        LoginFormRoot responce = JsonUtility.FromJson<LoginFormRoot>(json);
        Debug.Log(json);

        if (responce.response.status)
        {
            PlayerPrefs.SetString("MobileNum", Mobile.text);
            PlayerPrefs.SetString("password", Password.text);
            PlayerPrefs.Save();

            UserDetail.UserId = responce.response.data.user_id;
            UserDetail.ID = responce.response.data.id.ToString();
            UserDetail.Name = responce.response.data.name;
            UserDetail.ProfileId = responce.response.data.profile_id;
            UserDetail.MobileNo = responce.response.data.mobile_number;
            UserDetail.Balance = responce.response.data.chip_balance;
            UserDetail.refer_id = responce.response.data.refer_id;          
            LoginPanel.SetActive(false);
            LoginPanel1.SetActive(false);
            HomeScript.Instance.ShowHomeUI();
        }
        else
        {

        }
    }    
    public void ForgetPasswordBtn()
    {
        ForgetPasswordScript.Instance.ShowForgetPasswordUI();
    }   
    public void LoginScreenBtn()
    {
        LoginPanel.SetActive(false);
        LoginPanel1.SetActive(true);
    }
    public void PlayGuestBtn()
    {
        string device_id = SystemInfo.deviceUniqueIdentifier;
        GuestForm form = new GuestForm(device_id, "en");
        WebRequestHandler.instance.Post(GuestURL, JsonUtility.ToJson(form), OnGuestRequestProcessed);       
    }
    public void CloseUI()
    {
        LoginPanel.SetActive(true);
        LoginPanel1.SetActive(false);
    }
    private void OnGuestRequestProcessed(string json, bool success)
    {
        GuestRes responce = JsonUtility.FromJson<GuestRes>(json);

        if (responce.response.status)
        {
            PlayerPrefs.SetString("GuestData", json);
            PlayerPrefs.Save();            
            UserDetail.UserId = responce.response.data.user_id;
            UserDetail.ID = responce.response.data.id.ToString();
            UserDetail .Name = responce.response.data.name;
            UserDetail.ProfileId = responce.response.data.profile_id;
            UserDetail.Balance = responce.response.data.chip_balance;
            UserDetail.refer_id = responce.response.data.refer_id;
            LoginPanel.SetActive(false);
            LoginPanel1.SetActive(false);
            HomeScript.Instance.ShowHomeUI();
        }
    }  
}
[Serializable]
public class LoginForm
{
    public string mobile_number;
    public string password;
    public string language;

    public LoginForm(string mobile_number, string password, string language)
    {
        this.mobile_number = mobile_number;
        this.password = password;
        this.language = language;
    }
}
[Serializable]
public class LoginFormData
{
    public int id;
    public int user_id;
    public string name;
    public int chip_balance;
    public int profile_id;
    public string refer_id;
    public string mobile_number;
}
[Serializable]
public class LoginFormResponse
{
    public bool status;
    public string message;
    public LoginFormData data;
}
[Serializable]
public class LoginFormRoot
{
    public LoginFormResponse response;
}
[Serializable]
public class GuestForm
{
    public string device_id;
    public string language;

    public GuestForm(string device_id, string language)
    {
        this.device_id = device_id;
        this.language = language;
    }
}
[Serializable]
public class GuestData
{
    public string id;
    public int user_id;
    public string name;
    public int chip_balance;
    public int profile_id;
    public string refer_id;
}
[Serializable]
public class GuestResponse
{
    public bool status;
    public string message;
    public GuestData data;
}
[Serializable]
public class GuestRes
{
    public GuestResponse response;
}