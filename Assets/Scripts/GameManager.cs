using System;
using System.Collections;
using System.Collections.Generic;
using TMPro;
using UnityEngine;
using UnityEngine.UI;
using static UnityEngine.Rendering.VirtualTexturing.Debugging;
using Random = UnityEngine.Random;

public class GameManager : MonoBehaviour
{

    [SerializeField]
    Sprite[] symbols;

    public GameObject slotOne, slotTwo, slotThree, handleButton, handle;

    public TMP_Text creditsText, betText, message, additionalInfo;

    int credits, bet;
    bool gameStarted = false;

    void Start()
    {
        credits = 0;
        bet = 0;
        gameStarted = false;

        SetAlpha(slotOne, 0);
        SetAlpha(slotTwo, 0);
        SetAlpha(slotThree, 0);
    }

    void SetAlpha(GameObject slot, float alpha)
    {
        var image = slot.GetComponent<Image>();
        var color = image.color;
        color.a = alpha;
        image.color = color;
    }

    // Update is called once per frame
    void Update()
    {
        if (gameStarted)
        {
            GivePrize();

            gameStarted = false;

            // Get the Animator component of the handle game object
            Animator handleAnimator = handle.GetComponent<Animator>();

            // Set a trigger parameter to stop the animation
            handleAnimator.SetTrigger("Stop");

            handleButton.SetActive(true);
        }
    }

    public void AddCredits(int number)
    {
        credits += number;
        ChangeCredits();
    }

    public void ChangeCredits()
    {
        creditsText.text = credits.ToString();
    }

    public void BetCredits(int number)
    {
        if (credits >= number)
        {
            bet = number;
            ChangeBet();
        }
    }

    public void ChangeBet()
    {
        betText.text = bet.ToString();
    }

    public void StartGame()
    {

        if (bet > 0) {

            handleButton.SetActive(false);

            //play animation

            // Get the Animator component of the handle game object
            Animator handleAnimator = handle.GetComponent<Animator>();

            // Play the animation
            handleAnimator.SetTrigger("Start");

            StartCoroutine(SpinSlotMachine());
        }
    }

    IEnumerator SpinSlotMachine()
    {
        // Wait for 1 second before starting to spin
        yield return new WaitForSeconds(1f);

        float spinDuration = 3f; // Duration for spinning the slots
        float elapsedTime = 0f;
        float symbolChangeInterval = 0.2f; // Delay between each symbol change

        while (elapsedTime < spinDuration)
        {
            // Randomize symbols for each slot
            RandomizeSymbols(symbols);

            // Increment elapsed time
            elapsedTime += symbolChangeInterval;

            yield return new WaitForSeconds(symbolChangeInterval);
        }

        // Ensure the slots stop on random symbols after spin duration
        RandomizeSymbols(symbols);

        // Indicate that the game has started
        gameStarted = true;
    }


    void RandomizeSymbols(Sprite[] symbols)
    {
        SetAlpha(slotOne, 1);
        SetAlpha(slotTwo, 1);
        SetAlpha(slotThree, 1);

        int randomIndex = Random.Range(0, symbols.Length);
        var image = slotOne.GetComponent<Image>();
        image.sprite = symbols[randomIndex];

        randomIndex = Random.Range(0, symbols.Length);
        image = slotTwo.GetComponent<Image>();
        image.sprite = symbols[randomIndex];

        randomIndex = Random.Range(0, symbols.Length);
        image = slotThree.GetComponent<Image>();
        image.sprite = symbols[randomIndex];

    }

    public void GivePrize()
    {
        int payout = 0;

        if (IsThreeOfAKind())
        {
            // Three of a kind combination, high payout
            payout = bet*3;
            message.text = "Three of a kind!";
            message.color = new Color(253f / 255f, 208f / 255f, 65f / 255f);
            additionalInfo.text = "+ bet x3";
            additionalInfo.color = new Color(253f / 255f, 208f / 255f, 65f / 255f);
        }
        else if (IsTwoOfAKind())
        {
            // Two of a kind combination, medium payout
            payout = bet*2;
            message.text = "Two of a kind!";
            message.color = new Color(253f / 255f, 208f / 255f, 65f / 255f);
            additionalInfo.text = "+ bet x2";
            additionalInfo.color = new Color(253f / 255f, 208f / 255f, 65f / 255f);
        }
        else
        {
            credits = credits - bet;
            message.text = "You lost!";
            message.color = new Color(228f / 255f, 59f / 255f, 0f);
            additionalInfo.text = "no prize";
            additionalInfo.color = new Color(228f / 255f, 59f / 255f, 0f);
        }
        
        //add more combinations

        credits += payout;
        ChangeCredits();

        if (credits < bet)
        {
            bet = 0;
            ChangeBet();
        }
    }

    bool IsThreeOfAKind()
    {
        //same symbol in 3 slots
        var image1 = slotOne.GetComponent<Image>();
        var image2 = slotTwo.GetComponent<Image>();
        var image3 = slotThree.GetComponent<Image>();

        return image1.sprite == image2.sprite && image2.sprite == image3.sprite;
    }

    bool IsTwoOfAKind()
    {
        //same symbol in two slots
        var image1 = slotOne.GetComponent<Image>();
        var image2 = slotTwo.GetComponent<Image>();
        var image3 = slotThree.GetComponent<Image>();

        return (image1.sprite == image2.sprite && image1.sprite != image3.sprite) ||
               (image1.sprite == image3.sprite && image1.sprite != image2.sprite) ||
               (image2.sprite == image3.sprite && image2.sprite != image1.sprite);
    }

}
