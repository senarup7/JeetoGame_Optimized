using AndarBahar.Utility;
using System;
using System.Collections;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using UnityEngine;
using UnityEngine.UI;

namespace AndarBahar.Gameplay
{
    class AndarBahar_RoundWinnerHandlercs : MonoBehaviour
    {

        public Transform AndarImg;
        public Transform BaharImg;
        public Transform Andar;
        public Transform Bahar;
        public Transform oneToFive;
        public Transform sixToTen;
        public Transform elevenToFifteen;
        public Transform sixteenToTwentyFive;
        public Transform twentySixToThirty;
        public Transform thirtyOneToThirtyFive;
        public Transform thirtySixToFouty;
        public Transform fortyOneAndMore;
        public Button test;

        public void Start()
        {
            AndarImg.gameObject.SetActive(false);
            BaharImg.gameObject.SetActive(false);
            Andar.gameObject.SetActive(false);
            Bahar.gameObject.SetActive(false);
            oneToFive.gameObject.SetActive(false);
            sixToTen.gameObject.SetActive(false);
            elevenToFifteen.gameObject.SetActive(false);
            sixteenToTwentyFive.gameObject.SetActive(false);
            twentySixToThirty.gameObject.SetActive(false);
            thirtyOneToThirtyFive.gameObject.SetActive(false);
            thirtySixToFouty.gameObject.SetActive(false);
            fortyOneAndMore.gameObject.SetActive(false);

        }

        /// <summary>
        /// 
        /// </summary>
        /// <param name="spot">willl only consider andar Or Bahar</param>
        /// <param name="subSpot"></param>
        public void OnWin(Spots spot, int subSpot)
        {
            Transform winnerSpot = null;
            Transform winnerImg = null;
            Transform subWinnerSpot = null;
            switch (spot)
            {
                case Spots.Andar:
                    winnerSpot = Andar;
                    winnerImg = AndarImg;

                    break;
                case Spots.Bahar:
                    winnerSpot = Bahar;
                    winnerImg = BaharImg;
                    break;

            }

            if (subSpot < 5)
            {
                subWinnerSpot = oneToFive;
            }
            else if (subSpot < 10)
            {
                subWinnerSpot = sixToTen;

            }
            else if (subSpot < 15)
            {
                subWinnerSpot = elevenToFifteen;

            }
            else if (subSpot < 25)
            {
                subWinnerSpot = sixteenToTwentyFive;

            }
            else if (subSpot < 30)
            {
                subWinnerSpot = twentySixToThirty;

            }
            else if (subSpot < 35)
            {
                subWinnerSpot = twentySixToThirty;

            }
            else if (subSpot < 40)
            {
                subWinnerSpot = thirtySixToFouty;

            }
            else if (subSpot > 40)
            {
                subWinnerSpot = fortyOneAndMore;

            }
            StartCoroutine(ShowRings(winnerImg, winnerSpot, subWinnerSpot));
        }


        public Action OnWinAnimationComplete;

        float delay = .25f;
        IEnumerator ShowRings(Transform ring1, Transform ring2, Transform ring3)
        {
            int rounds = 6;
            for (int i = 0; i < rounds; i++)
            {
                ring1.gameObject.SetActive(true);
                ring2.gameObject.SetActive(true);
                ring3.gameObject.SetActive(true);
                yield return new WaitForSeconds(delay);
                ring1.gameObject.SetActive(false);
                ring2.gameObject.SetActive(false);
                ring3.gameObject.SetActive(false);
                yield return new WaitForSeconds(delay);
            }
            yield return new WaitForSeconds(delay);
            GetComponent<AndarBahar_ChipController>().DestroyChips();
            OnWinAnimationComplete?.Invoke();
        }
    }
}
