using Updown7.Gameplay;
using UpDown7.Utility;
using System;
using System.Collections;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using TMPro;
using UnityEngine;
using UnityEngine.UI;
using Shared;
namespace Updown7.Gameplay
{
    class _7updown_OnlinePlayerBets :MonoBehaviour
    {
        public BetSpots betSpots;
        IChipMovement chipMovement;
        List<ChipDate> chipData;
        public GameObject mainUnite;
        _7updown_Timer timer;
       public Button testBtn;
        private void Start()
        {
            chipMovement = mainUnite.GetComponent<IChipMovement>();
            timer = mainUnite.GetComponent<_7updown_Timer>();
            chipData = betSpots.GetData();
            intialPos = winCanvas.transform.position;
            finalPos = finalTransform.transform.position;
            //testBtn.onClick.AddListener(() => StartCoroutine(WinAnimation(-1000)));
        }
        IEnumerator CreateChip(float delay, int index)
        {
            yield return new WaitForSeconds(delay);
            ChipDate O = chipData[index];
            ChipDate chip = new ChipDate
            {
                spawnNo = 0,
                chip = O.chip,
                spot = O.spot,
                target = O.target,
            };
           
            chipMovement.CreateBotsChips(chip);
        }
        float min = 0.1f, max = .8f;

        public void ChipCreator(int dataNo)
        {
            StartCoroutine(CreateChip(UnityEngine.Random.Range(min, max), dataNo));

        }

        public void OnWin(object data)
        {
            if (timer.is_a_FirstRound) return;
            RoundData win = UpDown7.Utility.Utility.GetObjectOfType<RoundData>(data);
            Debug.Log("here we are");
            StartCoroutine(WinAnimation(win.RandomWinAmount));
        }
        Vector3 intialPos;
        Vector3 finalPos;
        public Transform finalTransform;
        float animationTime = 1f;
        public GameObject winCanvas;
        public iTween.EaseType easeType;
        public float speed = 1f;
        IEnumerator WinAnimation(int winamount)
        {
            winCanvas.SetActive(true);
            Debug.Log("here we are");
            if (winamount < 0)
            {
                winCanvas.GetComponentInChildren<TMP_Text>().color = Color.red;
                winCanvas.GetComponentInChildren<TMP_Text>().text = "-" + winamount.ToString();

            }
            else
            {
                winCanvas.GetComponentInChildren<TMP_Text>().color = Color.green;
                winCanvas.GetComponentInChildren<TMP_Text>().text = "+" + winamount.ToString();
            }
            float d = Vector2.Distance(winCanvas.transform.position, finalPos);
            while (d > 0.01f)
            {
                winCanvas.transform.position = Vector2.MoveTowards(winCanvas.transform.position, finalPos, speed * Time.deltaTime);
                d = Vector2.Distance(winCanvas.transform.position, finalPos);
                yield return new WaitForEndOfFrame();
            }
            winCanvas.SetActive(false);
            winCanvas.transform.position = intialPos;
        }

    }
}
