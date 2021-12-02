using AndarBahar.Utility;
using Shared;
using System;
using System.Collections;
using UnityEngine;
using UnityEngine.UI;
using TMPro;
using System.Collections.Generic;
using Newtonsoft.Json;
using DG.Tweening;
using AndarBahar.Gameplay;
using AndarBahar.UI;

namespace AndarBahar.UI
{
    public class AndarBahar_UiHandler : MonoBehaviour
    {
        public Toggle chit10Btn;
        public Toggle chit50Btn;
        public Toggle chit100Btn;
        public Toggle chit500Btn;
        public Toggle chit1000Btn;
        public Toggle chit5000Btn;

        public Button sidemenuBtn;
        public GameObject mainUnit;

        public bool IsEnoughBalanceAvailable()
        {
            return balance > (int)currentChip;
        }
        public void AddBets()
        {
            balance -= (float)currentChip;
            UpdateUI();
        }


        float balance;
        int totalBets;
        int andarBets;
        int baharBets;
        int oneToFiveBets;
        int sixToTenBets;
        int elevenToFifteenBets;
        int sixteenToTwentyFiveBets;
        int twentySixToThirtyBets;
        int thirtyOneToThirtyFiveBets;
        int thirtySixToFourtyBets;
        int fourtyAndMoreBets;

        public TMP_Text andarTxt;
        public TMP_Text baharTxt;
        public TMP_Text oneToFiveTxt;
        public TMP_Text sixToTenTxt;
        public TMP_Text eleventToFifteenTxt;
        public TMP_Text sixteenToTwentyFiveTxt;
        public TMP_Text twentySixToThirtyTxt;
        public TMP_Text thirtyOneToThirtyFiveTxt;
        public TMP_Text thirtySixToFourtyBetsTxt;
        public TMP_Text fourtyAndMoreTxt;

        public TMP_Text balanceTxt;
        public TMP_Text userNameTxt;

        bool isSideMenuOpen;
        public GameObject sideMenu;
        private void Start()
        {
            isSideMenuOpen = false;
            sidemenuBtn.onClick.AddListener(() =>
            {
                if (sideMenu.activeSelf)
                {
                    sideMenu.gameObject.SetActive(false);
                    return;
                }
                    sideMenu.gameObject.SetActive(true);
            });
            messagePanel.SetActive(true);
            currentChip = Chip.Chip10;
            chit10Btn.isOn = true;
            chit10Btn.Select();
            SoundManager.instance.PlayBackgroundMusic();
            AddListeners();
        }
        string AndarTxt;
        public Chip currentChip;
        public GameObject mainUnite;
        public AndarBahar_Timer timer;
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
            Debug.Log(timer == null);
            timer.onTimerEnd += () =>
            {
                if (timer.is_a_FirstRound) return;
                StartCoroutine(TransitionAnimation(stopBettingSprite));
            };
            timer.onTimerStart += () =>
            {
                if (timer.is_a_FirstRound) return;
                StartCoroutine(TransitionAnimation(startBettingSprite));
            };
            timer.onWait += () =>
            {
                ResetUI();
            };
            //lobby.onClick.AddListener(() => sideMenu.ShowPopup());
        }

        public void UpdateBets(Spots spot, Chip c)
        {

            int bet = (int)c;
            switch (spot)
            {
                case Spots.Andar:
                    andarBets += bet;
                    break;
                case Spots.Bahar:
                    baharBets += bet;
                    break;
                case Spots.oneToFive:
                    oneToFiveBets += bet;
                    break;
                case Spots.sixToTen:
                    sixToTenBets += bet;
                    break;
                case Spots.elevenToFifteen:
                    elevenToFifteenBets += bet;
                    break;
                case Spots.sixteenToTwentyFive:
                    sixteenToTwentyFiveBets += bet;
                    break;
                case Spots.twentySixToThirty:
                    twentySixToThirtyBets += bet;
                    break;
                case Spots.thirtyOneToThirtyFive:
                    thirtyOneToThirtyFiveBets += bet;
                    break;
                case Spots.thirtySixToFouty:
                    thirtySixToFourtyBets += bet;
                    break;
                case Spots.fortyOneAndMore:
                    fourtyAndMoreBets += bet;
                    break;
                default:
                    break;
            }
            UpdateUI();
        }

