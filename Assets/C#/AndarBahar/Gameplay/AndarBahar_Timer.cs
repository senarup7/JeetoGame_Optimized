using AndarBahar.Utility;
using Newtonsoft.Json;
using System;
using System.Collections;
using AndarBahar.UI;
using TMPro;
using UnityEngine;
namespace AndarBahar.Gameplay
{
    public class AndarBahar_Timer : MonoBehaviour
    {
        int bettingTime = 15;
        int timeUpTimer = 10;
        int waitTimer = 3;
        public Action onTimeUp;
        public Action onTimerStart;
        public Action onTimerEnd;
        public Action onWait;
        public Action startCountDown;
        public static gameState gamestate;
        [SerializeField] TMP_Text countdownTxt;

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

            onTimerStart?.Invoke();
            gamestate = gameState.canBet;
            for (int i = time != -1 ? time : bettingTime; i >= 0; i--)
            {

                countdownTxt.text = i.ToString();
                yield return new WaitForSecondsRealtime(1f);
            }
            onTimerEnd?.Invoke();

        }
        IEnumerator TimpUpCountdown(int time = -1)
        {
            gamestate = gameState.cannotBet;
            onTimeUp?.Invoke();

            for (int i = time != -1 ? time : timeUpTimer; i >= 0; i--)
            {
                yield return new WaitForSecondsRealtime(1f);
            }

        }
        IEnumerator WaitCountdown(int time = -1)
        {
            gamestate = gameState.wait;
            print("here");
            onWait?.Invoke();
            for (int i = time != -1 ? time : waitTimer; i >= 0; i--)
            {
                countdownTxt.text = i.ToString();
                yield return new WaitForSecondsRealtime(1f);
            }

        }
        public void OnCurrentTime(object data = null)
        {
            Debug.Log("Please wait for next round");
            is_a_FirstRound = true;
            onTimeUp();
            uiHandler.ShowMessage("please wait for next round...");
            try
            {

                InitialData cr = JsonConvert.DeserializeObject<InitialData>(data.ToString());
                //  uiHandler.UpDateBalance(float.Parse(cr.balance));
            }
            catch (Exception e)
            {
                Debug.Log(e.Message);
            }
        }

        public void OnTimerStart()
        {
            is_a_FirstRound = false;
            uiHandler.HideMessage();
            Debug.Log("Timer started");
            if (is_a_FirstRound)
            {
            }

            StopAllCoroutines();
            StartCoroutine(Countdown());
        }

        public void OnTimeUp(object data)
        {
            if (is_a_FirstRound)
            {
                return;
            }
            StopAllCoroutines();
            StartCoroutine(TimpUpCountdown());

        }

        public void OnWait(object data)
        {
            if (is_a_FirstRound) return;
            StopAllCoroutines();
            StartCoroutine(WaitCountdown());
        }
        public AndarBahar_UiHandler uiHandler;
        public bool is_a_FirstRound;
    }
}

public class InitialData
{

}