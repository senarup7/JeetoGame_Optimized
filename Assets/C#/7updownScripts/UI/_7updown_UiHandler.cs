using UpDown7.Utility;
using Updown7.ServerStuff;
using System.Collections;
using System.Collections.Generic;
using TMPro;
using Shared;
using UnityEngine;
using UnityEngine.UI;
using Updown7.ServerStuff;
using Updown7.Gameplay;
using System;

namespace Updown7.UI
{

    public class _7updown_UiHandler : MonoBehaviour
    {

        [SerializeField] Toggle chit10Btn;
        [SerializeField] Toggle chit50Btn;
        [SerializeField] Toggle chit100Btn;
        [SerializeField] Toggle chit500Btn;
        [SerializeField] Toggle chit1000Btn;
        [SerializeField] Toggle chit5000Btn;

        [SerializeField] TMP_Text leftBets;
        [SerializeField] TMP_Text middleBets;
        [SerializeField] TMP_Text rightBets;
        [SerializeField] TMP_Text usernameTxt;
        [SerializeField] TMP_Text balanceTxt;

        [SerializeField] Button lobby;

        Dictionary<Spot, TMP_Text> betUiRefrence = new Dictionary<Spot, TMP_Text>();
        public TMP_Text PlayerBet;
        float playerbetvalue = 0;
         public Chip currentChip;
        _7updown_Timer timer;
        public GameObject mainUnite;
        float balance;
        private void Start()
        {
            playerbetvalue = 0;
            PlayerBet.text = "0";
            currentChip = Chip.Chip10;
            betUiRefrence.Add(Spot.left, leftBets);
            betUiRefrence.Add(Spot.middle, middleBets);
            betUiRefrence.Add(Spot.right, rightBets);
            countdownPanel.gameObject.SetActive(false);
            placeBets.gameObject.SetActive(false);
            timer = mainUnite.GetComponent<_7updown_Timer>();
            //test.onClick.AddListener(() => StartCoroutine(StartCountDown()));
            SoundManager.instance.PlayBackgroundMusic();
            AddListeners();
            ResetUi();
            UpdateUi();
            //StartCoroutine(Loading());
        }

        IEnumerator StartBetting()
        {
            placeBets.gameObject.SetActive(true);
            placeBets.sprite = startBetting;
            yield return new WaitForSeconds(.5f);
            placeBets.gameObject.SetActive(false);
        }
        int leftTotalBets;
        int middleTotalBets;
        int rightTotalBets;
        public void AddBotsBets(Spot spot, Chip chip)
        {
            string betValue = string.Empty;
            switch (spot)
            {
                case Spot.left:
                    leftTotalBets += (int)chip;
                    betValue = leftTotalBets.ToString();
                    break;
                case Spot.middle:
                    middleTotalBets += (int)chip;
                    betValue = middleTotalBets.ToString();
                    break;
                case Spot.right:
                    rightTotalBets += (int)chip;
                    betValue = rightTotalBets.ToString();
                    break;
                default:
                    break;
            }
            UpdateUi();
        }
        public void AddPlayerBets()
        {
            balance -= (float)currentChip;
            playerbetvalue += (float)currentChip;
            PlayerBet.text = playerbetvalue.ToString();
            UpdateUi();
        }
        public bool IsEnoughBalancePresent()
        {
            return balance - (float)currentChip > 0;
        }
        public void UpDateBalance(float amount)
        {
            StopLoading();
            isLoading = false;
            balance = amount;
            UpdateUi();
        }
        public _7updown_LeftSideMenu sideMenu;
        private void AddListeners()
        {
            chit10Btn.onValueChanged.AddListener((isOn) =>
            {
                currentChip = Chip.Chip10;
            });
            chit50Btn.onValueChanged.AddListener((isOn) =>
            {
                currentChip = Chip.Chip50;
            });
            chit100Btn.onValueChanged.AddListener((isOn) =>
            {
                currentChip = Chip.Chip100;
            });
            chit500Btn.onValueChanged.AddListener((isOn) =>
            {
                currentChip = Chip.Chip500;
            });
            chit1000Btn.onValueChanged.AddListener((isOn) =>
            {
                currentChip = Chip.Chip1000;
            });
            chit5000Btn.onValueChanged.AddListener((isOn) =>
            {
                currentChip = Chip.Chip5000;
            });
            timer.onCountDownStart += () =>
            {
                if (timer.is_a_FirstRound) return;
                StartCoroutine(StartBetting());
            };
            timer.startCountDown += () =>
            {
                SoundManager.instance.PlayCountdown();
                StartCoroutine(StartCountDown());
            };
            lobby.onClick.AddListener(() => sideMenu.ShowPopup());
        }
       public void ResetUi()
        {
            playerbetvalue = 0;
            PlayerBet.text = "0";
            leftTotalBets = 0;
            middleTotalBets = 0;
            rightTotalBets = 0;
            leftBets.text = string.Empty;
            middleBets.text = string.Empty;
            rightBets.text = string.Empty;
        }
        public void SetBets(int left,int middle,int right)
        {
            leftTotalBets = left;
            middleTotalBets = middle;
            rightTotalBets = right;
            UpdateUi();
        }
       public void UpdateUi()
        {
            leftBets.text = leftTotalBets.ToString();
            middleBets.text = middleTotalBets.ToString();
            rightBets.text = rightTotalBets.ToString();
            balanceTxt.text = balance.ToString();
            usernameTxt.text ="000"+ LocalPlayer.deviceId;
        }


