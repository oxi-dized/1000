using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class CardProperties : MonoBehaviour
{ 
    // Type of card in numerical form
    // Dimonds = 1; Hearts = 2; Spades = 3; Clubs = 4;
    public int type;
    // Numerical value of a card
    // Nine = 0; Jack = 2; Queen = 3; King = 4; Ten = 10; Ace = 11;
    public int value;
    // Flags card as beeing possible to pick
    public bool pickable = false;
    // Flags a card if marriage of that type was played
    public bool marriage = false;
    // Flags a card as beeing able to initiate marriage
    public bool possibleMarriage = false;
}
