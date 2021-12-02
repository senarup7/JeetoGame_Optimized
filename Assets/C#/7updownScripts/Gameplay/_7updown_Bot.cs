using Updown7.Gameplay;
using System;
using System.Collections;
using System.Collections.Generic;
using Shared;
using System.Linq;
using TMPro;
using UnityEngine;
using UnityEngine.UI;
namespace UpDown7.Gameplay
{
    class _7updown_Bot : MonoBehaviour, IBot
    {
        public SpriteRenderer avatar;
        public TMP_Text balance;
        public TMP_Text id;
        public int botNo;
        int _balance;
        public BetSpots betSpots;
        public Button test;
        float min = 0.1f, max = 1.5f;
        public GameObject mainUnite;
        IChipMovement chipMovement;
        public Sprite[] avatars;
        List<ChipDate> chipData;
        _7updown_Timer timer;
        private void Start()
        {
            chipData = betSpots.chipDetails;
            chipMovement = mainUnite.GetComponent<IChipMovement>();
            timer = mainUnite.GetComponent<_7updown_Timer>();
            winCanvas.SetActive(false);
            intialPos = winCanvas.transform.position;
            finalPos = avatar.gameObject.transform.position;
        }

        IEnumerator CreateChip(float delay, int index)
        {
            yield return new WaitForSeconds(delay);
            ChipDate O = chipData[index];
            ChipDate chip = new ChipDate
            {
                spawnNo = botNo,
                chip = O.chip,
                spot = O.spot,
                target = O.target,
            };
            _balance -= (int)O.chip;
            UpdateBalance();
            chipMovement.CreateBotsChips(chip);
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

        }

        public void UpdateData(BotsBetsDetail data)
        {
            id.text = data.name;
            _balance = Math.Abs((int)data.balance);
            avatar.sprite = avatars[data.avatarNumber];
            UpdateBalance();
            if (timer.is_a_FirstRound) return;

            if (data.win != 0)
            {
                StartCoroutine(WinAnimation(data.win));
            }


        }
        public void UpdateBalance()
        {
            balance.text = _balance.ToString();
        }

        Vector3 intialPos;
        Vector3 finalPos;
        float animationTime = 1f;
        public GameObject winCanvas;
        public iTween.EaseType easeType;
        public float speed = 10f;
        IEnumerator WinAnimation(int winamount)
        {
            winCanvas.SetActive(true);
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
