using Dragon.Utility;
using Shared;
using System.Collections.Generic;
using UnityEngine;

namespace Dragon.Gameplay
{


#if ENABLE_WEBSOCKET_CLIENT
#endif
    public class BetManager : MonoBehaviour
    {
        public static BetManager Instance;
        Dictionary<Spot, int> betHolder = new Dictionary<Spot, int>();
        private void Awake()
        {
            Instance = this;
        }
        private void Start()
        {
            betHolder.Add(Spot.left, 0);
            betHolder.Add(Spot.middle, 0);
            betHolder.Add(Spot.right, 0);
            Timer.Instance.onTimeUp += PostBets;
            Timer.Instance.onCountDownStart += ClearBet;
        }
        void PostBets()
        {
        }
        void ClearBet()
        {
            betHolder[Spot.left] = 0;
            betHolder[Spot.middle] = 0;
            betHolder[Spot.right] = 0;
        }
        public void AddBets(Spot betType, Chip chipType)
        {
            betHolder[betType] = GetBetAmount(chipType);
        }
        //public void WinnerBets(Spot betType)
        //{
        //    int ind = betHolder[betType];
        //    Debug.Log("khusi : " + ind);
        //    betHolder[betType] = 0;
        //    int a = betHolder[Spot.left] + betHolder[Spot.middle] + betHolder[Spot.right];

        //    if (a!=0)
        //    {

        //    }

        //}
        private int GetBetAmount(Chip chipType)
        {
            int amount = 0;
            switch (chipType)
            {
                case Chip.Chip10:
                    amount = 10;
                    break;
                case Chip.Chip50:
                    amount = 50;
                    break;
                case Chip.Chip100:
                    amount = 100;
                    break;
                case Chip.Chip500:
                    amount = 500;
                    break;
                case Chip.Chip1000:
                    amount = 1000;
                    break;
                case Chip.Chip5000:
                    amount = 5000;
                    break;
                default:
                    break;
            }
            return amount;
        }
    }
    public class Bet
    {
        public string user_Id;
    }
}