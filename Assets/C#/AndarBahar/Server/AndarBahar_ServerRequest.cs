using UnityEngine;
using SocketIO;
using Shared;
using AndarBahar.Utility;
using System.Collections;

namespace AndarBahar.Server
{

    public class AndarBahar_ServerRequest : MonoBehaviour
    {
       public  SocketIOComponent socket;
        public static AndarBahar_ServerRequest instance;
        IEnumerator Start()
        {
            instance = this;
            socket = SocketIOComponent.instance;
            yield return new WaitForSeconds(2f);
            JoinGame();
        }
        void JoinGame()
        {
            Debug.Log("Join");
            Player player = new Player()
            {
                balance = LocalPlayer.balance,
                playerId = UserDetail.UserId.ToString(),
                profilePic = LocalPlayer.profilePic,
                gameId = "3"
            };
            socket.Emit(Events.RegisterPlayer, new JSONObject(JsonUtility.ToJson(player)));
        }
        public void onleaveRoom()
        {
            socket.Emit(Events.onleaveRoom);
        }
        public void OnChipMove(Vector3 position, Chip chip, Spots spot)
        {
            OnChipMove Obj = new OnChipMove()
            {
                position = position,
                playerId = UserDetail.UserId.ToString(),
                chip = chip,
                spot = spot
            };
            socket.Emit(Events.OnChipMove, new JSONObject(JsonUtility.ToJson(Obj)));
        }
    }
}