        [SerializeField] GameObject messagePopUP;
        [SerializeField] TMP_Text msgTxt;
        public void ShowMessage(string msg)
        {
            messagePopUP.SetActive(true);
            msgTxt.text = msg;
        }
        public void HideMessage()
        {
            messagePopUP.SetActive(false);
            msgTxt.text = string.Empty;
        }

        private void OnApplicationQuit()
        {
            LocalPlayer.balance = balance.ToString();
            LocalPlayer.SaveGame();
        }
        public void AutoHidehowMessage(string msg, int time)
        {
            StartCoroutine(HidePanel(time));
            messagePopUP.SetActive(true);
            msgTxt.text = msg;
        }
        IEnumerator HidePanel(int time)
        {
            yield return new WaitForSeconds(time);
            messagePopUP.SetActive(false);
        }
        public void OnPlayerWin(object o)
        {
            Win win = UpDown7.Utility.Utility.GetObjectOfType<Win>(o);
            balance += win.winAmount;
            UpdateUi();
        }
        public Sprite[] frams;
        public Sprite stopBetting;
        public Sprite startBetting;
        public Image countdownPanel;
        public Image placeBets;
        public float countdownSpeed=.25f;
        public Button test;

        IEnumerator StartCountDown()
        {
            countdownPanel.gameObject.SetActive(true);
            foreach (var item in frams)
            {
                countdownPanel.sprite = item;
                yield return new WaitForSeconds(countdownSpeed);
            }
            countdownPanel.gameObject.SetActive(false);
            placeBets.gameObject.SetActive(true);
            placeBets.sprite = stopBetting;
            yield return new WaitForSeconds(0.5f);
            placeBets.gameObject.SetActive(false);
        }

        public Image loadingpnel;
        public Image loadingImag;
        public Sprite[] loadingFrames;
        bool isLoading = true;
        IEnumerator Loading()
        {
            loadingpnel.gameObject.SetActive(true);
            foreach (var item in loadingFrames)
            {
                if (!isLoading) yield break;
                loadingImag.sprite = item;
                yield return new WaitForEndOfFrame();
            }
            StartCoroutine(Loading());
        }
        public void StopLoading()
        {
            StopCoroutine(Loading());
            loadingpnel.gameObject.SetActive(false);

        }



    }
    
}

[Serializable]
public class Win
{
    public float winAmount;
}