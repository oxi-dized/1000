using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class CardPicking : MonoBehaviour
{

    private bool cardOfMasterType = false;


    private void OnMouseDown()
    {

        EndOfTurnCalculation endOfTurnCalculation = GameObject.Find("Game Master").GetComponent<EndOfTurnCalculation>();
        int parentID = transform.parent.gameObject.GetComponent<PlayerController>().playerID;
    
        // Checking if it's this player's move
        if (endOfTurnCalculation.masterPlayerID== parentID)
        {
            
            // Checking if it's the first player in a round
            if (parentID==0)
            {
                endOfTurnCalculation.masterCardType = GetComponent<CardProperties>().type;

                cardMove(endOfTurnCalculation);
            }
            else
            {
                // Checking if there exists a card with of a proper type in a player's hand
                foreach (Transform child in transform.parent)
                {
                    if (child.GetComponent<CardProperties>().type == endOfTurnCalculation.masterCardType)
                    {
                        cardOfMasterType = true;
                    }
                }

                // Checink if the card is of a proper type, or if there isn't any card of a proper type in a player's hand
                if (GetComponent<CardProperties>().type == endOfTurnCalculation.masterCardType || !cardOfMasterType)
                {
                    cardMove(endOfTurnCalculation);
                }
            }

                                  
        }
    }

    // Function moving the card
    void cardMove(EndOfTurnCalculation endOfTurnCalculation)
    {
        // Moving the card
        transform.position = new Vector3(12, transform.position.y, transform.position.z);
        // Ending the player's move
        endOfTurnCalculation.masterPlayerID = endOfTurnCalculation.masterPlayerID + 1;
        // Taging the played card with proper tag
        gameObject.tag = "CardInPlay";
    }
}
