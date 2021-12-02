using Dragon.Utility;
using KhushbuPlugin;
using Shared;
using System;
using System.Collections;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using UnityEngine;
using UnityEngine.UI;

namespace Dragon.Gameplay
{

    class RoundWinningHandler : MonoBehaviour
    {
        public static RoundWinningHandler Instance;
        [SerializeField] GameObject leftRing;
        [SerializeField] GameObject middleRing;
        [SerializeField] GameObject rightRing;
        public Sprite[] Imgs;
        public Image[] previousWins;
        public Action<object> onWin;
        public Cards[] Card;
        public Image Card1;
        public Image Card2;
        public Sprite BackCard;
        //public string CardName[4]=new {"","","",""};
        bool isTimeUp;
        public Sprite[] TigerWinnerAnimation;
        public Sprite[] DragonWinnerAnimation;
        public Image TigerWinner;
        public Image DragonWinner;
        public Sprite[] TieWinnerAnimation;
        public Image TieWinner;

        public Image TigerWinner1;
        public Image DragonWinner1;
       

        private void Awake()
        {
            Instance = this;
        }
        private void Start()
        {
            Timer.Instance.onTimeUp += () => isTimeUp = true;
            Timer.Instance.onCountDownStart += () => isTimeUp = false;

            onWin += OnWin;
            //leftDice.SetActive(false);
            //rightDice.SetActive(false);       
        }

        public void SetWinNumbers(object o)
        {
            InitialData winData = Utility.Utility.GetObjectOfType<InitialData>(o);

            for (int i = 0; i < winData.previousWins.Count; i++)
            {
                int num = winData.previousWins[i];
                if (num == 0)
                {
                    previousWins[i].sprite = Imgs[0];//dragon
                }
                else if (num == 1)
                {
                    previousWins[i].sprite = Imgs[1];//tie
                }
                else
                {
                    previousWins[i].sprite = Imgs[2];//tiger
                }
                //previousWins[i].gameObject.transform.GetChild(0).GetComponent<Text>().text = totalDiceNo.ToString();

            }
        }
        public void OnWin(object o)
        {
            if (Timer.Instance.is_a_FirstRound) return;
            DiceWinNos winData = Utility.Utility.GetObjectOfType<DiceWinNos>(o);
            int[] turnArray = { 0, 1, 2, 3, 0, 1, 2, 3, 0, 1, 2, 3 };
            System.Random rands = new System.Random();
            List<int> rand = turnArray.OrderBy(c => rands.Next()).Select(c => c).ToList();
            int No1 = winData.winNo[0] - 1;
            int No2 = winData.winNo[1] - 1;
            StartCoroutine(cardOpen(No1, No2, rand[0], rand[1]));

            for (int i = 0; i < winData.previousWins.Count; i++)
            {
                int num = winData.previousWins[i];
                if (num == 0)
                {
                    previousWins[i].sprite = Imgs[0];//dragon
                }
                else if (num == 1)
                {
                    previousWins[i].sprite = Imgs[1];//tie
                }
                else
                {
                    previousWins[i].sprite = Imgs[2];//tiger
                }                
            }
            if (winData.winningSpot == 0)//aniamtion add khusi
            {
                StartCoroutine(ShowWinningRing(leftRing, Spot.left, o));//dragon
            }
            else if (winData.winningSpot == 1)
            {
                StartCoroutine(ShowWinningRing(middleRing, Spot.middle, o));
            }
            else
            {
                StartCoroutine(ShowWinningRing(rightRing, Spot.right, o));
            }
        }
        IEnumerator cardOpen(int No1,int No2, int rand0, int rand1)
        {
            Card1.sprite = Card[rand0].card[No1];
            yield return new WaitForSeconds(0.5f);
            Card2.sprite = Card[rand1].card[No2];
        }
        IEnumerator ShowWinningRing(GameObject ring, Spot winnerSpot, object o)
        {
            yield return new WaitForSeconds(1f);
            BotsManager.Instance.UpdateBotData(o);
            OnlinePlayerBets.Intsance.OnWin(o);
            ChipController.Instance.TakeChipsBack(winnerSpot);
            yield return new WaitForSeconds(2f);
            ring.SetActive(false);           
            if (winnerSpot == Spot.left)
            {
                StartCoroutine(DragonAnimation());
            }
            else if (winnerSpot == Spot.middle)
            {
                StartCoroutine(TieAnimation());
            }
            else if (winnerSpot == Spot.right)
            {
                StartCoroutine(TigerAnimation());
            }

            //BetManager.Instance.WinnerBets(winnerSpot);

            yield return new WaitForSeconds(2f);
            UtilitySound.Instance.Cardflipsound();
            Card1.sprite = BackCard;           
            Card2.sprite = BackCard;

        }
        IEnumerator DragonAnimation()
        {
            DragonWinner1.gameObject.SetActive(false);
            UtilitySound.Instance.DragonRoarsound();
            DragonWinner.gameObject.SetActive(true);
            foreach (var item in DragonWinnerAnimation)
            {
                DragonWinner.sprite = item;
                yield return new WaitForSeconds(0.03f);
            }
            DragonWinner.gameObject.SetActive(false);
            DragonWinner1.gameObject.SetActive(true);
        }
        IEnumerator TieAnimation()
        {
            UtilitySound.Instance.DragonRoarsound();
            TieWinner.gameObject.SetActive(true);
            foreach (var item in TieWinnerAnimation)
            {
                TieWinner.sprite = item;
                yield return new WaitForSeconds(0.1f);
            }
            TieWinner.gameObject.SetActive(false);
        }
        IEnumerator TigerAnimation()
        {
            UtilitySound.Instance.TigerRoarsound();
            TigerWinner1.gameObject.SetActive(false);
            TigerWinner.gameObject.SetActive(true);
            foreach (var item in TigerWinnerAnimation)
            {
                TigerWinner.sprite = item;
                yield return new WaitForSeconds(0.03f);
            }
            TigerWinner.gameObject.SetActive(false);
            TigerWinner1.gameObject.SetActive(true);
        }
    }
    [System.Serializable]
    public class Cards
    {
        public Sprite[] card;
    }
    public class DiceWinNos
    {
        public List<int> winNo;
        public List<int> wins;
        public List<int> previousWins;
        public int winningSpot;
    }
}