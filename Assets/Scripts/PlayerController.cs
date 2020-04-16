using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.U2D;
using UnityEngine.UI;

public class PlayerController : MonoBehaviour
{
    public int playerID;
    public int score;
    public bool cardCheck = true;
    public int maximalValueWithProperType = -1;
    public Text scoreText;


    private GameObject[] cardsInPlay;

    private void Update()
    {
        EndOfTurnCalculation endOfTurnCalculation = GameObject.Find("Game Master").GetComponent<EndOfTurnCalculation>();
        if (endOfTurnCalculation.masterPlayerID == playerID && cardCheck)
        {
            if (playerID == 0)
            {

                foreach (Transform child in transform)
                {
                    if (child.tag == "Card")
                    {
                        child.gameObject.GetComponent<CardProperties>().pickable = true;
                        cardCheck = false;
                        
                    }
                    if(child.tag == "Indicator")
                    {
                        child.gameObject.SetActive(true);
                    }
                }

            }
            else
            {
                // Extracting the maximal value of a card in play with a proper type
                cardsInPlay = GameObject.FindGameObjectsWithTag("CardInPlay");
                int maximalValueOfCardsInPlay = 0;
                for (int i = 0; i < cardsInPlay.GetLength(0); i++)
                {
                    int weightedValue = Convert.ToInt32(cardsInPlay[i].GetComponent<CardProperties>().type == endOfTurnCalculation.masterCardType) * cardsInPlay[i].GetComponent<CardProperties>().value;
                    if (weightedValue > maximalValueOfCardsInPlay)
                    {
                        maximalValueOfCardsInPlay = weightedValue;
                    }
                }

                // Checking if there exists a card with of a proper type in a player's hand
                foreach (Transform child in transform)
                {
                    if (child.tag == "Card")
                    {
                        if (child.gameObject.GetComponent<CardProperties>().type == endOfTurnCalculation.masterCardType)
                        {
                            if (child.gameObject.GetComponent<CardProperties>().value > maximalValueWithProperType)
                            {
                                maximalValueWithProperType = child.gameObject.GetComponent<CardProperties>().value;
                            }
                        }
                    }
                }

                foreach (Transform child in transform)
                {
                    if (child.tag == "Card")
                    {
                        // Checking if this card can be played, so there are no cards in hand of a proper type or this card is of a proper type and also one of two is true: 
                        // this card value is higher than max value of cards in play or no card in hand has a higher value than max value of cards in play
                        if (maximalValueWithProperType == -1 || (child.gameObject.GetComponent<CardProperties>().type == endOfTurnCalculation.masterCardType && (maximalValueWithProperType < maximalValueOfCardsInPlay || child.gameObject.GetComponent<CardProperties>().value > maximalValueOfCardsInPlay)))
                        {
                            child.gameObject.GetComponent<CardProperties>().pickable = true;
                            cardCheck = false;
                        }
                    }

                    if (child.tag == "Indicator")
                    {
                        child.gameObject.SetActive(true);
                    }

                }



                // Reseting the value
                maximalValueWithProperType = -1;

            }
        }                
    }
}
