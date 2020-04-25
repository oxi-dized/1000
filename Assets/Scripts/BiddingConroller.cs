using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.U2D;

public class BiddingConroller : MonoBehaviour
{
    // If it's true then it is a pass buton, if not it is +10 button
    public bool passButton;

    private void Update()
    {
        if(GameObject.Find("Game Master").GetComponent<Bidding>().biddingPhase && transform.parent.gameObject.GetComponent<PlayerController>().playerID == GameObject.Find("Game Master").GetComponent<Bidding>().playerBidding)
        {
            Indicator(true);
        }
    }

    void OnMouseDown()
    {
        Bidding bidding = GameObject.Find("Game Master").GetComponent<Bidding>();
        PlayerController playerController = transform.parent.gameObject.GetComponent<PlayerController>();
        if (bidding.playerBidding == playerController.playerID)
        {
            Indicator(false);

            if (passButton)
            {
                bidding.playerPassed++;
                // Zeroing bid after a pass
                playerController.bid = 0;
                playerController.pass = true;
                bidding.playerBidding = (bidding.playerBidding + 1) % 3;
                transform.parent.gameObject.GetComponent<PlayerController>().bidText.text = "Pass";

            }
            else
            {
                playerController.bid = bidding.maximalBid + 10;
                bidding.maximalBid = playerController.bid;
                bidding.playerBidding = (bidding.playerBidding + 1) % 3;
                transform.parent.gameObject.GetComponent<PlayerController>().bidText.text = "Bid: " + transform.parent.gameObject.GetComponent<PlayerController>().bid;
            }


        }

    }

    void Indicator(bool active)
    {

        foreach (Transform child in transform.parent)
        {
            if (child.CompareTag("Indicator"))
            {
                child.gameObject.SetActive(active);
            }
        }

    }
}
