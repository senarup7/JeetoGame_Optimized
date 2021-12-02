using UnityEngine.UI;
using UnityEngine;
using AndarBahar.Utility;
using Shared;
using System.Collections;
using AndarBahar.UI;
using System;
using AndarBahar.Server;
using System.Collections.Generic;

namespace AndarBahar.Gameplay
{
    class AndarBahar_ChipController : MonoBehaviour
    {

        public AndarBahar_UiHandler uiHandler;
        public AndarBahar_Timer timer;
        public AndarBahar_Spawner chipSpawner;
        public Action<Transform, Vector3> OnUserInput;
        public AndarBhar_AiBot_Data AndarBhar_Ai;
        bool isTimeUp;
        private void Start()
        {
            OnUserInput += AddUserBet;
            timer.onTimerStart += () => isTimeUp = false;
            timer.onTimeUp += () => isTimeUp = true;

        }
        public Transform ChipsHolder;

        void AddUserBet(Transform bettingSpot, Vector3 target)
        {
            if (isTimeUp) return;
            if (!uiHandler.IsEnoughBalanceAvailable()) return;
            uiHandler.AddBets();
            Chip chip = uiHandler.currentChip;
            Spots spot = bettingSpot.GetComponent<Utility.Spot>().spot;
            AndarBahar_ServerRequest.instance.OnChipMove(target, chip, spot);
            CreateChip(target, chip, spot, 0);
        }

        public void CreateChip(Vector3 target, Chip chip, Spots spot, int spawnNo)
        {
            uiHandler.UpdateBets(spot, chip);

            GameObject chipInstance = chipSpawner.Spawn(spawnNo, chip, ChipsHolder);
            chips.Add(chipInstance.transform);
            //Bot bot = new Bot()
            //{
            //    spot = spot,
            //    position = target,
            //    chip = chip
            //};
            //AndarBhar_Ai.AddData(bot);
            StartCoroutine(MoveChip(chipInstance, target));
        }
        public void CreateChip(object o)
        {
        }

        public float chipMovetime, speed;
        public iTween.EaseType easeType;


        IEnumerator MoveChip(GameObject chip, Vector3 target)
        {
            float distance = Vector3.Distance(chip.transform.position, target);
            //var rotat = new Vector3(0f, 0f, UnityEngine.Random.Range(0, 180));
            //iTween.RotateTo(chip, iTween.Hash("rotation", rotat, "time", chipMovetime, "easetype", easeType));

            while (distance > .1f)
            {
                chip.transform.position = Vector3.MoveTowards(chip.transform.position, target, speed);
                distance = Vector3.Distance(chip.transform.position, target);
                yield return new WaitForEndOfFrame();
            }
            iTween.PunchScale(chip, iTween.Hash("x", .3, "y", 0.3f, "default", .1));
            SoundManager.instance.PlayClip("addchip");

        }

        public void DestroyChips()
        {
             if (chips.Count > 0)
                MoveChipAndDestroy();

        }

        List<Transform> chips = new List<Transform>();
        public Transform chipDistroyerSpot;
        public float chipMoveTime = 1f;
        void MoveChipAndDestroy()
        {
            foreach (var chip in chips)
            {
                StartCoroutine(MoveChip(chip.gameObject));
            }
            chips.Clear();
        }
        IEnumerator MoveChip(GameObject chip)
        {
            var target = chipDistroyerSpot.position;

            float distance = Vector3.Distance(chip.transform.position, target);
            while (distance > .1f)
            {
                chip.transform.position = Vector3.MoveTowards(chip.transform.position, target, speed/2);
                distance = Vector3.Distance(chip.transform.position, target);
                yield return new WaitForEndOfFrame();
            }
            Destroy(chip);

        }


    }
}
