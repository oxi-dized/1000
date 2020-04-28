using System;
using System.Collections;
using System.Collections.Generic;
using System.Linq;
using UnityEngine;
using UnityEngine.U2D;
using UnityEngine.UI;

public class PlayerController : MonoBehaviour
{
    public int playerID;
    public int score;
    public bool cardCheck = false;
    public Text scoreText;
    public Text totalScoreText;
    public Text bidText;
    public GameObject indicator;
    public int bid = 0;
    public bool pass = false;
    public int roundPlayerID;
    public int totallScore = 0;
    public bool marriageInHand = false;



    private GameObject[] cardsInPlay;

    private void LateUpdate()
    {
        EndOfTurnCalculation endOfTurnCalculation = GameObject.Find("Game Master").GetComponent<EndOfTurnCalculation>();
        if (endOfTurnCalculation.masterPlayerID == playerID && cardCheck)
        {

            cardCheck = false;

            // When the first player moves, they can select any card
            if (playerID == 0)
            {


                foreach (Transform child in transform)
                {


                    if (child.CompareTag("Card"))
                    {
                        CardProperties cardProperties = child.gameObject.GetComponent<CardProperties>();
                        // First player can pick any card so all are flaged as pickable
                        cardProperties.pickable = true;

                        // Checking and flaging if marriage is possible
                        foreach (Transform child2 in transform)
                        {
                            if (child2.CompareTag("Card"))
                            {
                                CardProperties cardProperties2 = child2.gameObject.GetComponent<CardProperties>();
                                if (cardProperties.type == cardProperties2.type && cardProperties.value == 3 && cardProperties2.value == 4)
                                {
                                    cardProperties.possibleMarriage = true;
                                    cardProperties2.possibleMarriage = true;
                                }
                            }
                        }

                    }
                }

            }
            else
            {

                cardsInPlay = GameObject.FindGameObjectsWithTag("CardInPlay");

                // Finding a card in play with the highest value and a propper type
                int maximalValueOfCardsInPlay = 0;
                for (int i = 0; i < cardsInPlay.GetLength(0); i++)
                {
                    int weightedValue = Convert.ToInt32(cardsInPlay[i].GetComponent<CardProperties>().type == endOfTurnCalculation.masterCardType) * cardsInPlay[i].GetComponent<CardProperties>().value;
                    if (weightedValue > maximalValueOfCardsInPlay)
                    {
                        maximalValueOfCardsInPlay = weightedValue;
                    }
                }

                // Checking if there is a card in play after marriage with different type than master type and if there is one, checking its value
                bool marriageInPlay = false;
                int maximalValueOfCardsWithMarriageInPlay = 0;
                for (int i = 0; i < cardsInPlay.GetLength(0); i++)
                {
                    marriageInPlay = marriageInPlay || (cardsInPlay[i].GetComponent<CardProperties>().marriage && !(cardsInPlay[i].GetComponent<CardProperties>().type == endOfTurnCalculation.masterCardType));
                    int weightedMarriageValue = Convert.ToInt32(cardsInPlay[i].GetComponent<CardProperties>().marriage)* cardsInPlay[i].GetComponent<CardProperties>().value;
                    if(weightedMarriageValue > maximalValueOfCardsWithMarriageInPlay)
                    {
                        maximalValueOfCardsWithMarriageInPlay = weightedMarriageValue;
                    }
                }

                // Checking if there is a card in the hand with a propper type
                bool cardsInHandWithPropperType = false;
                for (int i = 0; i < transform.childCount; i++)
                {                    
                    if(transform.GetChild(i).CompareTag("Card"))
                    {                      
                        if(transform.GetChild(i).gameObject.GetComponent<CardProperties>().type == endOfTurnCalculation.masterCardType)
                        {
                            cardsInHandWithPropperType = true;
                        }
                    }
                }

                // If there are cards with a propper type in the hand then one of them has to be played
                if (cardsInHandWithPropperType)
                {
                    // If there is a marriage in play then any card with a propper type can be played
                    if (marriageInPlay)
                    {
                        for (int i = 0; i < transform.childCount; i++)
                        {
                            if (transform.GetChild(i).CompareTag("Card"))
                            {
                                if (transform.GetChild(i).gameObject.GetComponent<CardProperties>().type == endOfTurnCalculation.masterCardType)
                                {
                                    transform.GetChild(i).gameObject.GetComponent<CardProperties>().pickable = true;
                                }
                            }
                        }
                    }
                    // If there is no marriage in play, then cards with a propper type that can take have to be played
                    else
                    {
                        // This bool will remain false if there are no cards with a propper type that can take
                        bool cardInHandCanTake = false;
                        // Flaging every card with a propper type that can take
                        for (int i = 0; i < transform.childCount; i++)
                        {
                            if (transform.GetChild(i).CompareTag("Card"))
                            {
                                if (transform.GetChild(i).gameObject.GetComponent<CardProperties>().type == endOfTurnCalculation.masterCardType && transform.GetChild(i).gameObject.GetComponent<CardProperties>().value > maximalValueOfCardsInPlay)
                                {
                                    transform.GetChild(i).gameObject.GetComponent<CardProperties>().pickable = true;
                                    cardInHandCanTake = true;
                                }
                            }
                        }
                        
                        // If no card with a propper type from the hand can take, then every card with a propper type can be played
                        if (!cardInHandCanTake)
                        {
                            for (int i = 0; i < transform.childCount; i++)
                            {
                                if (transform.GetChild(i).CompareTag("Card"))
                                {
                                    if (transform.GetChild(i).gameObject.GetComponent<CardProperties>().type == endOfTurnCalculation.masterCardType)
                                    {
                                        transform.GetChild(i).gameObject.GetComponent<CardProperties>().pickable = true;
                                    }
                                }
                            }
                        }
                    }
                }
                else
                {
                    // Checking if there is a card after a marriage in the hand
                    bool marriageInHand = false;
                    for (int i = 0; i < transform.childCount; i++)
                    {
                        if (transform.GetChild(i).CompareTag("Card"))
                        {
                            if(transform.GetChild(i).gameObject.GetComponent<CardProperties>().marriage)
                            {
                                marriageInHand = true;
                            }
                        }
                    }

                    if(marriageInHand)
                    {
                        // If there is a card after a marriage in the hand it has to be played only if it can take
                        if(marriageInPlay)
                        {
                            // This bool will remain false if there are no cards in the hand after marriage that can take
                            bool cardInHandCanTake = false;
                            //  Checking if there is a card after marriage that can take and if it is, flagging it
                            for (int i = 0; i < transform.childCount; i++)
                            {
                                if (transform.GetChild(i).CompareTag("Card"))
                                {
                                    if (transform.GetChild(i).gameObject.GetComponent<CardProperties>().marriage && transform.GetChild(i).gameObject.GetComponent<CardProperties>().value>maximalValueOfCardsWithMarriageInPlay)
                                    {
                                        cardInHandCanTake = true;
                                        transform.GetChild(i).gameObject.GetComponent<CardProperties>().pickable = true;
                                    }
                                }
                            }
                            // If a card after marriage can't take, then every card can be played
                            if(!cardInHandCanTake)
                            {
                                foreach (Transform child in transform)
                                {
                                    if (child.CompareTag("Card"))
                                    {
                                        child.gameObject.GetComponent<CardProperties>().pickable = true;
                                    }
                                }
                            }
                        }
                        // If there are no cards in play after marriage then every card after marriage can be played
                        else
                        {
                            foreach (Transform child in transform)
                            {
                                if (child.CompareTag("Card"))
                                {
                                    if(child.gameObject.GetComponent<CardProperties>().marriage)
                                    {
                                        child.gameObject.GetComponent<CardProperties>().pickable = true;
                                    }
                                }
                            }  
                        }
                    }
                    // If there are no cards in the hand affter marriage then every card can be played
                    else
                    {
                        foreach (Transform child in transform)
                        {
                            if (child.CompareTag("Card"))
                            { 
                                child.gameObject.GetComponent<CardProperties>().pickable = true;
                            }
                        }
                    }
                }
            }
        }                
    }

    void Marriage()
    {
        foreach (Transform child1 in transform)
        {
            foreach(Transform child2 in transform)
            {
                if (child1.CompareTag("Card") && child2.CompareTag("Card"))
                {
                    CardProperties cp1 = child1.gameObject.GetComponent<CardProperties>();
                    CardProperties cp2 = child2.gameObject.GetComponent<CardProperties>();
                    if (cp1.type == cp2.type && cp1.value == 3 && cp2.value == 4)
                    {
                        cp1.possibleMarriage = true;
                        cp2.possibleMarriage = true;
                    }
                }
            }
        }
    }
}
