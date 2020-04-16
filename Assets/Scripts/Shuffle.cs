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
    // List of players 
    public GameObject[] player;

    void Start()
    {

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

        QuickSort(deck, 0, 7);
        QuickSort(deck, 8, 15);
        QuickSort(deck, 16, 23);
        for (int i = 0; i < 3; i++)
        {
            for (int j = 0; j < 8; j++)
            {
                Instantiate(card[deck[j + 8 * i]], new Vector3(2 * j - 8, 5 * i - 5, -j), Quaternion.identity, player[i].transform);
            }
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

