using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Repositioning : MonoBehaviour
{
    // Dealing is done so repositioning may be done
    public bool repoPhase = false;
    private GameObject[] cards = new GameObject[8];
    private int[] cardOrder = new int[8];



    void Update()
    {
        Bidding bidding = GameObject.Find("Game Master").GetComponent<Bidding>();
        if (repoPhase ==true)
        {
            repoPhase = false;
            
            int counter = 0;
            foreach(Transform child in transform)
            { 
                if(child.CompareTag("Card"))
                {
                    cards[counter] = child.gameObject;
                    counter++;
                }
            }

            for(int i = 0; i < 8; i++)
            {
                cardOrder[i] = cards[i].GetComponent<CardProperties>().order;
            }

            QuickSort(cardOrder, 0, 7);

            for(int i = 0; i < 8; i++)
            {
                for(int j = 0; j < 8; j++)
                {
                    if(cards[j].GetComponent<CardProperties>().order == cardOrder[i])
                    {
                        cards[j].transform.position = new Vector3(2 * i - 8, transform.position.y, -i);
                    }
                }
            }
            GameObject.Find("Game Master").GetComponent<FinalBid>().playerReady++;
            //GetComponent<PlayerController>().cardCheck = true;
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
