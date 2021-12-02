using Dragon.Utility;
using Dragon.ServerStuff;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;
using Dragon.ServerStuff;
using System;
using Dragon.Gameplay;
using Shared;
using Dragon.player;
using UnityEngine.SceneManagement;
using System.Linq;

namespace Dragon.UI
{
    public class UiHandler : MonoBehaviour
    {
        public static UiHandler Instance;
        public GameObject HistoryPanel;
        public GameObject PredictionTiger;
        public GameObject PredictionDragon;
        public Image[] TopBG;
        public Image[] MiddleBG;
        public Image[] slotRecordBG;
        [SerializeField] Text leftBets;
        [SerializeField] Text middleBets;
        [SerializeField] Text rightBets;
        [SerializeField] Text usernameTxt;
        [SerializeField] Text balanceTxt;
        Dictionary<Spot, Text> betUiRefrence = new Dictionary<Spot, Text>();
        public Sprite TigerSelect;
        public Sprite TigerUnSelect;
        public Sprite DragonSelect;
        public Sprite DragonUnSelect;

        public Chip currentChip;
        float balance;

        public Sprite stopBetting;
        public Sprite startBetting;
        public Image placeBets;
        public float countdownSpeed = .25f;
        public Text dragonBetsTxt;
        public Text tigerBetsTxt;
        public Text tieBetsTxt;
        private void Awake()
        {
            Instance = this;
        }
        private void Start()
        {            
            dragonBetsTxt.text = "0";
            LocalPlayer.LoadGame();
            currentChip = Chip.Chip10;
            betUiRefrence.Add(Spot.left, leftBets);
            betUiRefrence.Add(Spot.middle, middleBets);
            betUiRefrence.Add(Spot.right, rightBets);
            placeBets.gameObject.SetActive(false);
            AddListeners();
            ResetUi();
            balance = float.Parse(LocalPlayer.balance);
            UpdateUi();
            StartCoroutine(Loading());
        }

        void SetupProfile()
        {
            //int playerId=LocalPlayer.de
        }
        IEnumerator StartBetting()
        {
            ResetUi();
            dragonBetsTxt.text = "0";
            placeBets.gameObject.SetActive(true);
            placeBets.sprite = startBetting;
            yield return new WaitForSeconds(.5f);
            placeBets.gameObject.SetActive(false);
        }

