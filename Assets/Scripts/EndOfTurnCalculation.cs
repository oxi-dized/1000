using System;
using System.Collections;
using System.Collections.Generic;
using System.Linq;
using UnityEngine;

public class EndOfTurnCalculation : MonoBehaviour
{
    // ID of a current player
    public int masterPlayerID;
    // Type of the first played card
    public int masterCardType;

    private GameObject[] cardsInPlay;
    private GameObject[] players;
    private int[] valuesOfCardsInPlay = new int[3];
    private int[] typesOfCardsInPlay = new int[3];
    private int[] weightedValuesOfCardsInPlay = new int[3];

    private void Start()
    {
        // Player list
        players = GameObject.FindGameObjectsWithTag("Player");
    }

    private void Update()
    {
        if (masterPlayerID == 3)
        {           
            // List of all the cards in play 
            cardsInPlay = GameObject.FindGameObjectsWithTag("CardInPlay");
            // Iterator
            int i = 0;

            // Cards in play analysis
            foreach (GameObject card in cardsInPlay)
            {
                typesOfCardsInPlay[i] = Convert.ToInt32(card.GetComponent<CardProperties>().type == masterCardType);
                // Array of values of cards in play
                valuesOfCardsInPlay[i] = card.GetComponent<CardProperties>().value;
                // Array of values of cards in play of a proper type
                weightedValuesOfCardsInPlay[i] = valuesOfCardsInPlay[i] * typesOfCardsInPlay[i];
                i++;
                Destroy(card);
            }
            // Maximal value of cards in play
            int maximalValue = weightedValuesOfCardsInPlay.Max();
            // Player winnig this turn
            int winningPlayer = Array.IndexOf(weightedValuesOfCardsInPlay, maximalValue);
            // Adding values of all cards in this turn to the score of a winning player
            cardsInPlay[winningPlayer].transform.parent.GetComponent<PlayerController>().score += valuesOfCardsInPlay.Sum();


            //Determining the order of players in the next turn
            foreach(GameObject player in players)
            {
                player.GetComponent<PlayerController>().playerID = (player.GetComponent<PlayerController>().playerID + 3 - winningPlayer) % 3;
            }

            masterPlayerID = 0;
        }
    }
}
