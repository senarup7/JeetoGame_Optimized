using System;
using UnityEngine;
using TMPro;
using AndarBahar.Utility;
using UnityEngine.UI;
using System.Collections;
using System.Collections.Generic;

namespace AndarBahar.Gameplay
{
    class AndarBahar_Bots : MonoBehaviour
    {
        public TMP_Text id;
        public TMP_Text balance;
        public int botNo;
        int _balance;
        public AndarBhar_AiBot_Data betSpots;
        public Button test;
        float min = 0.1f, max = 1.5f;
        public GameObject mainUnite;
        public AndarBahar_ChipController chipMovement;
        public Sprite[] avatars;
        public Image avatar;
        List<Bot> chipData;
        AndarBahar_Timer timer;
        private void Start()
        {
            chipData = betSpots.bots;
            chipMovement = mainUnite.GetComponent<AndarBahar_ChipController>();
            timer = mainUnite.GetComponent<AndarBahar_Timer>();
            winTxt.SetActive(false);
            intialPos = winTxt.transform.position;
            finalPos = avatar.gameObject.transform.position;
         
        }

        public void AddBet(int dataNo)
        {
            if (dataNo > chipData.Count-1&&dataNo>=0) return;
            StartCoroutine(CreateChip(UnityEngine.Random.Range(min, max), dataNo));
            StartCoroutine(ShakeAnimation());
        }
        IEnumerator CreateChip(float delay, int index)
        {
            yield return new WaitForSeconds(delay);
            try
            {
            Bot O = chipData[index];
            if (O == null) yield break;

            _balance -= (int)O.chip;
            UpdateBalance();
            ///  print(JsonUtility.ToJson(O));
            ///  

                if (chipMovement != null)
                    chipMovement.CreateChip(O.position, O.chip, O.spot, botNo);
            }
            catch (Exception e)
            {
                print("index " + index);
                print(e.Message);
                print(e.StackTrace);
            }
        }

        float shakeOffset = 0.25f;
        float shakeSpeed = 5f;
        IEnumerator ShakeAnimation()
        {
            var finil_Pos = new Vector3(intialPos.x + shakeOffset,
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
            //this.transform.position = intialPos;

        }

        public void UpdateData(BotsBets data)
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
            if(balance!=null)
            balance.text = _balance.ToString();
        }

        Vector3 intialPos;
        Vector3 finalPos;
        float animationTime = 1f;
        public GameObject winTxt;
        public iTween.EaseType easeType;
        public Button testBtn;
        public float speed = 10f;
        IEnumerator WinAnimation(int winamount)
        {
            winTxt.SetActive(true);
            if (winamount < 0)
            {
                winTxt.GetComponentInChildren<TMP_Text>().color = Color.red;
                winTxt.GetComponentInChildren<TMP_Text>().text = "-" + winamount.ToString();

            }
            else
            {
                winTxt.GetComponentInChildren<TMP_Text>().color = Color.green;
                winTxt.GetComponentInChildren<TMP_Text>().text = "+" + winamount.ToString();
            }
            float d = Vector2.Distance(winTxt.transform.position, finalPos);
            while (d > 0.01f)
            {
                winTxt.transform.position = Vector2.MoveTowards(winTxt.transform.position, finalPos, speed * Time.deltaTime);
                d = Vector2.Distance(winTxt.transform.position, finalPos);
                yield return new WaitForEndOfFrame();
            }
            winTxt.SetActive(false);
            //winTxt.transform.position = intialPos;
        }

    }
}