        int leftTotalBets;
        int middleTotalBets;
        int rightTotalBets;
        int tigerBets;
        int dragonBets;
        int tieBetsBets;
        public void UpDateBets(Spot spot, Chip chip)
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
            SetBets(leftTotalBets, middleTotalBets, rightTotalBets);
        }
        public void AddBets(Spot spot)
        {
            balance -= (float)currentChip;

            switch (spot)
            {
                case Spot.left:dragonBets += (int)currentChip;
                    break;
                case Spot.middle:tieBetsBets += (int)currentChip;
                    break;
                case Spot.right:tigerBets += (int)currentChip;
                    break;
                default:
                    break;
            }
            UpdateUi();
        }
        public bool IsEnoughBalancePresent()
        {
            return balance - (float)currentChip > 0;
        }
        public void UpDateBalance(float amount)
        {
            Debug.Log("balance updated");
            StopLoading();
            isLoading = false;
            balance = amount;
            UpdateUi();
        }
        private void AddListeners()
        {
            Timer.Instance.onCountDownStart += () =>
            {
                if (Timer.Instance.is_a_FirstRound) return;
                StartCoroutine(StartBetting());
            };
            Timer.Instance.startCountDown += () =>
            {
                StartCoroutine(StartCountDown());
            };
        }
        public void ResetUi()
        {
            MainPlayer.Instance.totalBet = 0;
            dragonBetsTxt.text = "0";
            leftTotalBets = 0;
            middleTotalBets = 0;
            rightTotalBets = 0;
            tigerBets = 0;
            dragonBets = 0;
            tieBetsBets = 0;
            leftBets.text = string.Empty;
            middleBets.text = string.Empty;
            rightBets.text = string.Empty;
            UpdateUi();
        }
        public void SetBets(int left, int middle, int right)
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
            dragonBetsTxt.text = dragonBets.ToString();
            tieBetsTxt.text = tieBetsBets.ToString();
            tigerBetsTxt.text = tigerBets.ToString();
            usernameTxt.text = LocalPlayer.deviceId;
        }


        [SerializeField] GameObject messagePopUP;
        [SerializeField] Text msgTxt;
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
            Win win = Dragon.Utility.Utility.GetObjectOfType<Win>(o);
            if (win.winAmount != 0)
                MainPlayer.Instance.winner(Convert.ToInt32(win.winAmount));
            balance += win.winAmount;
            UpdateUi();

        }
        IEnumerator StartCountDown()
        {
            placeBets.gameObject.SetActive(true);
            placeBets.sprite = stopBetting;
            yield return new WaitForSeconds(0.5f);
            placeBets.gameObject.SetActive(false);
        }

        public Image loadingpnel;
        public Image loadingImag;
        public Sprite[] loadingFrames;
        public Sprite[] DragonAnimFrames;
        public Image DragonAnimpanel;
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
        public void ExitLobby()
        {
            ServerRequest.instance.socket.Emit(Events.onleaveRoom);
            SceneManager.LoadScene("MainScene");
        }
        public void ShowHistoryGame(object o)
        {
            HistoryPanel.SetActive(true);
            HistoryRoot Data = Dragon.Utility.Utility.GetObjectOfType<HistoryRoot>(o);
            for (int i = 0; i < Data.matrixRecord.Count; i++)
            {
                if (Data.matrixRecord[i] == 0)
                {
                    MiddleBG[i].sprite = RoundWinningHandler.Instance.Imgs[0];
                }
                else if (Data.matrixRecord[i] == 1)
                {
                    MiddleBG[i].sprite = RoundWinningHandler.Instance.Imgs[1];
                }
                else if (Data.matrixRecord[i] == 2)
                {
                    MiddleBG[i].sprite = RoundWinningHandler.Instance.Imgs[1];
                }
            }
            for (int i = 0; i < RoundWinningHandler.Instance.previousWins.Length; i++)
            {
                TopBG[i].sprite = RoundWinningHandler.Instance.previousWins[i].sprite;
            }
            int[] turnArray = { 0, 1, 2, 3, 0, 1, 2, 3, 0, 1, 2, 3 };
            for (int i = 0; i < slotRecordBG.Length; i++)
            {
                System.Random rands = new System.Random();
                List<int> rand = turnArray.OrderBy(c => rands.Next()).Select(c => c).ToList();
                int T1 = Data.slotRecord[i].T - 1;
                int D2 = Data.slotRecord[i].D - 1;
                if (Data.slotRecord[i].T > Data.slotRecord[i].D)
                {
                    slotRecordBG[i].transform.GetChild(0).GetComponent<Image>().sprite = TigerSelect;
                    slotRecordBG[i].transform.GetChild(1).GetComponent<Image>().sprite = DragonUnSelect;
                }
                else if (Data.slotRecord[i].T < Data.slotRecord[i].D)
                {
                    slotRecordBG[i].transform.GetChild(0).GetComponent<Image>().sprite = TigerUnSelect;
                    slotRecordBG[i].transform.GetChild(1).GetComponent<Image>().sprite = DragonSelect;
                }
                else
                {
                    slotRecordBG[i].transform.GetChild(0).GetComponent<Image>().sprite = TigerUnSelect;
                    slotRecordBG[i].transform.GetChild(1).GetComponent<Image>().sprite = DragonUnSelect;
                }
                slotRecordBG[i].transform.GetChild(0).GetChild(0).GetComponent<Text>().text = T1.ToString();
                slotRecordBG[i].transform.GetChild(1).GetChild(0).GetComponent<Text>().text = D2.ToString();
            }
            
        }
        public void CloseHistoryUI()
        {
            HistoryPanel.SetActive(false);
        }
    }

}

[Serializable]
public class Win
{
    public float winAmount;
}
[System.Serializable]
public class SlotRecord
{
    public int D;
    public int T;
}
[System.Serializable]
public class HistoryRoot
{
    public List<int> matrixRecord;
    public List<SlotRecord> slotRecord;
}