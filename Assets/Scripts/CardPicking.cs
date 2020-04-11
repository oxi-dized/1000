using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class CardPicking : MonoBehaviour
{
    private void OnMouseDown()
    {
        //
        
        PlayerPicker playerPicker = GameObject.Find("Game Master").GetComponent<PlayerPicker>();
        int parentID = transform.parent.gameObject.GetComponent<PlayerController>().playerID;
        

        if (playerPicker.masterPlayerID== parentID)
        {
            transform.position = new Vector3(12, transform.position.y, transform.position.z);
            playerPicker.masterPlayerID = playerPicker.masterPlayerID+1;
            gameObject.tag = "CardInPlay";
        }                
    }
}
