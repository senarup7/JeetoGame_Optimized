using Dragon.Utility;
using Shared;
using System;
using System.Collections;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using UnityEngine;
using UnityEngine.UI;

namespace Dragon.Gameplay
{
    class OnlinePlayerBets : MonoBehaviour
    {
        public static OnlinePlayerBets Intsance;
        public BetSpots betSpots;
        List<ChipDate> chipData;

        //public Button testBtn;
        private void Awake()
        {
            Intsance = this;
        }
        private void Start()
        {
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

            ChipController.Instance.CreateBotsChips(chip);
        }
        float min = 0.1f, max = .8f;

        public void ChipCreator(int dataNo)
        {
            StartCoroutine(CreateChip(UnityEngine.Random.Range(min, max), dataNo));

        }

        public void OnWin(object data)
        {
            if (Timer.Instance.is_a_FirstRound) return;
            RoundData win = Utility.Utility.GetObjectOfType<RoundData>(data);
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
                winCanvas.GetComponent<Text>().color = Color.red;
                winCanvas.GetComponent<Text>().text = "-" + winamount.ToString();

            }
            else
            {
                winCanvas.GetComponent<Text>().color = Color.green;
                winCanvas.GetComponent<Text>().text = "+" + winamount.ToString();
            }
            float d = Vector2.Distance(winCanvas.transform.position, finalPos);
            while (d > 0.01f)
            {
                winCanvas.transform.position = Vector2.MoveTowards(winCanvas.transform.position, finalPos, speed * Time.deltaTime);
                d = Vector2.Distance(winCanvas.transform.position, finalPos);
                yield return new WaitForEndOfFrame();
            }
            //winCanvas.SetActive(false);
            winCanvas.transform.position = intialPos;
        }

    }
}