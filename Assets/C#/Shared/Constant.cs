using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using System;

namespace Shared
{
    [Serializable]
    public class ChipDate
    {
        public Vector3 target;
        public Chip chip;
        public Spot spot;
        public int spawnNo;
    }
    public enum Chip
    {
        Chip10 = 10,
        Chip50 = 50,
        Chip100 = 100,
        Chip500 = 500,
        Chip1000 = 1000,
        Chip5000 = 5000,
    }

    public enum Spot
    {
        left = 0,
        middle = 1,
        right = 2
    }
    public enum GameState
    {
        canBet,
        cannotBet,
        wait
    }
    public class Player
    {
        public string playerId;
        public string profilePic;
        public string balance;
        public string gameId;
    }

}