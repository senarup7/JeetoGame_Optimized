using Dragon.UI;
using Dragon.Utility;
using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;
using Dragon.ServerStuff;
using Dragon.Utility;
using Shared;
using KhushbuPlugin;
using Dragon.player;

namespace Dragon.Gameplay
{
    /// <summary>
    /// this script is only usw for creating chip and move to different
    /// places this script also send request to sever the a chip is moved
    /// and also receive msg from sever that a chip moved
    /// </summary>
    public class ChipController : MonoBehaviour
    {
        public static ChipController Instance;

        [SerializeField] iTween.EaseType easeType;

        [SerializeField] Transform chipSecondLastSpot;
        [SerializeField] Transform chipLastSpot;

        /// <summary>
        /// the following types of parents is representing
        /// where the chip will be present after it 
        /// the movement
        /// </summary>
        public Transform leftParent;
        public Transform rightParent;
        public Transform middleParent;

        [SerializeField] float chipMoveTime;
        public Action<Transform, Vector3> OnUserInput;
        public Action<Vector3, Chip, int, Spot> OnServerResponse;

        public Dictionary<Spot, Transform> chipHolder = new Dictionary<Spot, Transform>();
        bool isTimeUp;
        private void Awake()
        {
            Instance = this;
        }
        private void Start()
        {
            OnUserInput += CreateChip;
            OnServerResponse += CreateChipForOtherPlayers;
            chipHolder.Add(Spot.right, rightParent);
            chipHolder.Add(Spot.left, leftParent);
            chipHolder.Add(Spot.middle, middleParent);
            Timer.Instance.onTimeUp += () => isTimeUp = true;
            Timer.Instance.onCountDownStart += () => isTimeUp = false;
        }
        public BetManager betManager;
        public BetSpots betSpots;
        void CreateChip(Transform bettingSpot, Vector3 target)
        {
            if (isTimeUp) return;
            if (!UiHandler.Instance.IsEnoughBalancePresent()) return;
            Chip chip = UiHandler.Instance.currentChip;
            Spot spot = bettingSpot.GetComponent<BettingSpot>().spotType;
            UiHandler.Instance.AddBets(spot);
            ServerRequest.instance.OnChipMove(target, chip, spot);
            betManager.AddBets(spot, UiHandler.Instance.currentChip);
            UiHandler.Instance.UpDateBets(spot, chip);
            GameObject chipInstance = ChipSpawner.Instance.Spawn(0, chip, GetChipParent(spot));
            StartCoroutine(MoveChip(chipInstance, target));          
        }
        public void AddData(Spot spot, Vector3 target)
        {
            int ind = UnityEngine.Random.Range(1,6);
            ChipDate chipData = new ChipDate
            {
                chip = UiHandler.Instance.currentChip,
                spot = spot,
                target = target,
                spawnNo = ind
            };
            GameObject chipInstance = ChipSpawner.Instance.Spawn(0, UiHandler.Instance.currentChip, this.transform);
            //StartCoroutine(MoveChip(chipInstance, target));
            betSpots.AddData(chipData);
        }
        Transform GetChipParent(Spot betType)
        {
            switch (betType)
            {
                case Spot.left: return leftParent;
                case Spot.middle: return middleParent;
                case Spot.right: return rightParent;
            }
            return null;
        }
        void CreateChipForOtherPlayers(Vector3 target, Chip chip, int spanNo, Spot spot)
        {
            GameObject chipInstance = ChipSpawner.Instance.Spawn(spanNo, chip, GetChipParent(spot));
            UiHandler.Instance.UpDateBets(spot, chip);
            StartCoroutine(MoveChip(chipInstance, target));

        }
        float chipMovetime = .25f;
        IEnumerator MoveChip(GameObject chip, Vector3 target)
        {
            iTween.MoveTo(chip, iTween.Hash("position", target, "time", chipMovetime, "easetype", easeType));
            //var rotat = new Vector3(0f, 0f, UnityEngine.Random.Range(0, 180));
            //iTween.RotateBy(gameObject, iTween.Hash("z", UnityEngine.Random.Range(0, 180), "easeType", "easeInOutBack", "loopType", "pingPong", "delay", chipMovetime));
            //iTween.RotateTo(chip, iTween.Hash("rotation", rotat, "time", chipMovetime, "easetype", easeType));
            yield return new WaitForSeconds(chipMovetime);
            UtilitySound.Instance.addchipsound();
            iTween.PunchScale(chip, iTween.Hash("x", .3, "y", 0.3f, "default", .1));
        }
        public void OnOtherPlayerMove(object data)
        {
            if (isTimeUp) return;
            OnChipMove chip = Utility.Utility.GetObjectOfType<OnChipMove>(data);
            CreateChipForOtherPlayers(chip.position, chip.chip, 0, chip.spot);
        }
        public Button test;
        float time = .5f;
        float moveTime = 1f;

        public void TakeChipsBack(Spot winner)
        {
            StartCoroutine(DestroyChips(winner));
        }
        IEnumerator DestroyChips(Spot winnerSpot)
        {
            foreach (var item in chipHolder)
            {
                if (item.Key == winnerSpot) continue;
                foreach (Transform child in item.Value)
                {
                    StartCoroutine(MoveChips(child, chipSecondLastSpot));
                }
            }
            yield return new WaitForSeconds(1);
            foreach (Transform child in chipHolder[winnerSpot])
            {
                StartCoroutine(MoveChips(child, chipLastSpot));
            }
            UiHandler.Instance.ResetUi();

        }
        public float speed = 1f;
        public float waitTime = 0.85f;
        IEnumerator MoveChips(Transform chip, Transform destinatio)
        {
            iTween.MoveTo(chip.gameObject, iTween.Hash("position", destinatio.position, "time", chipMoveTime, "easetype", easeType));
            yield return new WaitForSeconds(waitTime);
            Destroy(chip.gameObject);
        }
        public void CreateBotsChips(ChipDate chipData)
        {
            if (isTimeUp) return;

            var chip = chipData.chip;
            var spot = chipData.spot;
            var spanNo = chipData.spawnNo;
            var target = chipData.target;
            UiHandler.Instance.UpDateBets(spot, chip);
            GameObject chipInstance = ChipSpawner.Instance.Spawn(spanNo, chip, GetChipParent(spot));
            StartCoroutine(MoveChip(chipInstance, target));

        }
        public void CreatePlayerChip(ChipDate chipData)
        {
            if (isTimeUp) return;
            if (!UiHandler.Instance.IsEnoughBalancePresent()) return;
            print("create chip");
            Chip chip = chipData.chip;
            Spot spot = chipData.spot;
            UiHandler.Instance.AddBets(spot);
            ServerRequest.instance.OnChipMove(chipData.target, chip, spot);
            betManager.AddBets(spot, UiHandler.Instance.currentChip);
            MainPlayer.Instance.totalBet += (int)UiHandler.Instance.currentChip;
            //UiHandler.Instance.BetCoinTxt.text = MainPlayer.Instance.totalBet.ToString();
           UiHandler.Instance.UpDateBets(spot, chip);
            GameObject chipInstance = ChipSpawner.Instance.Spawn(0, chip, GetChipParent(spot));
            StartCoroutine(MoveChip(chipInstance, chipData.target));
        }
    }

}
