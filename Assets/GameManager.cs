using System;
using System.Collections;
using System.Collections.Generic;
using TMPro;
using UnityEngine;
using Random = UnityEngine.Random;

public class GameManager : MonoBehaviour
{

    [SerializeField]
    GameObject[] slotOneSymbols;
    [SerializeField]
    GameObject[] slotTwoSymbols;
    [SerializeField]
    GameObject[] slotThreeSymbols;

    public TMP_Text creditsText;
    public TMP_Text betText;

    int credits = 0, bet = 0;
    bool gameStarted = false;

    void Start()
    {
        credits = 0;
        bet = 0;
        gameStarted = false;
    }

    // Update is called once per frame
    void Update()
    {
        if (gameStarted)
        {
            //give prize according to combinations
            //to-do
            GivePrize();

            gameStarted = false;
        }
    }

    public void AddTenCredits()
    {
        credits += 10;
        ChangeCredits();
    }

    public void AddFiftyCredits()
    {
        credits += 50;
        ChangeCredits();
    }

    public void AddHundredCredits()
    {
        credits += 100;
        ChangeCredits();
    }

    public void ChangeCredits()
    {
        creditsText.text = credits.ToString();
        Debug.Log("added credits");
    }

    public void BetTenCredits()
    {
        bet = 10;
        ChangeBet();
    }

    public void BetFiftyCredits()
    {
        bet = 50;
        ChangeBet();
    }

    public void BetHundredCredits()
    {
        bet = 100;
        ChangeBet();
    }

    public void ChangeBet()
    {
        betText.text = bet.ToString();
        Debug.Log("changed bet");
    }

    public void StartGame()
    {
        //aleatorizar cada uma das slots
        //to-do
        RandomizeSymbols(slotOneSymbols);
        RandomizeSymbols(slotTwoSymbols);
        RandomizeSymbols(slotThreeSymbols);

        gameStarted =true;
    }

    public void GivePrize()
    {
        //give prize according to combinations

        ChangeCredits();
    }

    void RandomizeSymbols(GameObject[] symbols)
    {
        foreach (var symbol in symbols)
        {
            symbol.SetActive(false);
        }

        int randomIndex = Random.Range(0, symbols.Length);
        symbols[randomIndex].SetActive(true);
    }

}
