using PathCreation;
using Shared;
using System.Collections;
using System.Collections.Generic;
using System.Linq;
using UnityEngine;
using UnityEngine.UI;

public class _7updown_JarAnimation : MonoBehaviour
{
    public GameObject Jar,AnimationJar;
    public Sprite[] frams;
    public Sprite[] number;//this contain number that will display at he end of animation
    public PathCreator pathCreator;
    public EndOfPathInstruction endOfPathInstruction;
    public Button testBtn;
    private void Start()
    {
        //initalPosition = Jar.transform.position;
        if (pathCreator != null)
        {
            // Subscribed to the pathUpdated event so that we're notified if the path changes during the game
            pathCreator.pathUpdated += OnPathChanged;

        }
        initalPosition = Jar.transform.position;
        int[] no = new int[]{ 1, 3 };
        testBtn.onClick.AddListener(()=>PlayAnimatin(no));
        ResetJar();
    }


    public Transform desitnation;
    public SpriteRenderer JarSpriteRenderer;
    public float speed = 5;
    Vector3 initalPosition;
    float distanceTravelled;
    float scaleOffset = .05f;
    //below are the dice imgs last fram
    public Sprite[] leftSprites;
    public Sprite[] middleSprites;
    public Sprite[] rightSprites;
    public Sprite[] diceNo;
    public Sprite blancJar;
    public GameObject leftDice;
    public GameObject rightDice;
    public GameObject  Dice1, Dice2;
    public void PlayAnimatin(int[] no)
    {
        StartCoroutine(ShowNo(no));
    }
    IEnumerator ShowNo(int[] no)
    {
        int winNumber = no.Sum();
        int leftDiceNo = no[0] - 1;
        int rightDiceNo = no[1] - 1;
        distanceTravelled = 0;
        float distance = Vector3.Distance(desitnation.position, Jar.transform.position);
        print(distance);
        while (.1f < distance)
        {
            distance = Vector3.Distance(desitnation.position, Jar.transform.position);
            distanceTravelled += speed * Time.deltaTime;
            Jar.transform.position = pathCreator.path.GetPointAtDistance(distanceTravelled, endOfPathInstruction);
            if (Jar.transform.localScale.x < .9f)
                Jar.transform.localScale = new Vector3(Jar.transform.localScale.x + scaleOffset, Jar.transform.localScale.y + scaleOffset);
            yield return new WaitForEndOfFrame();
        }
        JarSpriteRenderer.transform.localScale = new Vector3(1.1f, 1.1f);
        Sprite diceWinImg = leftSprites[0];
        foreach (var item in frams)
        {
            JarSpriteRenderer.sprite = item;
            yield return new WaitForSeconds(0.025f);
        }

        int winNOs = no.Sum();
        leftDice.GetComponent<SpriteRenderer>().sprite = diceNo[leftDiceNo];
        Dice2.GetComponent<SpriteRenderer>().sprite = diceNo[leftDiceNo];
        rightDice.GetComponent<SpriteRenderer>().sprite = diceNo[rightDiceNo];
        Dice1.GetComponent<SpriteRenderer>().sprite = diceNo[rightDiceNo];
      //  AnimationJar.GetComponent<SpriteRenderer>().sprite = blancJar;
        Dice1.SetActive(true);
        Dice2.SetActive(true);
        
        //JarSpriteRenderer.sprite = diceNo[winNOs - 2];
        leftDice.SetActive(true);
        rightDice.SetActive(true);
        yield return new WaitForSeconds(3);
        ResetJar();

    }
    public void ResetJar()
    {
        Jar.transform.position = initalPosition;
        JarSpriteRenderer.transform.localScale = new Vector3(.6f, .6f);
        AnimationJar.GetComponent<SpriteRenderer>().sprite = frams[0];

        leftDice. SetActive(false);
        rightDice.SetActive(false);
        Dice1.SetActive(false);
        Dice2.SetActive(false);
    }
    float NumberMapping(float num, float in_min, float in_max, float out_min, float out_max)
    {
        return (num - in_min) * (out_max - out_min) / (in_max - in_min) + out_min;
    }
    void OnPathChanged()
    {
        distanceTravelled = pathCreator.path.GetClosestDistanceAlongPath(Jar.transform.position);
    }
}
