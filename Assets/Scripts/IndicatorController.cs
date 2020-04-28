using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class IndicatorController : MonoBehaviour
{
    private GameObject[] players;
    private bool phaseBid;
    private bool phaseDeal;
    private bool phaseFinalBid;
    private bool phaseCard;

    void Start()
    {
        players = GameObject.FindGameObjectsWithTag("Player");
    }
    void Update()
    {
        Bidding bidding = GetComponent<Bidding>();
        Shuffle shuffle = GetComponent<Shuffle>();
        FinalBid finalBid = GetComponent<FinalBid>();
        EndOfTurnCalculation endOfTurnCalculation = GetComponent<EndOfTurnCalculation>();
        for (int i = 0; i < 3; i++)
        {
            PlayerController playerController = players[i].GetComponent<PlayerController>();
            phaseBid = bidding.biddingPhase == 3 && playerController.playerID == bidding.playerBidding;
            phaseDeal = shuffle.dealingPhaze && playerController.playerID == 0;
            phaseFinalBid = finalBid.playerReady == 3 && playerController.bid > 0;
            phaseCard = bidding.biddingPhase != 3 && !shuffle.dealingPhaze && finalBid.playerReady != 3 && playerController.playerID == endOfTurnCalculation.masterPlayerID;
            if (phaseBid || phaseDeal || phaseFinalBid || phaseCard)
            {
                playerController.indicator.SetActive(true);
            }
            else
            {
                playerController.indicator.SetActive(false);
            }

        }
    }
}
