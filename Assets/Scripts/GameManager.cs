using System;
using System.Collections;
using System.Collections.Generic;
using TMPro;
using UnityEngine;
using UnityEngine.UI;
using Random = UnityEngine.Random;

public class GameManager : MonoBehaviour
{

    [SerializeField]
    Sprite[] symbols;

    public GameObject slotOne, slotTwo, slotThree, spinButton;

    public TMP_Text creditsText, betText, message, additionalInfo;

    public AudioSource buttonSound, spinningSound;

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

    void Update()
    {
        if (gameStarted)
        {
            GivePrize();

            gameStarted = false;

            spinButton.SetActive(true);
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

            spinButton.SetActive(false);

            StartCoroutine(SpinSlotMachine());
        }
    }

    IEnumerator SpinSlotMachine()
    {
        buttonSound.Play();
        spinningSound.Play();

        message.text = "SPINNING!";
        message.color = new Color(1f, 1f, 1f, 0.9f);
        additionalInfo.text = "good luck";
        additionalInfo.color = new Color(1f, 1f, 1f, 0.9f);

        float spinDuration = 2.5f; // Duration for spinning the slots
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

        var image = slotOne.GetComponent<Image>();
        image.sprite = symbols[GetSpriteIndex()];

        image = slotTwo.GetComponent<Image>();
        image.sprite = symbols[GetSpriteIndex()];

        image = slotThree.GetComponent<Image>();
        image.sprite = symbols[GetSpriteIndex()];
    }

    int GetSpriteIndex()
    {
        int probabilityRange = Random.Range(0, 100);
        int spriteIndex;

        if (probabilityRange < 5)
        {
            //5% chance to get 7
            spriteIndex = 0;
        }
        else if (probabilityRange < 10)
        {
            //5% chance to get bar
            spriteIndex = 1;
        }
        else
        {
            //90% chance to get fruits
            spriteIndex = Random.Range(2, symbols.Length);
        }

        return spriteIndex;
    }

