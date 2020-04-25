using System;
using System.Collections;
using System.Collections.Generic;
using System.Threading.Tasks;
using UnityEngine;
using UnityEngine.UI;

public class FinalBid : MonoBehaviour
{
    public int playerReady = 0;
    public Slider sliderPrefab;
    private Slider slider;
    public Text finalBidText;
    private Text text;
    public Button bidButton;
    private Button button;
    private GameObject[] players = new GameObject[3];
    private bool bidChoice = false;
    void Update()
    {
        if(playerReady == 3)
        {
            playerReady = 0;

            players = GameObject.FindGameObjectsWithTag("Player");
            foreach(GameObject player in players)
            {
                if (player.GetComponent<PlayerController>().playerID == 0)
                {
                    slider = Instantiate(sliderPrefab, new Vector3(300, 29 * player.transform.position.y, 0), Quaternion.identity);
                    slider.transform.SetParent(GameObject.Find("Canvas").transform, false);
                    slider.minValue = GetComponent<Bidding>().maximalBid/10;
                    slider.maxValue = 32;
                    text = Instantiate(finalBidText, new Vector3(300, 29 * player.transform.position.y + 20, 0), Quaternion.identity);
                    text.transform.SetParent(GameObject.Find("Canvas").transform, false);
                    text.text = "Bid: " + slider.value;
                    button = Instantiate(bidButton, new Vector3(300, 29 * player.transform.position.y - 20, 0), Quaternion.identity);
                    button.transform.SetParent(GameObject.Find("Canvas").transform, false);
                    button.onClick.AddListener(TaskOnClick);

                }
            }

            bidChoice = true;
        }

        if(bidChoice)
        {
            text.text = "Bid: " + slider.value*10;
        }
    }

    void TaskOnClick()
    {
        bidChoice = false;
        foreach (GameObject player in players)
        {
            if (player.GetComponent<PlayerController>().playerID == 0)
            {
                player.GetComponent<PlayerController>().bid = Convert.ToInt32( slider.value * 10);
                Destroy(slider.gameObject);
                Destroy(text.gameObject);
                Destroy(button.gameObject);
            }
            player.GetComponent<PlayerController>().cardCheck = true;
        }
    }
}
