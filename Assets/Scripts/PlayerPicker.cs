using System;
using System.Collections;
using System.Collections.Generic;
using System.Linq;
using UnityEngine;

public class PlayerPicker : MonoBehaviour
{
    // ID of a current player
    public int masterPlayerID;
    // Type of the first played card
    public int masterCardType;

    private GameObject[] cardsInPlay;
    private int[] valuesOfCardsInPlay = new int[3];

   
    private void Update()
    {
        if(masterPlayerID==3)
        {
            // ID of a player that playes a card
            masterPlayerID = 0;
            // List of all the cards in play 
            cardsInPlay = GameObject.FindGameObjectsWithTag("CardInPlay");
            // Iterator
            int i = 0;
            foreach(GameObject card in cardsInPlay)
            {
                // Array of values of cards in play
                valuesOfCardsInPlay[i]=card.GetComponent<CardProperties>().value;
                i++;
                Destroy(card);
            }
            // Maximal value of cards in play
            int maximalValue =valuesOfCardsInPlay.Max();
            // Player winnig this turn
            int winningPlayer = Array.IndexOf(valuesOfCardsInPlay, maximalValue);
            // Adding values of all cards in this turn to the score of a winning player
            cardsInPlay[winningPlayer].transform.parent.GetComponent<PlayerController>().score += valuesOfCardsInPlay.Sum();
        }
    }
}
