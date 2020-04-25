using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Bidding : MonoBehaviour
{
    // Signal that shuffle is done and bidding may start
    public bool biddingPhase = false;
    // Currently bidding player
    public int playerBidding = 0;
    // How many players passed
    public int playerPassed = 0;
    // Idicator that it is the first bid
    public bool firstRound = true;
    // Maximal bid on the table
    public int maximalBid;
    private GameObject[] players;
    private GameObject[] cards = new GameObject[10];
    private int[] cardOrder = new int[10];


    private void Update()
    {
        if(biddingPhase)
        {
            players = GameObject.FindGameObjectsWithTag("Player");
            // When the first player is bidding it's automatically 100. All buttons are anabled
            if (firstRound)
            {
                foreach(GameObject player in players)
                {
                    foreach(Transform child in player.transform)
                    {
                        if(child.CompareTag("BiddingButton"))
                        {
                            child.gameObject.SetActive(true);
                        }
                    }
                }

                for (int i = 0; i < 3; i++)
                {
                    players[i].GetComponent<PlayerController>().bid = 100;
                    if (players[i].GetComponent<PlayerController>().playerID == 0)
                    {
                        players[i].GetComponent<PlayerController>().bidText.text = "Bid: " + players[i].GetComponent<PlayerController>().bid;
                        maximalBid = 100;
                        playerBidding = 1;
                    }
                }
                firstRound = false;
            }
            // Skiping the turn of a player that passed
            for(int i = 0; i < 3; i++)
            {
                if (players[i].GetComponent<PlayerController>().pass && players[i].GetComponent<PlayerController>().playerID == playerBidding)
                {
                    playerBidding = (playerBidding + 1) % 3;
                }

            }
            // If two players has passed there is a winner and bidding is over
            if(playerPassed == 2)
            {
                biddingPhase = false;
                foreach (GameObject player in players)
                {
                    player.GetComponent<PlayerController>().bidText.text = " ";
                    foreach (Transform child in player.transform)
                    {
                        if (child.CompareTag("BiddingButton"))
                        {
                            child.gameObject.SetActive(false);
                        }
                    }
                }

                // Looking for a winning player and giving them dealing cards
                int winningPlayerID = 0;
                foreach(GameObject player in players)
                {
                    if(!player.GetComponent<PlayerController>().pass)
                    {
                        winningPlayerID = player.GetComponent<PlayerController>().playerID;
                        for(int i = 0; i < 3; i++)
                        {
                            Instantiate(GetComponent<Shuffle>().dealingCards[i], new Vector3(2 * i + 6, player.transform.position.y, -7 - i), Quaternion.identity, player.transform);
                        }


                        // All below is just repositioning of cards
                        int counter = 0;
                        foreach (Transform child in player.transform)
                        {
                            if (child.CompareTag("Card"))
                            {
                                cards[counter] = child.gameObject;
                                counter++;
                            }
                        }

                        for (int i = 0; i < 10; i++)
                        {
                            cardOrder[i] = cards[i].GetComponent<CardProperties>().order;
                        }

                        QuickSort(cardOrder, 0, 9);

                        for (int i = 0; i < 10; i++)
                        {
                            for (int j = 0; j < 10; j++)
                            {
                                if (cards[j].GetComponent<CardProperties>().order == cardOrder[i])
                                {
                                    cards[j].transform.position = new Vector3(2 * i - 8, player.transform.position.y, -i);
                                }
                            }
                        }
                    }
                }

                // Setting new ID's
                foreach (GameObject player in players)
                {
                    player.GetComponent<PlayerController>().playerID = (player.GetComponent<PlayerController>().playerID + 3 - winningPlayerID) % 3;
                }

                GetComponent<Shuffle>().dealingPhaze = true;
                
            }

        }
    }


    private static void QuickSort(int[] array, int left, int right)
    {
        var i = left;
        var j = right;
        var pivot = array[(left + right) / 2];
        while (i < j)
        {
            while (array[i] < pivot) i++;
            while (array[j] > pivot) j--;
            if (i <= j)
            {
                // swap
                var tmp = array[i];
                array[i++] = array[j];  // ++ and -- inside array braces for shorter code
                array[j--] = tmp;
            }
        }
        if (left < j) QuickSort(array, left, j);
        if (i < right) QuickSort(array, i, right);
    }
}
