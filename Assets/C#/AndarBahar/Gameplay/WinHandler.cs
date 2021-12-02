using AndarBahar.Utility;
using System;
using UnityEngine;

namespace AndarBahar.Gameplay
{
    class WinHandler : MonoBehaviour
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

        public void OnWin(Spots spot, int subSpot)
        {
            Transform winnerSpot=null;
            Transform winnerImg=null;
            Transform subWinnerSpot=null;
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

            if (subSpot < (int)Spots.oneToFive)
            {
                subWinnerSpot = oneToFive;
            }
            else if (subSpot < (int)Spots.sixToTen)
            {
                subWinnerSpot = sixToTen;

            }else if (subSpot < (int)Spots.elevenToFifteen)
            {
                subWinnerSpot = elevenToFifteen;

            }else if (subSpot < (int)Spots.sixteenToTwentyFive)
            {
                subWinnerSpot = sixteenToTwentyFive;

            }else if (subSpot < (int)Spots.twentySixToThirty)
            {
                subWinnerSpot = twentySixToThirty;

            }else if (subSpot < (int)Spots.thirtyOneToThirtyFive)
            {
                subWinnerSpot = twentySixToThirty;

            }else if (subSpot < (int)Spots.thirtySixToFouty)
            {
                subWinnerSpot = thirtySixToFouty;

            }else if (subSpot < (int)Spots.fortyOneAndMore)
            {
                subWinnerSpot = fortyOneAndMore;

            }

        }
    }
}
