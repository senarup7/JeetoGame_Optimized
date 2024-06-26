using Com.BigWin.WebUtils;
using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class ForgetPasswordScript : MonoBehaviour
{
    public static ForgetPasswordScript Instance;
    public GameObject ForgetPasswordPanel;
    public InputField MobileNo;
    public InputField Password;
    public InputField ReEnterPassword;
    public InputField OTP;
    public Text Message;
    string ForgetPasswordURL = "https://jeetogame.in/jeeto_game_new/WebServices/forgotPassword";
   // string ForgetPasswordURL = "http://3.109.48.186/Backend_code/rummy/WebServices/forgotPassword";
    private void Awake()
    {
        Instance = this;
    }
    public void ShowForgetPasswordUI()
    {
        ForgetPasswordPanel.SetActive(true);
    }
    public void CloseForgetPasswordUI()
    {
        ForgetPasswordPanel.SetActive(false);
    }
    /*public void OTPVerifyBtn()
    {
        if (MobileNo.text != "")
        {
            string device_id = SystemInfo.deviceUniqueIdentifier;
            RegisterForm form = new RegisterForm(MobileNo.text, device_id);
            WebRequestHandler.instance.Post(RegisterScript.OTPURL, JsonUtility.ToJson(form), OnOtpVerifyRequestProcessed);
        }
    }*/
    private void OnOtpVerifyRequestProcessed(string json, bool success)
    {
        LoginFormRoot responce = JsonUtility.FromJson<LoginFormRoot>(json);
        Debug.Log(responce.response.message);
        AndroidToastMsg.ShowAndroidToastMessage(responce.response.message);       
    }
    public void OkBtn()
    {
        //  if (MobileNo.text != "" && OTP.text != "")
        if (MobileNo.text != "" )
        {
            if (Password.text == ReEnterPassword.text)
            {
                ForgetForm form = new ForgetForm(MobileNo.text, Password.text, "en");
                WebRequestHandler.instance.Post(ForgetPasswordURL, JsonUtility.ToJson(form), OnForgetPasswordRequestProcessed);
            }
            else
            {
                Message.text = "Password & Re-Enter Password Mismatched";
            }
        }
        else
        {
            if (MobileNo.text.Length < 10)
            {
                Message.text = "Enter Valid Mobile No";
            }
        }
    }
    private void OnForgetPasswordRequestProcessed(string json, bool success)
    {
        LoginFormRoot responce = JsonUtility.FromJson<LoginFormRoot>(json);
        Debug.Log(responce.response.message);
        Message.text = responce.response.message;
        PlayerPrefs.SetString("MobileNum", MobileNo.text);
        PlayerPrefs.SetString("password", Password.text);
        PlayerPrefs.Save();
        AndroidToastMsg.ShowAndroidToastMessage(responce.response.message);

    }
}
[Serializable]
public class ForgetForm
{
    public string mobile_number;
    public string password;
    public string language;
   // public string otp;

    public ForgetForm(string mobile_number, string password, string language)
    {
        this.mobile_number = mobile_number;
        this.password = password;
        this.language = language;
      //  this.otp = otp;
    }
}