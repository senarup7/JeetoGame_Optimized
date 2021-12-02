using Dragon.Gameplay;
using Dragon.Utility;
using Shared;
using System;
using System.Collections;
using System.Collections.Generic;
using System.Linq;
using UnityEngine;
using UnityEngine.UI;

namespace Dragon.player
{
    public class MainPlayer : MonoBehaviour
    {
        public static MainPlayer Instance;
        float shakeOffset = 0.25f;
        float shakeSpeed = 5f;
        public Transform Finaltra;
        Vector3 intialPos;
        Vector3 finalPos;
        float animationTime = 1f;
        public GameObject winCanvas;
        public iTween.EaseType easeType;
        public float speed = 1f;
        public int totalBet = 0;
        private void Awake()
        {
            Instance = this;
        }
        private void Start()
        {
            winCanvas.SetActive(false);
            intialPos = winCanvas.transform.position;
            finalPos = Finaltra.position;
        }
        public void AddBet(int index, Chip chip, Spot spot, Vector3 target)
        {           
            StartCoroutine(CreateChip(index, chip, spot, target));
            StartCoroutine(ShakeAnimation());
        }      
        IEnumerator CreateChip(int index, Chip chip, Spot spot, Vector3 target)
        {
            yield return new WaitForSeconds(0.1f);
            //ChipDate O = chipData[index];
            ChipDate chips = new ChipDate
            {
                spawnNo = index,
                chip = chip,
                spot = spot,
                target = target
            };
            //_balance -= (int)O.chip;
            ChipController.Instance.CreatePlayerChip(chips);
        }
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
        public void winner(int win)
        {
            StartCoroutine(WinAnimation(win));
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
        IEnumerator wait()
        {
            yield return new WaitForSeconds(0.2f);
            foreach (Transform item in this.transform)
            {
                Destroy(item.gameObject);
            }
        }
    }
}