using System.Collections;
using System.Collections.Generic;
using System.Linq;
using UnityEngine;
using UnityEngine.UI;

public class Shuffle : MonoBehaviour
{
    // Size of a deck
    private int deckSize = 24;
    // Array with all cards - deck
    private int[] deck = new int[24];
    // List of cards - prefabs
    public GameObject[] card;
    // List of dealing cards
    public GameObject[] dealingCards = new GameObject[3];
    // List of players 
    public GameObject[] players;
    // Idicator of a shuffle phase
    public bool shuffleStart = true;

    // These two need to be here, beacuse Dealing is a script on every card and these values have to be global
    // Idicator of a dealing phase
    public bool dealingPhaze = false;
    // Idicator of a player that will recive the next card in dealing phase
    public int playerIDDealingCounter = 1;



    void Update()
    {
        if(shuffleStart)
        {
            
            // Resteting this value in case there was a reshuffle
            GetComponent<Bidding>().biddingPhase = 0;

            // Filling up the deck with cards
            for (int i = 0; i < 24; i++)
            {
                deck[i] = i;
            }
            // Shuffling
            for (int i = 0; i < deckSize; i++)
            {
                int temp = deck[i];
                int randomIndex = Random.Range(i, deckSize);
                deck[i] = deck[randomIndex];
                deck[randomIndex] = temp;
            }

            QuickSort(deck, 0, 6);
            QuickSort(deck, 7, 13);
            QuickSort(deck, 14, 20);
            for (int i = 0; i < 3; i++)
            {
                
                for (int j = 0; j < 7; j++)
                {
                    Instantiate(card[deck[j + 7*i]], new Vector3(2 * j - 8, players[i].transform.position.y, -j), Quaternion.identity, players[i].transform);
                }

                dealingCards[i] = card[deck[21 + i]];
                    
            }


            foreach(GameObject player in players)
            {
                player.GetComponent<PlayerController>().marriageInHand = false;

                // Marriage check for 120 bid rule
                foreach (Transform child in player.transform)
                {
                    if (child.CompareTag("Card"))
                    {
                        CardProperties cardProperties = child.gameObject.GetComponent<CardProperties>();
                        foreach (Transform child2 in player.transform)
                        {
                            if (child2.CompareTag("Card"))
                            {
                                
                                CardProperties cardProperties2 = child2.gameObject.GetComponent<CardProperties>();
                                if (cardProperties.type == cardProperties2.type && cardProperties.value == 3 && cardProperties2.value == 4)
                                {
                                    player.GetComponent<PlayerController>().marriageInHand = true;
                                    
                                }
                            }
                        }

                    }
                }


                player.GetComponent<RulesInHand>().ruleInHandCheck = true;
            }

            shuffleStart = false;
        }       
    }

    public static void QuickSort(int[] array, int left, int right)
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

