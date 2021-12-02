using System;
using System.Collections.Generic;
using UnityEngine;
using AndarBahar.Utility;
using Newtonsoft.Json;
namespace AndarBahar.Gameplay
{
    class AndarBahar_BotsManager : MonoBehaviour
    {
        public AndarBahar_Bots[] bots;
        public AndarBahar_OnlinePlayerBot onlinePlayerBot;
        public AndarBahar_Timer timer;
        public GameObject mainUnite;
        bool isTimeUp;
        private void Start()
        {
            timer.onTimeUp += () => isTimeUp = true;
            timer.onTimerStart += () => isTimeUp = false;
        }
        public void UpdateBotData(object data)
        {
          
          

            OnDrawResulData roundData = JsonConvert.DeserializeObject<OnDrawResulData>(data.ToString());
            Debug.Log(roundData.botsBetsDetails.Count);
            for (int i = 0; i < bots.Length; i++)
            {
                bots[i].UpdateData(roundData.botsBetsDetails[i]);
            }
          
           
        }
        public void AddBotsBets(object o)
        {
            if (timer.is_a_FirstRound) return;
            CurrentRoundChipsData data = Fuction.GetObjectOfType<CurrentRoundChipsData>(o);

            //this will only emite chips from one spot thats the spot which is 
            //present at the beginning 
            foreach (var item in data.onlinePlayersBets)
            {
                onlinePlayerBot.AddBet(item);
            }

            int index = 0;
            foreach (var bot in data.botsBets)
            {
                bots[index].AddBet(bot.dataIndex);
                index++;
            }
        }
        public void OnDrawResult(object o)
        {
            DrawResultData data = JsonConvert.DeserializeObject<DrawResultData>(o.ToString());

            //this will only emite chips from one spot thats the spot which is 
            //present at the beginning 
            //foreach (var item in data.onlinePlayersBets)
            //{
            //}
            int index = 0;
            foreach (var bot in data.botsBetsDetails)
            {
                bots[index].UpdateData(bot);
                index++;
            }
        }
    }
}


public class CurrentRoundChipsData
{
    public List<int> onlinePlayersBets;
    public BotsData[] botsBets;
}

public class BotsData
{
    public int botIndex;
    public int dataIndex;
}
public class BotsBetsDetail
{
    public string name;
    public int left;
    public int middle;
    public int right;
    public double balance;
    public int win;
    public int avatarNumber;
}

public class RoundData
{
    public List<int> previousWins;
    public List<BotsBetsDetail> botsBetsDetails;
    public int RandomWinAmount;
}
public class PreviousWin
{
    public int win_no;
}


public class OnDrawResulData
{
    public List<BotsBets> botsBetsDetails;
}