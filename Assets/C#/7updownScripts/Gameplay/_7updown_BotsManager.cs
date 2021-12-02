using System;
using System.Collections.Generic;
using UnityEngine;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using UpDown7.Gameplay;
using UpDown7.Utility;

namespace Updown7.Gameplay
{
    class _7updown_BotsManager : MonoBehaviour
    {
        public _7updown_Bot[] bots;
        public void UpdateBotData(object data)
        {
            try
            {

                RoundData roundData = UpDown7.Utility.Utility.GetObjectOfType<RoundData>(data);
                for (int i = 0; i < bots.Length; i++)
                {
                    bots[i].UpdateData(roundData.botsBetsDetails[i]);
                }
            }
            catch (Exception e)
            {
                Debug.Log(e.Message);
            }
        }
    }
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