    public void GivePrize()
    {
        int payout = 0;

        if (IsThreeOfAKind())
        {
            // Three of a kind combination
            payout = bet * 20;
            message.text = "Three of a kind!";
            message.color = new Color(3f / 255f, 152f / 255f, 0f); // Green color for win
            additionalInfo.text = "+ " + payout;
            additionalInfo.color = new Color(3f / 255f, 152f / 255f, 0f); // Green color for win
        }
        else if (IsTwoOfAKind())
        {
            // Two of a kind combination
            payout = bet * 5;
            message.text = "Two of a kind!";
            message.color = new Color(3f / 255f, 152f / 255f, 0f); // Green color for win
            additionalInfo.text = "+ " + payout;
            additionalInfo.color = new Color(3f / 255f, 152f / 255f, 0f); // Green color for win
            if (IsOneSeven())
            {
                // Two of a kind + One 7 combination
                payout = bet * 15;
                message.text = "Any two + One 7!";
                message.color = new Color(3f / 255f, 152f / 255f, 0f); // Green color for win
                additionalInfo.text = "+ " + payout;
                additionalInfo.color = new Color(3f / 255f, 152f / 255f, 0f); // Green color for win
            }
            else if (IsOneBar())
            {
                // Two of a kind + One bar combination
                payout = bet * 15;
                message.text = "Any two + One Bar!";
                message.color = new Color(3f / 255f, 152f / 255f, 0f); // Green color for win
                additionalInfo.text = "+ " + payout;
                additionalInfo.color = new Color(3f / 255f, 152f / 255f, 0f); // Green color for win
            }
        }
        else if (IsThreeSeven())
        {
            // Three 7's combination
            payout = bet * 100;
            message.text = "Three 7's!";
            message.color = new Color(3f / 255f, 152f / 255f, 0f); // Green color for win
            additionalInfo.text = "+ " + payout;
            additionalInfo.color = new Color(3f / 255f, 152f / 255f, 0f); // Green color for win
        }
        else if (IsTwoSeven())
        {
            // Two 7's combination
            payout = bet * 50;
            message.text = "Two 7's!";
            message.color = new Color(3f / 255f, 152f / 255f, 0f); // Green color for win
            additionalInfo.text = "+ " + payout;
            additionalInfo.color = new Color(3f / 255f, 152f / 255f, 0f); // Green color for win
            if (IsOneBar())
            {
                // Two seven + One bar combination
                payout = bet * 60;
                message.text = "Two 7's + One Bar!";
                message.color = new Color(3f / 255f, 152f / 255f, 0f); // Green color for win
                additionalInfo.text = "+ " + payout;
                additionalInfo.color = new Color(3f / 255f, 152f / 255f, 0f); // Green color for win
            }
        }
        else if (IsOneSeven())
        {
            // One 7 combination
            payout = bet * 10;
            message.text = "One 7!";
            message.color = new Color(3f / 255f, 152f / 255f, 0f); // Green color for win
            additionalInfo.text = "+ " + payout;
            additionalInfo.color = new Color(3f / 255f, 152f / 255f, 0f); // Green color for win
            if (IsTwoBar())
            {
                // one seven and two bars combination
                payout = bet * 35;
                message.text = "One 7 + Two Bars!";
                message.color = new Color(3f / 255f, 152f / 255f, 0f); // Green color for win
                additionalInfo.text = "+ " + payout;
                additionalInfo.color = new Color(3f / 255f, 152f / 255f, 0f); // Green color for win
            }
            else if (IsOneBar())
            {
                // one seven and one bar combination
                payout = bet * 20;
                message.text = "One 7 + One Bar!";
                message.color = new Color(3f / 255f, 152f / 255f, 0f); // Green color for win
                additionalInfo.text = "+ " + payout;
                additionalInfo.color = new Color(3f / 255f, 152f / 255f, 0f); // Green color for win
            }
        }
        else if (IsThreeBar())
        {
            // Three bars combination
            payout = bet * 50;
            message.text = "Three Bars!";
            message.color = new Color(3f / 255f, 152f / 255f, 0f); // Green color for win
            additionalInfo.text = "+ " + payout;
            additionalInfo.color = new Color(3f / 255f, 152f / 255f, 0f); // Green color for win
        }
        else if (IsTwoBar())
        {
            // Two bars combination
            payout = bet * 25;
            message.text = "Two Bars!";
            message.color = new Color(3f / 255f, 152f / 255f, 0f); // Green color for win
            additionalInfo.text = "+ " + payout;
            additionalInfo.color = new Color(3f / 255f, 152f / 255f, 0f); // Green color for win
        }
        else if (IsOneBar())
        {
            // One bar combination
            payout = bet * 10;
            message.text = "One Bar!";
            message.color = new Color(3f / 255f, 152f / 255f, 0f); // Green color for win
            additionalInfo.text = "+ " + payout;
            additionalInfo.color = new Color(3f / 255f, 152f / 255f, 0f); // Green color for win
        }
        else
        {
            // No winning combination, deduct bet
            credits -= bet;
            message.text = "You lost!";
            message.color = new Color(241f / 255f, 7f / 255f, 0f / 255f); // Red color for loss
            additionalInfo.text = "No prize";
            additionalInfo.color = new Color(241f / 255f, 7f / 255f, 0f / 255f); // Red color for loss
        }

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

        return image1.sprite.name != "slot-symbol1" && image1.sprite.name != "slot-symbol4" &&
           image1.sprite == image2.sprite && image2.sprite == image3.sprite;
    }

    bool IsTwoOfAKind()
    {
        //same symbol in two slots
        var image1 = slotOne.GetComponent<Image>();
        var image2 = slotTwo.GetComponent<Image>();
        var image3 = slotThree.GetComponent<Image>();

        return ((image1.sprite.name != "slot-symbol1" && image1.sprite.name != "slot-symbol4" &&  
            image1.sprite == image2.sprite && image1.sprite != image3.sprite) ||
           (image1.sprite.name != "slot-symbol1" && image1.sprite.name != "slot-symbol4" &&  
           image1.sprite == image3.sprite && image1.sprite != image2.sprite) ||
           (image2.sprite.name != "slot-symbol1" && image2.sprite.name != "slot-symbol4" && 
           image2.sprite == image3.sprite && image2.sprite != image1.sprite));
    }

