using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.U2D;

public class BiddingController : MonoBehaviour
{
    // If it's true then it is a pass buton, if not it is +10 button
    public bool passButton;


    void OnMouseDown()
    {
        Bidding bidding = GameObject.Find("Game Master").GetComponent<Bidding>();
        PlayerController playerController = transform.parent.gameObject.GetComponent<PlayerController>();
        if (bidding.playerBidding == playerController.playerID)
        {

            if (passButton)
            {
                // Zeroing bid after a pass
                playerController.bid = 0;
                playerController.pass = true;
                bidding.playerBidding = (bidding.playerBidding + 1) % 3;
                playerController.bidText.text = "Pass";

            }
            else
            {
                playerController.bid = bidding.maximalBid + 10;
                bidding.maximalBid = playerController.bid;
                bidding.playerBidding = (bidding.playerBidding + 1) % 3;
                playerController.bidText.text = "Bid: " + transform.parent.gameObject.GetComponent<PlayerController>().bid;
            }


        }

    }
}
