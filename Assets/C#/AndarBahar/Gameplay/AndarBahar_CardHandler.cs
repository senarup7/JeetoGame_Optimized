using AndarBahar.Utility;
using AndarBahar.UI;
using Newtonsoft.Json;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

namespace AndarBahar.Gameplay
{

    public class AndarBahar_CardHandler : MonoBehaviour
    {
        public Sprite[] Hears;
        public Sprite[] Spades;
        public Sprite[] Diamonds;
        public Sprite[] Clubs;
        public Sprite CardBackSide;

        public Button test;
        public Button reset;
        public List<CardFlip> cardFlips = new List<CardFlip>();
        public void Start()
        {
            winnerHandlercs = GetComponent<AndarBahar_RoundWinnerHandlercs>();
            botsManager = GetComponent<AndarBahar_BotsManager>();
            int limit = andar.childCount + bahar.childCount;
            int a = 0;
            int b = 0;
            for (int i = 0; i < limit; i++)
            {
                if (i % 2 == 0)
                {
                    var c = andar.GetChild(a).GetComponent<CardFlip>();
                    cardFlips.Add(c);
                    a++;
                }
                else
                {
                    var c = bahar.GetChild(b).GetComponent<CardFlip>();
                    cardFlips.Add(c);
                    b++;
                }
            }
        }

        int maxCards = 20;
        void ResetCards()
        {
            foreach (var item in cardFlips)
            {
                item.ResetRotation();
            }
            winnerCardObject.ResetRotation();
            cards.Clear();
        }
        public Sprite GetCard(Card card)
        {
            int cardNo = card.cardNo-1;
            switch (card.cardName)
            {
                case Cards.Hearts:
                    return Hears[cardNo];
                case Cards.Speads:
                    return Spades[cardNo];
                case Cards.Clubs:
                    return Clubs[cardNo];
                case Cards.Diamonds:
                    return Diamonds[cardNo];
                default: return null;
            }
        }
        public void SetWinnerCard(object cardDetail)
        {
            Debug.Log("winner card");
            Debug.Log(cardDetail.ToString());
            WinnerCard wc = Fuction.GetObjectOfType<WinnerCard>(cardDetail);
            var card = new Card()
            {
                cardName = (Cards)wc.Joker_Card_Type,
                cardNo = wc.Joker_Card_No,
            };
            Sprite winnerCard = GetCard(card);
            uiHandler.DisplayWinnerCardOnDashBoard(card);
            ShowWinnerCard(winnerCard);
        }
        public CardFlip winnerCardObject;
        void ShowWinnerCard(Sprite wc)
        {
            winnerCardObject.faceSprite = wc;
            winnerCardObject.StartFlip();
        }
        List<Card> cards = new List<Card>();
       

        public Transform andar, bahar;
        AndarBahar_RoundWinnerHandlercs winnerHandlercs;
        AndarBahar_BotsManager botsManager;
        public AndarBahar_UiHandler uiHandler;
        /// <summary>
        /// this will called from server response
        /// </summary>
        /// <param name="c"></param>
        /// <param name="index"></param>
        void CardAnimation(List<Card> c, object o, int index = 0)
        {
            if (c.Count <= index)
            {
                Spots s = c.Count % 2 == 0 ? Spots.Andar : Spots.Bahar;
                winnerHandlercs.OnWin(s, c.Count);
                winnerHandlercs.OnWinAnimationComplete = () => ResetCards();

                //show win txt on bots
                //and update ui dashboard
                botsManager.OnDrawResult(o);
                uiHandler.UpdateDashboard(o);
                uiHandler.ResetUI();
                return;
            }
            int i = index;
            Sprite card = GetCard(c[i]);
            var f = cardFlips[i];
            f.gameObject.SetActive(true);
            f.backSprite = CardBackSide;
            f.faceSprite = card;
            f.OnFlipComplete = () => { CardAnimation(c,o, i); };
            f.StartFlip();
            i++;

        }

        public void OnRoundDrawResult(object o)
        {
            List<Card> cardsList = new List<Card>();

            DrawResult drawResult = JsonConvert.DeserializeObject<DrawResult>(o.ToString());

            foreach (var c in drawResult.displayCard)
            {
                cardsList.Add(new Card
                {
                    cardName = (Cards)c.type,
                    cardNo = c.card
                });
            }
            CardAnimation(cardsList,o);
        }

    }
}

public class DisplayCard
{
    public int card;
    public int type;
}

public class DrawResult
{
    public List<int> winningSpot;
    public List<DisplayCard> displayCard;
}