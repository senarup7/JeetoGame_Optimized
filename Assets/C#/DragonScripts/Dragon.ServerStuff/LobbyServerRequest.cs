using Dragon.ServerStuff;
using Dragon.Utility;
using Shared;
using SocketIO;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.SceneManagement;

public class LobbyServerRequest : SocketHandler
{  
    private void Start()
    {
        //socket = GameObject.Find("SocketIOComponents").GetComponent<SocketIOComponent>();
        socket.On("open", OnConnected);
        socket.On("disconnected", OnDisconnected);
    }
    void OnConnected(SocketIOEvent e)
    {
        print("connected");

        isConnected = true;
        Player player = new Player()
        {
            playerId = UserDetail.UserId.ToString()
        };
        socket.Emit("onEnterLobby", new JSONObject(JsonUtility.ToJson(player)));

    }
    void OnDisconnected(SocketIOEvent e)
    {
        print("disconnected");
        isConnected = false;
    }
}
