using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.SceneManagement;
using UnityEngine.UI;

namespace Updown7.UI
{
    public class _7updown_LeftSideMenu : MonoBehaviour
    {
        public Vector3 start;
        public Vector3 target;
        public float animationTime;
        public float offset;
        public iTween.EaseType easeType;
        public State currentState;
        public Music currentMusicState;
        public Sound currentSoundState;
        public Button LobbyBtn;
        public Button musicBtn;
        public Button soundBtn;
        public Button rulesBtn;
        public Button rulesBackBtn;
        public Sprite[] musicIcons;
        public Sprite[] soundIcons;
        public Image musicIcon;
        public Image soundIcon;
        public GameObject rulesPanel;
        public Screen screen;
        Vector3 initalPosition;
        void Start()
        {
            print(GetComponent<Image>().sprite.rect.size.x);
            float sceen_width = (float)UnityEngine.Screen.width;
            float side_Panel_width = GetComponent<Image>().sprite.rect.width;
            float xOffset = sceen_width + side_Panel_width;
            initalPosition = new Vector3(xOffset, transform.position.y );
            var rect = GetComponent<RectTransform>();
            var pos = this.gameObject.transform.position;
            start = pos;
            offset = rect.rect.width;
            target = new Vector3(sceen_width - side_Panel_width, transform.position.y);
            currentState = State.close;
            currentMusicState = Music.musicOn;
            currentState = State.close;
            this.gameObject.SetActive(false);
            currentSoundState = Sound.soundOff;
            rulesPanel.SetActive(false);
            //transform.position = initalPosition;
            //GetComponent<RectTransform>().position = initalPosition;
            AddListner();
        }
        public void AddListner()
        {
            soundBtn.onClick.AddListener(() =>
            {
                bool isOn = Sound.soundOff == currentSoundState ? false : true;
                Debug.Log("sound on "+isOn);
                if (isOn)
                {
                    soundIcon.sprite = soundIcons[1];
                    currentSoundState = Sound.soundOff;
                    SoundManager.instance.canPlaySound = false;
                }
                else
                {
                    soundIcon.sprite = soundIcons[0];
                    currentSoundState = Sound.soundOn;
                    SoundManager.instance.canPlaySound = true;
                }
            });
            musicBtn.onClick.AddListener(() =>
              {
                  bool isOn = Music.musicOff == currentMusicState ? false : true;
                  if (isOn)
                  {
                      musicIcon.sprite = musicIcons[1];
                      currentMusicState = Music.musicOff;
                      SoundManager.instance.StopBackgroundMusic();
                  }
                  else
                  {
                      SoundManager.instance.PlayBackgroundMusic();
                      musicIcon.sprite = musicIcons[0];
                      currentMusicState = Music.musicOn;
                  }
              });
            
            rulesBtn.onClick.AddListener(() =>
              {
                  rulesPanel.SetActive(true);
              }); 
            rulesBackBtn.onClick.AddListener(() =>
              {
                  rulesPanel.SetActive(false);
              });
            LobbyBtn.onClick.AddListener(() =>
            {
                ServerStuff.ServerRequest.instance.onleaveRoom();
                SceneManager.LoadScene(1);
            });

        }
        public void ShowPopup()
        {
            currentState = State.close == currentState ? State.open : State.close;
                PlayAnimation();
        }
        void PlayAnimation()
        {
            switch (currentState)
            {
                case State.open:
                    this.gameObject.SetActive(true);
                    //iTween.MoveTo(this.gameObject, iTween.Hash("position", target, "time", animationTime, "easetype", easeType));
                    break;
                case State.close:
                    this.gameObject.SetActive(false);
                    //iTween.MoveTo(this.gameObject, iTween.Hash("position", start, "time", animationTime, "easetype", easeType));
                    break;
            }

        }
        //IEnumerator PlayAnimation()
        //{
        //    switch (currentState)
        //    {
        //        case State.open:
        //            this.gameObject.SetActive(true);
        //            //iTween.MoveTo(this.gameObject, iTween.Hash("position", target, "time", animationTime, "easetype", easeType));
        //            break;
        //        case State.close:
        //            this.gameObject.SetActive(false);
        //            //iTween.MoveTo(this.gameObject, iTween.Hash("position", start, "time", animationTime, "easetype", easeType));
        //            break;
        //    }
        //    yield return new WaitForSeconds(animationTime);

        //}

    }
    public enum State
    {
        open,
        close
    }
    public enum Sound
    {
        soundOff,
        soundOn
    }
    public enum Music
    {
        musicOn,
        musicOff
    }
}
