using UnityEngine;
using SocketIO;
using Updown7.Gameplay;
using UpDown7.Utility;
using Updown7.UI;

namespace Updown7.ServerStuff
{
    class ServerResponse : SocketHandler
    {
        public GameObject mainUnit;

        IChipMovement chipMovement;
        ITimer timer;
        _7updown_BotsManager botManager;
        private void Start()
        {
            socket = SocketIOComponent.instance;
            //socket = GameObject.Find("SocketIOComponents").GetComponent<SocketIOComponent>();
            chipMovement = mainUnit.GetComponent<IChipMovement>();
            timer = mainUnit.GetComponent<ITimer>();
            botManager = mainUnit.GetComponent<_7updown_BotsManager>();
            botsController = mainUnit.GetComponent<_7updown_BetsHandler>();
            roundWinningHandler = mainUnit.GetComponent<_7updown_RoundWinningHandler>();
            socket.On("open", OnConnected);
            socket.On("disconnected", OnDisconnected);
            socket.On(Events.OnChipMove, OnChipMove);
            socket.On(Events.OnGameStart, OnGameStart);
            socket.On(Events.OnAddNewPlayer, OnAddNewPlayer);
            socket.On(Events.OnPlayerExit, OnPlayerExit);
            socket.On(Events.OnTimerStart, OnTimerStart);
            socket.On(Events.OnWait, OnWait);
            socket.On(Events.OnTimeUp, OnTimerUp);
            socket.On(Events.OnCurrentTimer, OnCurrentTimer);
            socket.On(Events.OnWinNo, OnWinNo);
            socket.On(Events.OnBotsData, OnBotsData);
            socket.On(Events.OnPlayerWin, OnPlayerWin);
        }
        void OnConnected(SocketIOEvent e)
        {
            print("connected");
            isConnected = true;
        }
        void OnDisconnected(SocketIOEvent e)
        {
            print("disconnected");
            isConnected = false;
        }
        void OnChipMove(SocketIOEvent e)
        {
            chipMovement.OnOtherPlayerMove((object)e.data);
        }

        _7updown_BetsHandler botsController;
        void OnBotsData(SocketIOEvent e)
        {
            botsController.AddBotsData(e.data);
        }
        _7updown_RoundWinningHandler roundWinningHandler;
        void OnWinNo(SocketIOEvent e)
        {
            Debug.Log(e.data);
            roundWinningHandler.OnWin(e.data);
        }

        void OnGameStart(SocketIOEvent e)
        {
            chipMovement.OnOtherPlayerMove((object)e.data);
        }
        void OnAddNewPlayer(SocketIOEvent e)
        {
            chipMovement.OnOtherPlayerMove((object)e.data);
        }
        void OnPlayerExit(SocketIOEvent e)
        {
            chipMovement.OnOtherPlayerMove((object)e.data);
        }


        void OnTimerStart(SocketIOEvent e)
        {
            Debug.Log("on timer start " + e.data);
            timer.OnTimerStart((object)e.data);
        }

        void OnTimerUp(SocketIOEvent e)
        {
            Debug.Log("on timeUp ");
            timer.OnTimeUp((object)e.data);
        }
        void OnWait(SocketIOEvent e)
        {
            Debug.Log("on wait ");
            timer.OnWait((object)e.data);
        }
        void OnCurrentTimer(SocketIOEvent e)
        {
            Debug.Log("currunt data " + e.data);
            botManager.UpdateBotData((object)e.data);
            timer.OnCurrentTime((object)e.data);
            roundWinningHandler.SetWinNumbers((object)e.data);
        }

        public _7updown_UiHandler uiHandler;
        void OnPlayerWin(SocketIOEvent e)
        {
            Debug.Log("win something " + e.data);
            uiHandler.OnPlayerWin(e.data);
        } 
    }
}
