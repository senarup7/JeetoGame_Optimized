using System;
using UnityEngine;

/// <summary>
/// This class initate the game get the require data to start the game
/// this class will call from ServerRequest.cs
/// </summary>

namespace Dragon.Gameplay
{

    public class StartGame : MonoBehaviour
    {
        public event Action OnGameStart;

        public void RegisterDataPlayers(object o)
        {
            PlayerManager.Instance.JoinGame(o);
            OnGameStart?.Invoke();
        }
    }
}