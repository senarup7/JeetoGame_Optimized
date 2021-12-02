using Shared;
using System;
using System.Collections;
using System.Collections.Generic;
using System.Linq;
using UnityEngine;
using UnityEngine.UI;


namespace Dragon.Gameplay
{
    class Bot : MonoBehaviour
    {
        public Image avatar;
        public Transform Finaltra;
        public Text balance;
        public Text id;
        public int botNo;
        public int _balance;
        public BetSpots betSpots;
        float min = 0.1f, max = 1.5f;
        public GameObject mainUnite;
        public Sprite[] avatars;
        List<ChipDate> chipData;
        Vector3 intialPos;
        Vector3 finalPos;
        float animationTime = 1f;
        public GameObject winCanvas;
        public iTween.EaseType easeType;
        public float speed = 1f;
        Vector3 pos;
        private void Start()
        {
            chipData = betSpots.chipDetails;
            winCanvas.SetActive(false);
            intialPos = winCanvas.transform.position;
            finalPos = avatar.transform.position;
            pos = this.transform.position;
        }
      

        IEnumerator CreateChip(float delay, int index)
        {
            yield return new WaitForSeconds(delay);
            ChipDate O = chipData[index];
            ChipDate chip = new ChipDate
            {
                spawnNo = O.spawnNo,
                chip = O.chip,
                spot = O.spot,
                target = O.target,
            };
            _balance -= (int)O.chip;
            ChipController.Instance.CreateBotsChips(chip);
        }

        public void ChipCreator(int dataNo)
        {
            StartCoroutine(CreateChip(UnityEngine.Random.Range(min, max), dataNo));
            StartCoroutine(ShakeAnimation());
        }
        float shakeOffset = 0.25f;
        float shakeSpeed = 5f;        
        IEnumerator ShakeAnimation()
        {
            var finil_Pos = new Vector3(this.transform.position.x + shakeOffset,
                 this.transform.position.y,
                 this.transform.position.z);
            float d = Vector2.Distance(this.transform.position, finil_Pos);
            while (d > 0.01f)
            {
                this.transform.position = Vector2.MoveTowards(this.transform.position, finil_Pos, shakeSpeed * Time.deltaTime);
                d = Vector2.Distance(this.transform.position, finil_Pos);
                yield return new WaitForEndOfFrame();
            }
            var new_finil_Pos = new Vector3(this.transform.position.x - shakeOffset,
                 this.transform.position.y,
                 this.transform.position.z);


            d = Vector2.Distance(this.transform.position, new_finil_Pos);
            while (d > 0.01f)
            {
                this.transform.position = Vector2.MoveTowards(this.transform.position, new_finil_Pos, shakeSpeed * Time.deltaTime);
                d = Vector2.Distance(this.transform.position, new_finil_Pos);
                yield return new WaitForEndOfFrame();
            }
            this.transform.position = pos;
        }

        public void UpdateData(BotsBetsDetail data)
        {
            id.text = data.name;
            _balance = Math.Abs((int)data.balance);
            avatar.sprite = avatars[data.avatarNumber];
            UpdateBalance();
            if (Timer.Instance.is_a_FirstRound) return;

            if (data.win != 0)
            {
                StartCoroutine(WinAnimation(data.win));
            }
        }
        public void UpdateBalance()
        {
            if(balance!=null)
            balance.text = _balance.ToString();
        }

       
        IEnumerator WinAnimation(int winamount)
        {
            winCanvas.SetActive(true);
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
            winCanvas.SetActive(false);
            winCanvas.transform.position = intialPos;
        }
    }
}