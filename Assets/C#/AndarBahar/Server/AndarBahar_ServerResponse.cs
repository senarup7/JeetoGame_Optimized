using SocketIO;
using UnityEngine;
using AndarBahar.Gameplay;
using AndarBahar.Utility;
using AndarBahar.UI;

namespace AndarBahar.Server
{
    class AndarBahar_ServerResponse : MonoBehaviour
    {

        public GameObject mainUnit;
        public SocketIOComponent socket;
        AndarBahar_Timer timer;
        public AndarBahar_UiHandler uiHandler;
        AndarBahar_ChipController chipController;
        AndarBahar_BotsManager botManager;
        AndarBahar_CardHandler cardHandler;
        private void Start()
        {
            socket = SocketIOComponent.instance;
            timer = mainUnit.GetComponent<AndarBahar_Timer>();
            chipController = mainUnit.GetComponent<AndarBahar_ChipController>();
            botManager = mainUnit.GetComponent<AndarBahar_BotsManager>();
            cardHandler = mainUnit.GetComponent<AndarBahar_CardHandler>();
            socket.On("open", OnConnected);
            socket.On("disconnected", OnDisconnected);
            socket.On(Events.OnWait, OnWait);
            socket.On(Events.OnTimeUp, OnTimerUp);
            socket.On(Events.OnCurrentTimer, OnCurrentTimer);
            socket.On(Events.OnBotsData, OnBotsData);
            socket.On(Events.OnTimerStart, OnTimerStart);
            socket.On(Events.OnPlayerWin, OnPlayerWin);
            socket.On(Events.OnWinNo, OnWinNo);
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
        //void OnChipMove(SocketIOEvent e)
        //{
        //    chipMovement.OnOtherPlayerMove((object)e.data);
        //}

        //void OnBotsData(SocketIOEvent e)
        //{
        //    botsController.AddBotsData(e.data);
        //}
        void OnWinNo(SocketIOEvent e)
        {
            if (timer.is_a_FirstRound) return;
            //Debug.Log(e.data);
            cardHandler.OnRoundDrawResult((object)e.data);
        }

        //void OnGameStart(SocketIOEvent e)
        //{
        //    chipMovement.OnOtherPlayerMove((object)e.data);
        //}
        //void OnAddNewPlayer(SocketIOEvent e)
        //{
        //    chipMovement.OnOtherPlayerMove((object)e.data);
        //}
        void OnPlayerExit(SocketIOEvent e)
        {
        }

        void OnBotsData(SocketIOEvent e)
        {

            botManager.AddBotsBets(e.data);
        }
        void OnTimerStart(SocketIOEvent e)
        {
            timer.OnTimerStart();
            cardHandler.SetWinnerCard((object)e.data);
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
        /// <summary>
        /// the function contion the inital data that need to start the round
        /// like last 50 rounds wins
        /// bots name and balance
        /// </summary>
        /// <param name="e"></param>
        void OnCurrentTimer(SocketIOEvent e)
        {
            timer.OnCurrentTime((object)e.data);
            botManager.UpdateBotData((object)e.data);
            uiHandler.UpdateDashboard((object)e.data);
            //roundWinningHandler.SetWinNumbers((object)e.data);
        }

        private bool isConnected;

        void OnPlayerWin(SocketIOEvent e)
        {
            uiHandler.OnPlayerWin(e.data);
        }
    }
}
