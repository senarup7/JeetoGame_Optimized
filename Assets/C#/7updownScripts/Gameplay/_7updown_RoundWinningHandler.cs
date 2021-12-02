using UpDown7.Utility;
using System;
using System.Collections;
using Shared;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using UnityEngine;
using TMPro;
using UnityEngine.UI;

namespace Updown7.Gameplay
{
    class _7updown_RoundWinningHandler : MonoBehaviour
    {
        [SerializeField] GameObject leftRing;
        [SerializeField] GameObject middleRing;
        [SerializeField] GameObject rightRing;

        public Sprite[] dice;
        public Image[] previousWins;
        public GameObject leftDice;
        public GameObject rightDice;
        public Action<object> onWin;
        public _7updown_Timer timer;
        private void Start()
        {
            timer = GetComponent<_7updown_Timer>();
            timer.onTimeUp += () => isTimeUp = true;
            timer.onCountDownStart += () => isTimeUp = false;

            onWin += OnWin;
            leftDice.SetActive(false);
            rightDice.SetActive(false);
            chipController = GetComponent<_7updown_ChipController>();
            botManager = GetComponent<_7updown_BotsManager>();
        }
        bool isTimeUp;
        public void SetWinNumbers(object o)
        {
            Debug.Log("here");
            InitialData winData = UpDown7.Utility.Utility.GetObjectOfType<InitialData>(o);

            for (int i = 0; i < winData.previousWins.Count; i++)
            {
                int totalDiceNo = winData.previousWins[i];
                if (totalDiceNo < 7)
                {
                    previousWins[i].color = Color.red;
                }
                else if (totalDiceNo == 7)
                {
                    previousWins[i].color = Color.blue;
                }
                else
                {
                    previousWins[i].color = Color.green;
                }
                previousWins[i].gameObject.transform.GetChild(0).GetComponent<TMP_Text>().text = totalDiceNo.ToString();

            }
        }
        public void OnWin(object o)
        {

            if (timer.is_a_FirstRound) return;
            DiceWinNos winData = UpDown7.Utility.Utility.GetObjectOfType<DiceWinNos>(o);
            int winNumber = winData.diceWinNos.Sum();
            int leftDiceNo = winData.diceWinNos[0] - 1;
            int rightDiceNo = winData.diceWinNos[1] - 1;
            for (int i = 0; i < winData.previousWins.Count; i++)
            {
                int totalDiceNo = winData.previousWins[i];
                if (totalDiceNo < 7)
                {
                    previousWins[i].color = Color.red;
                }
                else if (totalDiceNo == 7)
                {
                    previousWins[i].color = Color.blue;
                }
                else
                {
                    previousWins[i].color = Color.green;
                }
                previousWins[i].gameObject.transform.GetChild(0).GetComponent<TMP_Text>().text = totalDiceNo.ToString();

            }

            leftDice.GetComponent<SpriteRenderer>().sprite = dice[leftDiceNo];
            rightDice.GetComponent<SpriteRenderer>().sprite = dice[rightDiceNo];
        

            if (winNumber < 7)
            {
                StartCoroutine(ShowWinningRing(leftRing, Spot.left, o,winData.diceWinNos.ToArray()));
            }
            else if (winNumber == 7)
            {
                StartCoroutine(ShowWinningRing(middleRing, Spot.middle, o, winData.diceWinNos.ToArray()));
            }
            else
            {
                StartCoroutine(ShowWinningRing(rightRing, Spot.right, o, winData.diceWinNos.ToArray()));
            }

        }

        _7updown_ChipController chipController;
        _7updown_BotsManager botManager;
        public _7updown_OnlinePlayerBets onlinePlayerBets;
        public _7updown_JarAnimation jarAnimation;
        IEnumerator ShowWinningRing(GameObject ring, Spot winnerSpot, object o,int[] winNos)
        {
            jarAnimation.PlayAnimatin(winNos);
            yield return new WaitForSeconds(2.5f);
           
            ring.SetActive(true);
            yield return new WaitForSeconds(0.5f);
            ring.SetActive(false);
            yield return new WaitForSeconds(0.3f);
            ring.SetActive(true);
            yield return new WaitForSeconds(0.2f);
            ring.SetActive(false);
            yield return new WaitForSeconds(0.1f);
            ring.SetActive(true);

            botManager.UpdateBotData(o);
            onlinePlayerBets.OnWin(o);
            chipController.TakeChipsBack(winnerSpot);
            yield return new WaitForSeconds(2f);
            ring.SetActive(false);
            jarAnimation.ResetJar();
        }
    }
}
public class DiceWinNos
{
    public List<int> diceWinNos;
    public List<int> wins;
    public List<int> previousWins;
}