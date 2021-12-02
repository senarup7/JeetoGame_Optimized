using System.Collections.Generic;
using UnityEngine;
using Newtonsoft.Json;
using Dragon.Utility;
using Shared;

namespace Dragon.Gameplay
{

    public class PlayerManager : MonoBehaviour
    {
        public static PlayerManager Instance;
        public Sprite[] profiles;
        int[] occupiedSpotIndex = new int[6];
        private void Awake()
        {
            Instance = this;
        }
        public void AddPlayer(object o)
        {
            var obj = Utility.Utility.GetObjectOfType<Player>(o);
            PlayersProfile.instance.AddPlayer(obj);
        }
        public void RemovePlayer(object o)
        {
            var obj = Utility.Utility.GetObjectOfType<PlayerId>(o);
            PlayersProfile.instance.RemovePlayer(obj.playerId);
        }
        public void JoinGame(object o)
        {
            int i = 0;
            var obj = Utility.Utility.GetObjectOfType<List<Player>>(o);
            foreach (var item in obj)
            {
                PlayersProfile.instance.AddPlayer(item);
                if (i < occupiedSpotIndex.Length)
                {
                    occupiedSpotIndex[i] = 1;
                }
                else Debug.Log("player Limit exceeded");
                i++;
            }
        }
    }
    public class PlayerId
    {
        public string playerId;
    }
}