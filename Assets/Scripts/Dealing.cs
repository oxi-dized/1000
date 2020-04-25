using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Dealing : MonoBehaviour
{


    private void OnMouseDown()
    {
        // The first player is the one that won a bid
        Shuffle shuffle = GameObject.Find("Game Master").GetComponent<Shuffle>();
        if (shuffle.dealingPhaze && transform.parent.gameObject.GetComponent<PlayerController>().playerID == 0)
        {
            GameObject[] players = GameObject.FindGameObjectsWithTag("Player");
            bool check = false;
            // Finding a player that will recive the next card
            for (int i = 0; i < 3; i++)
            {
                if(players[i].GetComponent<PlayerController>().playerID == shuffle.playerIDDealingCounter)
                {
                    transform.parent = players[i].transform;
                    transform.position = new Vector3(6 + shuffle.playerIDDealingCounter * 2, players[i].transform.position.y,transform.position.z);
                    check = true;
                }
            }

            if(check)
            {
                shuffle.playerIDDealingCounter++;
            }
            // When this couter hits 3 this means that dealing phase is over
            if(shuffle.playerIDDealingCounter == 3)
            {
                foreach(GameObject player in players)
                {
                    player.GetComponent<Repositioning>().repoPhase = true;

                }
                GameObject.Find("Game Master").GetComponent<Shuffle>().dealingPhaze = false;
            }
        }
    }
}
