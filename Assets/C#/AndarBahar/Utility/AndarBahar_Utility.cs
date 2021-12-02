using Newtonsoft.Json;
using Shared;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using UnityEngine;

namespace AndarBahar.Utility
{
    public class Fuction
    {
        public static T GetObjectOfType<T>(object json) where T : class
        {
            T t = null;
            try
            {
                t = JsonConvert.DeserializeObject<T>(json.ToString());
                return t;
            }
            catch (Exception e)
            {
                Debug.Log(e.Message);
            }
            return t;
        }

    }
    public static class Events
    {
        internal static string OnChipMove = "OnChipMove";
        internal static string OnTest = "test";
        internal static string OnPlayerExit = "OnPlayerExit";
        internal static string OnJoinRoom = "OnJoinRoom";
        internal static string OnTimeUp = "OnTimeUp";
        internal static string OnWait = "OnWait";
        internal static string OnTimerStart = "OnTimerStart";
        internal static string OnDrawCompleted = "OnDrawCompleted";
        internal static string RegisterPlayer = "RegisterPlayer";
        internal static string OnGameStart = "OnGameStart";
        internal static string OnAddNewPlayer = "OnAddNewPlayer";
        internal static string OnCurrentTimer = "OnCurrentTimer";
        internal static string OnBetsPlaced = "OnBetsPlaced";
        internal static string OnWinNo = "OnWinNo";
        internal static string OnBotsData = "OnBotsData";
        internal static string OnPlayerWin = "OnPlayerWin";
        internal static string onleaveRoom = "onleaveRoom";
    }
    public enum Cards
    {
        Hearts = 0,
        Speads = 1,
        Clubs = 2,
        Diamonds = 3,
    }
    enum State
    {
        Betting,
        Drawing,
        Idle,

    }
    enum AndarBahar
    {
        Andar = 0,
        Bahar = 1
    }
   public enum gameState
    {
        canBet,
        cannotBet,
        wait
    }

    public class OnChipMove
    {
        public string playerId;
        public Chip chip;
        public Spots spot;
        public Vector3 position;
    }
    public enum Spots
    {
        Andar = 0,
        Bahar = 1,
        oneToFive = 2,
        sixToTen = 3,
        elevenToFifteen = 4,
        sixteenToTwentyFive = 5,
        twentySixToThirty = 6,
        thirtyOneToThirtyFive = 7,
        thirtySixToFouty = 8,
        fortyOneAndMore = 9
    }
    [Serializable]
    public class Bot
    {
        public Spots spot;
        public Chip chip;
        public Vector3 position;
    }
    [Serializable]
    public class WinnerCard
    {
        public int Joker_Card_No;
        public int Joker_Card_Type;
    }
    [Serializable]
    public class Win
    {
        public float winAmount;
    }
    public class Card
    {
        public Cards cardName;
        public int cardNo;
    }

    public class DisplayCard
    {
        public int card ;
        public int type;
    }

    public class HistoryCard
    {
        public int joker_card_no;
        public int winSpot;
    }

   

    public class DrawResultData
    {
        public List<int> winningSpot;
        public List<DisplayCard> displayCard;
        public List<int> previousWins;
        public List<HistoryCard> historyCards;
        public List<int> historyPercent;
        public List<int> pridictionPercent;
        public List<BotsBets> botsBetsDetails;
        public int RandomWinAmount;
    }

    public class BotsBets
    {
        public string name;
        public int Andar;
        public int Bahar;
        public int oneToFive;
        public int sixToTen;
        public int elevenToFifteen;
        public int sixteenToTwentyFive;
        public int twentySixToThirty;
        public int thirtyOneToThirtyFive;
        public int thirtySixToFouty;
        public int fortyOneAndMore;
        public double balance;
        public int win;
        public int avatarNumber;
    }
}
