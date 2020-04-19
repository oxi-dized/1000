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
    public bool cardCheck = true;
    public Text scoreText;



    private GameObject[] cardsInPlay;

    private void Update()
    {
        EndOfTurnCalculation endOfTurnCalculation = GameObject.Find("Game Master").GetComponent<EndOfTurnCalculation>();
        if (endOfTurnCalculation.masterPlayerID == playerID && cardCheck)
        {

            foreach (Transform child in transform)
            {
                if (child.tag == "Indicator")
                {
                    child.gameObject.SetActive(true);
                }
            }

            if (playerID == 0)
            {


                foreach (Transform child in transform)
                {


                    if (child.tag == "Card")
                    {
                        CardProperties cardProperties = child.gameObject.GetComponent<CardProperties>();
                        // First player can pick any card so all are flaged as pickable
                        cardProperties.pickable = true;
                        cardCheck = false;

                        // Checking and flaging if marriage is possible
                        foreach (Transform child2 in transform)
                        {
                            if (child2.tag == "Card")
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
                int maximalValueOfCardsInPlay = 0;
                for (int i = 0; i < cardsInPlay.GetLength(0); i++)
                {
                    int weightedValue = Convert.ToInt32(cardsInPlay[i].GetComponent<CardProperties>().type == endOfTurnCalculation.masterCardType) * cardsInPlay[i].GetComponent<CardProperties>().value;
                    if (weightedValue > maximalValueOfCardsInPlay)
                    {
                        maximalValueOfCardsInPlay = weightedValue;
                    }
                }

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

                bool cardsInHandWithPropperType = false;
                for (int i = 0; i < transform.childCount; i++)
                {                    
                    if(transform.GetChild(i).tag == "Card")
                    {                      
                        if(transform.GetChild(i).gameObject.GetComponent<CardProperties>().type == endOfTurnCalculation.masterCardType)
                        {
                            cardsInHandWithPropperType = true;
                        }
                    }
                }
                if (cardsInHandWithPropperType)
                {
                    if (marriageInPlay)
                    {
                        for (int i = 0; i < transform.childCount; i++)
                        {
                            if (transform.GetChild(i).tag == "Card")
                            {
                                if (transform.GetChild(i).gameObject.GetComponent<CardProperties>().type == endOfTurnCalculation.masterCardType)
                                {
                                    transform.GetChild(i).gameObject.GetComponent<CardProperties>().pickable = true;
                                }
                            }
                        }
                    }
                    else
                    {
                        bool cardInHandCanTake = false;
                        for (int i = 0; i < transform.childCount; i++)
                        {
                            if (transform.GetChild(i).tag == "Card")
                            {
                                if (transform.GetChild(i).gameObject.GetComponent<CardProperties>().type == endOfTurnCalculation.masterCardType && transform.GetChild(i).gameObject.GetComponent<CardProperties>().value > maximalValueOfCardsInPlay)
                                {
                                    transform.GetChild(i).gameObject.GetComponent<CardProperties>().pickable = true;
                                    cardInHandCanTake = true;
                                }
                            }
                        }

                        if (!cardInHandCanTake)
                        {
                            for (int i = 0; i < transform.childCount; i++)
                            {
                                if (transform.GetChild(i).tag == "Card")
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
                    bool marriageInHand = false;
                    for (int i = 0; i < transform.childCount; i++)
                    {
                        if (transform.GetChild(i).tag == "Card")
                        {
                            if(transform.GetChild(i).gameObject.GetComponent<CardProperties>().marriage)
                            {
                                marriageInHand = true;
                            }
                        }
                    }

                    if(marriageInHand)
                    {
                        if(marriageInPlay)
                        {
                            bool cardInHandCanTake = false;
                            for (int i = 0; i < transform.childCount; i++)
                            {
                                if (transform.GetChild(i).tag == "Card")
                                {
                                    if (transform.GetChild(i).gameObject.GetComponent<CardProperties>().marriage && transform.GetChild(i).gameObject.GetComponent<CardProperties>().value>maximalValueOfCardsWithMarriageInPlay)
                                    {
                                        cardInHandCanTake = true;
                                        transform.GetChild(i).gameObject.GetComponent<CardProperties>().pickable = true;
                                    }
                                }
                            }

                            if(!cardInHandCanTake)
                            {
                                foreach (Transform child in transform)
                                {
                                    if (child.tag == "Card")
                                    {
                                        child.gameObject.GetComponent<CardProperties>().pickable = true;
                                    }
                                }
                            }
                        }
                        else
                        {
                            foreach (Transform child in transform)
                            {
                                if (child.tag == "Card")
                                {
                                    if(child.gameObject.GetComponent<CardProperties>().marriage)
                                    {
                                        child.gameObject.GetComponent<CardProperties>().pickable = true;
                                    }
                                }
                            }  
                        }
                    }
                    
                    else
                    {
                        foreach (Transform child in transform)
                        {
                            if (child.tag == "Card")
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
                if (child1.tag == "Card"&& child2.tag == "Card")
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
