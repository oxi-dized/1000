using System.Collections;
using System.Collections.Generic;
using System.Data;
using UnityEngine;

public class RulesInHand : MonoBehaviour
{
    public bool ruleInHandCheck = false;
    private bool ruleBreak;
    private int sumInHand;
    private int numberOfNines;
    private GameObject[] cards;
    private GameObject[] players;

    
    void Update()
    {
        if(ruleInHandCheck)
        {
            sumInHand = 0;
            numberOfNines = 0;
            ruleInHandCheck = false;
            ruleBreak = false;
            foreach (Transform child in transform)
            {
                if(child.CompareTag("Card"))
                {
                    sumInHand += child.gameObject.GetComponent<CardProperties>().value;
                    if(child.gameObject.GetComponent<CardProperties>().value == 0)
                    {
                        numberOfNines++;
                    }
                }
            }

            if(numberOfNines == 4 || sumInHand < 17)
            {
                ruleBreak = true;
            }
            else
            {
                GameObject.Find("Game Master").GetComponent<Bidding>().biddingPhase++;
            }


        }
    }

    // I use here late update, because when update was used, the information about marriage in hand was transmited from the first (illigal) round to the second one
    private void LateUpdate()
    {
        if(ruleBreak)
        {
            ruleBreak = false;
            GameObject.Find("Game Master").GetComponent<Shuffle>().shuffleStart = true;
            cards = GameObject.FindGameObjectsWithTag("Card");
            foreach (GameObject card in cards)
            {
                Destroy(card);
            }
            players = GameObject.FindGameObjectsWithTag("Player");
            foreach (GameObject player in players)
            {
                player.GetComponent<PlayerController>().marriageInHand = false;
            }
        }
    }
}
