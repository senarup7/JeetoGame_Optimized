using System;
using System.Collections;
using UnityEngine;
using Dragon.Utility;
using Newtonsoft.Json;
using System.Collections.Generic;
using Dragon.UI;
using UnityEngine.UI;
using KhushbuPlugin;

namespace Dragon.Gameplay
{

    public class Timer : MonoBehaviour
    {
        public static Timer Instance;
        int bettingTime = 15;
        int timeUpTimer = 10;
        int waitTimer = 3;
        public Action onTimeUp;
        public Action onCountDownStart;
        public Action startCountDown;
        public static gameState gamestate;
        [SerializeField] Text countdownTxt;

        IEnumerator countDown;
        IEnumerator onTimeUpcountDown;
        IEnumerator onWaitcountDown;
        public void StartCoundown() => StartCoroutine(countDown);
        private void Awake()
        {
            Instance = this;
        }
        void Start()
        {
            gamestate = gameState.cannotBet;
            countDown = Countdown();
            onTimeUpcountDown = TimpUpCountdown();
            onWaitcountDown = WaitCountdown();
            onTimeUp?.Invoke();
            onTimeUp();
        }

        //this will run once it connected to the server
        //it will carry the time and state of server
        IEnumerator Countdown(int time = -1)
        {

            onCountDownStart?.Invoke();
            gamestate = gameState.canBet;
            for (int i = time != -1 ? time : bettingTime; i >= 0; i--)
            {
                if (i == 1)
                {
                    startCountDown?.Invoke();
                }
                countdownTxt.text = "Start Time :" + i.ToString();
                yield return new WaitForSecondsRealtime(1f);
            }
            onTimeUp?.Invoke();

        }
        IEnumerator TimpUpCountdown(int time = -1)
        {
            gamestate = gameState.cannotBet;
            onTimeUp?.Invoke();

            for (int i = time != -1 ? time : timeUpTimer; i >= 0; i--)
            {
                countdownTxt.text = "Time Up :" + i.ToString();
                yield return new WaitForSecondsRealtime(1f);
            }

        }
        IEnumerator WaitCountdown(int time = -1)
        {
            gamestate = gameState.wait;
            for (int i = time != -1 ? time : waitTimer; i >= 0; i--)
            {
                countdownTxt.text = "Wait Time :" + i.ToString();
                yield return new WaitForSecondsRealtime(1f);
            }

        }


        public void OnTimerStart(object data)
        {
            if (is_a_FirstRound)
            {
                UiHandler.Instance.HideMessage();
            }
            is_a_FirstRound = false;

            StopCoroutines();
            StartCoroutine(Countdown());
        }

        public void OnTimeUp(object data)
        {
            if (is_a_FirstRound) return;
            StopCoroutines();
            StartCoroutine(TimpUpCountdown());
        }

        public void OnWait(object data)
        {            
            StopCoroutines();
            StartCoroutine(StartDragonAnim());           
            if (is_a_FirstRound) return;
            StopCoroutines();
            StartCoroutine(WaitCountdown());
        }
        IEnumerator StartDragonAnim()
        {
            UiHandler.Instance.HideMessage();
            UiHandler.Instance.DragonAnimpanel.gameObject.SetActive(true);
            UtilitySound.Instance.VSsoundbtn();
            foreach (var item in UiHandler.Instance.DragonAnimFrames)
            {
                UiHandler.Instance.DragonAnimpanel.sprite = item;
                yield return new WaitForSeconds(0.045f);
            }
            UiHandler.Instance.DragonAnimpanel.gameObject.SetActive(false);
        }
        public bool is_a_FirstRound = true;
        public void OnCurrentTime(object data = null)
        {
            is_a_FirstRound = true;
            onTimeUp();
            UiHandler.Instance.ShowMessage("please wait for next round...");
            try
            {
                InitialData cr = JsonConvert.DeserializeObject<InitialData>(data.ToString());
                UiHandler.Instance.UpDateBalance(float.Parse(cr.balance));
            }
            catch (Exception e)
            {
                Debug.Log(e.Message);
            }
        }

        public void StopCoroutines()
        {
            StopCoroutine(Countdown());
            StopCoroutine(TimpUpCountdown());
            StopCoroutine(WaitCountdown());
        }
    }

    [Serializable]
    public class CurrentTimer
    {
        public gameState gameState;
        public int timer;
        public List<int> lastWins;
        public int LeftBets;
        public int MiddleBets;
        public int RightBets;
    }
    public enum gameState
    {
        canBet = 0,
        cannotBet = 1,
        wait = 2,
    }

    public class InitialData
    {
        public List<int> previousWins;
        public List<BotsBetsDetail> botsBetsDetails;
        public string balance;
    }

}