        public TMP_Text winnerCard;
        public void DisplayWinnerCardOnDashBoard(Card c)
        {
            int cardNo = c.cardNo;
            if (cardNo == 10)
            {
                winnerCard.text = "J";
            }
            else if (cardNo == 11)
            {
                winnerCard.text = "Q";
            }
            else if (cardNo == 12)
            {
                winnerCard.text = "K";
            }
            else
            {
                winnerCard.text = cardNo.ToString();
            }
        }
        public Sprite startBettingSprite;
        public Sprite stopBettingSprite;
        public GameObject startStopImgHolder;
        public Transform startingPoint;
        public Transform middlePoint;
        public Transform finishingPoint;
        public iTween.EaseType easeType;
        public float transitionTime = 1, delay = .1f;
        IEnumerator TransitionAnimation(Sprite img)
        {
            startStopImgHolder.GetComponent<Image>().sprite = img;
            startStopImgHolder.transform.position = startingPoint.position;
            startStopImgHolder.transform.DOLocalMoveX(middlePoint.position.x, transitionTime, false);
            //iTween.MoveTo(startStopImgHolder, iTween.Hash("position", middlePoint.position, "time", transitionTime, "easetype", easeType));
            yield return new WaitForSeconds(transitionTime + delay);
            startStopImgHolder.GetComponent<RectTransform>().DOBlendableMoveBy(finishingPoint.position, transitionTime, true);
            //iTween.MoveTo(startStopImgHolder, iTween.Hash("position", finishingPoint.position, "time", transitionTime, "easetype", easeType));
        }
        public void HideMessage()
        {

            messagePanel.SetActive(false);
        }
        public GameObject messagePanel;
        public TMP_Text messageTxt;
        public void ShowMessage(string msg)
        {
            messagePanel.SetActive(true);
            messageTxt.text = msg;
        }

        public void OnPlayerWin(object o)
        {
            Win win = JsonConvert.DeserializeObject<Win>(o.ToString());
            StartCoroutine(waitFor(win.winAmount));
        }
        IEnumerator waitFor(float amount)
        {
            yield return new WaitForSeconds(10);
            balance += amount;
            UpdateUI();
        }

        public void ResetUI()
        {
            print("reset ui");
            andarBets = 0;
            baharBets = 0;
            oneToFiveBets = 0;
            sixToTenBets = 0;
            elevenToFifteenBets = 0;
            sixteenToTwentyFiveBets = 0;
            twentySixToThirtyBets = 0;
            thirtyOneToThirtyFiveBets = 0;
            thirtySixToFourtyBets = 0;
            fourtyAndMoreBets = 0;
            UpdateUI();
        }
        void UpdateUI()
        {
            andarTxt.text = andarBets.ToString();
            baharTxt.text = baharBets.ToString();
            oneToFiveTxt.text = oneToFiveBets.ToString();
            sixToTenTxt.text = sixToTenBets.ToString();
            eleventToFifteenTxt.text = elevenToFifteenBets.ToString();
            sixteenToTwentyFiveTxt.text = sixteenToTwentyFiveBets.ToString();
            twentySixToThirtyTxt.text = twentySixToThirtyBets.ToString();
            thirtyOneToThirtyFiveTxt.text = thirtyOneToThirtyFiveBets.ToString();
            thirtySixToFourtyBetsTxt.text = thirtySixToFourtyBets.ToString();
            fourtyAndMoreTxt.text = fourtyAndMoreBets.ToString();
            balanceTxt.text = Mathf.RoundToInt(balance).ToString();
        }


        public Image[] andarBaharGrid;
        public TMP_Text[] last8roundsWins;
        public Sprite andarSprite, baharSprite;

        public TMP_Text andarHirtoryPC;
        public TMP_Text baharHirtoryPC;
        public TMP_Text baharPridictionPC;
        public TMP_Text andarPridictionPC;
        public void UpdateDashboard(object data)
        {
            Dashboard dashboard = JsonConvert.DeserializeObject<Dashboard>(data.ToString());
            andarHirtoryPC.text = dashboard.historyPercent[0].ToString() + "%";
            baharHirtoryPC.text = dashboard.historyPercent[1].ToString() + "%";
            andarPridictionPC.text = dashboard.pridictionPercent[0].ToString() + "%";
            baharPridictionPC.text = dashboard.pridictionPercent[1].ToString() + "%";
            if(dashboard.balance!=null)
            balance =float.Parse( dashboard.balance);

            for (int i = 0; i < dashboard.previousWins.Count; i++)
            {
                if (dashboard.previousWins[i] == 0)
                {
                    andarBaharGrid[i].sprite = andarSprite;
                }
                else
                {
                    andarBaharGrid[i].sprite = baharSprite;
                }
            }

            for (int i = 0; i < dashboard.historyCards.Count; i++)
            {
                if (i >=last8roundsWins.Length) break;
                if (dashboard.historyCards[i].joker_card_no == 10)
                {
                    last8roundsWins[i].text = "J";
                }
                else if (dashboard.historyCards[i].joker_card_no == 11)
                {
                    last8roundsWins[i].text = "Q";
                }
                else if (dashboard.historyCards[i].joker_card_no == 12)
                {
                    last8roundsWins[i].text = "K";
                }
                else
                {
                    try
                    {
                        if (i < last8roundsWins.Length)
                            last8roundsWins[i].text = dashboard.historyCards[i].joker_card_no.ToString();

                    }
                    catch (Exception)
                    {

                        throw;
                    }
                }

                if (i < last8roundsWins.Length)
                    if (dashboard.historyCards[i].winSpot == (int)Spots.Andar)
                {
                    last8roundsWins[i].color = Color.blue;
                }
                else
                {
                    last8roundsWins[i].color = Color.red;
                }
            }
            UpdateUI();
        }
    }
}

public class HistoryCard
{
    public int joker_card_no;
    public int winSpot;
}

public class Dashboard
{
    public List<int> previousWins;
    public List<HistoryCard> historyCards;
    public List<int> historyPercent;
    public List<int> pridictionPercent;
    public string balance;
}
