using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.U2D;

public class CardPicking : MonoBehaviour
{

    private void OnMouseDown()
    {
        if(GetComponent<CardProperties>().pickable)
        {
            EndOfTurnCalculation endOfTurnCalculation = GameObject.Find("Game Master").GetComponent<EndOfTurnCalculation>();

            if(transform.parent.GetComponent<PlayerController>().playerID==0)
            {
                endOfTurnCalculation.masterCardType = GetComponent<CardProperties>().type;
            }

            // Moving the card
            transform.position = new Vector3(12, transform.position.y, transform.position.z);
            // Ending the player's move
            endOfTurnCalculation.masterPlayerID = endOfTurnCalculation.masterPlayerID + 1;
            // Taging the played card with proper tag
            gameObject.tag = "CardInPlay";

            // Turning of ability to pick a card for this player
            GetComponent<CardProperties>().pickable = false;
            foreach (Transform child in transform.parent)
            {
                if(child.tag == "Card")
                {
                    child.gameObject.GetComponent<CardProperties>().pickable = false;
                }

                if (child.tag == "Indicator")
                {
                    child.gameObject.SetActive(false);
                }
            }
        } 
    }

   
}
