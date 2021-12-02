using System;
using System.Collections;
using UnityEngine;
using TMPro;
using UpDown7.Utility;
using Newtonsoft.Json;
using System.Collections.Generic;
using Updown7.UI;

namespace Updown7.Gameplay
{
    public class _7updown_Timer : MonoBehaviour, ITimer
    {

        int bettingTime = 15;
        int timeUpTimer = 10;
        int waitTimer = 3;
        public Action onTimeUp;
        public Action onCountDownStart;
        public Action startCountDown;
        public static gameState gamestate;
        [SerializeField] TMP_Text countdownTxt;
        [SerializeField] TMP_Text stateTxt;
        enum State
        {
            Betting,
            Drawing,
            Idle,

        }
        IEnumerator countDown;
        IEnumerator onTimeUpcountDown;
        IEnumerator onWaitcountDown;
        public void StartCoundown() => StartCoroutine(countDown);
        void Start()
        {
            gamestate = gameState.cannotBet;
            countDown = Countdown();
            onTimeUpcountDown = TimpUpCountdown();
            onWaitcountDown = WaitCountdown();
            onTimeUp?.Invoke();
        }

        //this will run once it connected to the server
        //it will carry the time and state of server
        IEnumerator Countdown(int time = -1)
        {

            onCountDownStart?.Invoke();
            stateTxt.text = State.Betting.ToString();
            gamestate = gameState.canBet;
            for (int i = time != -1 ? time : bettingTime; i >= 0; i--)
            {
                if (i == 3)
                {
                    startCountDown?.Invoke();
                }
                countdownTxt.text = i.ToString();
                yield return new WaitForSecondsRealtime(1f);
            }
            onTimeUp?.Invoke();

        }
        IEnumerator TimpUpCountdown(int time = -1)
        {
            gamestate = gameState.cannotBet;
            stateTxt.text = State.Drawing.ToString();
            onTimeUp?.Invoke();

            for (int i = time != -1 ? time : timeUpTimer; i >= 0; i--)
            {
                countdownTxt.text = i.ToString();
                yield return new WaitForSecondsRealtime(1f);
            }

        }
        IEnumerator WaitCountdown(int time = -1)
        {
            stateTxt.text = State.Idle.ToString();

            gamestate = gameState.wait;
            for (int i = time != -1 ? time : waitTimer; i >= 0; i--)
            {
                countdownTxt.text = i.ToString();
                yield return new WaitForSecondsRealtime(1f);
            }

        }


        public void OnTimerStart(object data)
        {
            if (is_a_FirstRound)
            {
                uiHandler.HideMessage();
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
            if (is_a_FirstRound) return;
            StopCoroutines();
            StartCoroutine(WaitCountdown());
        }

        public _7updown_UiHandler uiHandler;
        public bool is_a_FirstRound = true;
        public void OnCurrentTime(object data = null)
        {
            Debug.Log("Please wait for next round");
            is_a_FirstRound = true;
            onTimeUp();
            uiHandler.ShowMessage("please wait for next round...");
            try
            {

                InitialData cr = JsonConvert.DeserializeObject<InitialData>(data.ToString());
                uiHandler.UpDateBalance(float.Parse(cr.balance));
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