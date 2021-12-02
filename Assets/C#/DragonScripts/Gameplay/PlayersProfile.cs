using Dragon.Utility;
using Shared;
using System.Collections.Generic;
using UnityEngine;


namespace Dragon.Gameplay
{

    class PlayersProfile : MonoBehaviour
    {
        Dictionary<string, Player> players = new Dictionary<string, Player>();
        public static PlayersProfile instance;
        public void Awake()
        {
            instance = this;
        }
        public void ClearPlayer() => players.Clear();
        public void AddPlayer(Player player)
        {
            players.Add(player.playerId, player);
        }
        public void RemovePlayer(string player)
        {
            players.Remove(player);
        }
        public Player GetPlayer(string playerID)
        {
            return players[playerID];
        }
    }
    public interface IAddPlayerProfile
    {
        void AddPlayer(Player player);

    }
}