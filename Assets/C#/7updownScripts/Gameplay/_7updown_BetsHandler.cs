using UpDown7.Utility;
using System;
using System.Collections.Generic;
using UnityEngine;

namespace Updown7.Gameplay
{
    /// <summary>
    /// this class will handle all the bots bets
    /// </summary>
    class _7updown_BetsHandler : MonoBehaviour
    {
        public _7updown_Timer timer;
        public GameObject mainUnite;
        public GameObject botsGameobject;
        public _7updown_OnlinePlayerBets  onlinePlayersData;
        bool isTimeUp;
        public IBot[] Bots;
        private void Start()
        {
            timer.onTimeUp += () => isTimeUp = true;
            timer.onCountDownStart += () => isTimeUp = false;
            Bots = botsGameobject.GetComponentsInChildren<IBot>(true);
        }

        /// <summary>
        /// iterate through each bota and apply data from scriptable object
        /// it will help bots to achive realistic behaviour
        /// this function will call from SeverResponse.cs file
        /// whenever server sends any data regarding the bots
        /// </summary>
        /// <param name="data"></param>
        public void AddBotsData(object data)
        {
            if (isTimeUp||timer.is_a_FirstRound) return;
            CurrentRoundChipsData bots = UpDown7.Utility.Utility.GetObjectOfType<CurrentRoundChipsData>(data);

            //this will only emite chips from one spot thats the spot which is 
            //present at the beginning 
            foreach (var item in bots.onlinePlayersBets)
            {

              onlinePlayersData.ChipCreator(item);
            }
            foreach (var bot in bots.botsBets)
            {
                Bots[bot.botIndex].ChipCreator(bot.dataIndex);
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