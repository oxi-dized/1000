using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class EndOfRoundCalculation : MonoBehaviour
{
    public Text winnerText;

    private GameObject[] players;
    private bool endGameTrigger=false;
    private string winner;

    void Update()
    {
        if(GetComponent<EndOfTurnCalculation>().turnCounter == 8)
        {
            players = GameObject.FindGameObjectsWithTag("Player");
            foreach(GameObject player in players)
            {
                PlayerController playerController = player.GetComponent<PlayerController>();
                int mod = playerController.score % 10;
                if (playerController.bid == 0)
                {
                    if(playerController.totallScore < 800)
                    {
                        if (mod <= 5)
                        {
                            playerController.totallScore += playerController.score - mod;
                        }
                        else
                        {
                            playerController.totallScore += playerController.score - mod + 10;
                        }
                    }
                }
                else
                {
                    if (playerController.score >= playerController.bid)
                    {
                        playerController.totallScore += playerController.bid;
                    }
                    else
                    {
                        playerController.totallScore += -playerController.bid;
                    }
                }
                if (playerController.totallScore >= 1000)
                {
                    endGameTrigger = true;
                    winner = player.name;
                }


                playerController.roundPlayerID = (playerController.roundPlayerID + 2) % 3;

                playerController.totalScoreText.text = "Total score: " + playerController.totallScore; 
                playerController.playerID = playerController.roundPlayerID;
                playerController.score = 0;
                playerController.bid = 0;
                playerController.cardCheck = false;
                playerController.scoreText.text = "Score: 0";
                playerController.pass = false;
                playerController.marriageInHand = false;
            }

            if(endGameTrigger)
            {
                winnerText.text = winner + " has won!";
            }
            else
            {
                GetComponent<EndOfTurnCalculation>().turnCounter = 0;
                GetComponent<Shuffle>().playerIDDealingCounter = 1;
                GetComponent<Bidding>().playerBidding = 0;
                GetComponent<Bidding>().firstRound = true;
                GetComponent<Bidding>().maximalBid = 0;

                GetComponent<Shuffle>().shuffleStart = true;
            }
            

        }
    }
}