    bool IsThreeSeven()
    {
        // Three 7's combination
        var image1 = slotOne.GetComponent<Image>();
        var image2 = slotTwo.GetComponent<Image>();
        var image3 = slotThree.GetComponent<Image>();

        return image1.sprite.name == "slot-symbol1" && image2.sprite.name == "slot-symbol1" && image3.sprite.name == "slot-symbol1";
    }

    bool IsTwoSeven()
    {
        // Two 7's combination
        var image1 = slotOne.GetComponent<Image>();
        var image2 = slotTwo.GetComponent<Image>();
        var image3 = slotThree.GetComponent<Image>();

        return (image1.sprite.name == "slot-symbol1" && image2.sprite.name == "slot-symbol1" && image3.sprite.name != "slot-symbol1") ||
               (image1.sprite.name == "slot-symbol1" && image2.sprite.name != "slot-symbol1" && image3.sprite.name == "slot-symbol1") ||
               (image1.sprite.name != "slot-symbol1" && image2.sprite.name == "slot-symbol1" && image3.sprite.name == "slot-symbol1");
    }

    bool IsOneSeven()
    {
        // One 7's combination
        var image1 = slotOne.GetComponent<Image>();
        var image2 = slotTwo.GetComponent<Image>();
        var image3 = slotThree.GetComponent<Image>();

        return (image1.sprite.name == "slot-symbol1" && image2.sprite.name != "slot-symbol1" && image3.sprite.name != "slot-symbol1") ||
               (image1.sprite.name != "slot-symbol1" && image2.sprite.name == "slot-symbol1" && image3.sprite.name != "slot-symbol1") ||
               (image1.sprite.name != "slot-symbol1" && image2.sprite.name != "slot-symbol1" && image3.sprite.name == "slot-symbol1");
    }

    bool IsThreeBar()
    {
        // Three bars combination
        var image1 = slotOne.GetComponent<Image>();
        var image2 = slotTwo.GetComponent<Image>();
        var image3 = slotThree.GetComponent<Image>();

        return image1.sprite.name == "slot-symbol4" && image2.sprite.name == "slot-symbol4" && image3.sprite.name == "slot-symbol4";
    }

    bool IsTwoBar()
    {
        // Two bars combination
        var image1 = slotOne.GetComponent<Image>();
        var image2 = slotTwo.GetComponent<Image>();
        var image3 = slotThree.GetComponent<Image>();

        return (image1.sprite.name == "slot-symbol4" && image2.sprite.name == "slot-symbol4" && image3.sprite.name != "slot-symbol4") ||
               (image1.sprite.name == "slot-symbol4" && image2.sprite.name != "slot-symbol4" && image3.sprite.name == "slot-symbol4") ||
               (image1.sprite.name != "slot-symbol4" && image2.sprite.name == "slot-symbol4" && image3.sprite.name == "slot-symbol4");
    }

    bool IsOneBar()
    {
        // One bar combination
        var image1 = slotOne.GetComponent<Image>();
        var image2 = slotTwo.GetComponent<Image>();
        var image3 = slotThree.GetComponent<Image>();

        return (image1.sprite.name == "slot-symbol4" && image2.sprite.name != "slot-symbol4" && image3.sprite.name != "slot-symbol4") ||
               (image1.sprite.name != "slot-symbol4" && image2.sprite.name == "slot-symbol4" && image3.sprite.name != "slot-symbol4") ||
               (image1.sprite.name != "slot-symbol4" && image2.sprite.name != "slot-symbol4" && image3.sprite.name == "slot-symbol4");
    }

}
