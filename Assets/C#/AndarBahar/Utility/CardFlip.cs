using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class CardFlip : MonoBehaviour
{
    Image rend;

    [SerializeField]
    public Sprite faceSprite, backSprite;
    public Action OnFlipComplete;
    private bool coroutineAllowed, facedUp;

    // Start is called before the first frame update
    void Start()
    {
        this.gameObject.SetActive(false);
        rend = GetComponent<Image>();
        rend.sprite = backSprite;
        coroutineAllowed = true;
        facedUp = false;
    }

    private void OnMouseDown()
    {
        if (coroutineAllowed)
        {
            StartCoroutine(RotateCard());
        }
    }

    public void StartFlip()
    {
        if (coroutineAllowed)
        {
            gameObject.SetActive(true);
            StartCoroutine(RotateCard());
        }
    }
    float wait = 0.02f;
    private IEnumerator RotateCard()
    {
        coroutineAllowed = false;

        if (!facedUp)
        {
            for (float i =180f; i >= 0f; i -= 10f)
            {
                transform.rotation = Quaternion.Euler(0f, i, 0f);
                if (i == 90f)
                {
                    rend.sprite = faceSprite;
                }
                yield return new WaitForSeconds(wait);
            }
        }

        else if (facedUp)
        {
            for (float i = 180f; i >= 0f; i -= 10f)
            {
                transform.rotation = Quaternion.Euler(0f, -i, 0f);
                if (i == 90f)
                {
                    rend.sprite = backSprite;
                }
                yield return new WaitForSeconds(wait);
            }
        }

        coroutineAllowed = true;

        facedUp = !facedUp;
        OnFlipComplete?.Invoke();
    }
    public void ResetRotation()
    {
        gameObject.SetActive(false);
        facedUp = false;
        transform.rotation = Quaternion.Euler(0f, 180f, 0f);
    }
}
