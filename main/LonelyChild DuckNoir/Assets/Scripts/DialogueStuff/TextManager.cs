using System.Collections;
using System.Collections.Generic;
using UnityEngine;


public class TextManager : MonoBehaviour
{
    public static string[] stringsToArray(params string[] inputstr){
        return inputstr;
    }
    
    public string[] GetTextByID(int id){//using switch because no loaded memory and fast
        switch(id){
            case 0: return stringsToArray("fart",
            "fart 2",
            "fart 3, return of the fart");
            default: Debug.Log("got bad text id");return null;
        }
    }

    public string[] GetEnemyTextByID(int id){//using switch because no loaded memory and fast
        switch(id){
            // I'm leaving 0-50 for item behaviors and versatile game dialogue.
            case 0: return stringsToArray("You decided to do 'None' as your turn...",
            "You're not sure what you thought would happen...",
            "\"*Snide Judgement*\"");
            case 1: return stringsToArray("You ate the apple...",
            "and gained 5 health!",
            "\"...\"");
            case 2: return stringsToArray("You held the ball out to the being...",
            "But it cannot see it!",
            "Your machine needs more charge!",
            "\"...\"");


            //The start of test ghost 1
            //Attack
            case 51: return stringsToArray("You Attacked with the Flashlight...",
            "It was especially effective!",
            "\"Ow, who turned on the lights?\"");
            case 52: return stringsToArray("You Attacked with the Theramin...",
            "The ghost... liked it?",
            "\"That's nice...\"",
            "\"Not really my genre though.\"",
            "\"I'm more of a 'Boos' kind of guy.\""
            );
            case 53: return stringsToArray("You Attacked with the Fire Poker...",
            "It worked fine!",
            "\"Hey, cut that out!\"");
            case 54: return stringsToArray("You Attacked with the Garlic...",
            "It worked fine!",
            "\"I'm a ghost, not a vampire...\"");
            //Talk
            case 55: return stringsToArray("You smiled...",
            "The ghost smiled back!",
            "\"hehe.\"");
            case 56: return stringsToArray("You flirted with the ghost...",
            "He's not available!",
            "\"I actually have a boofriend...\"");
            case 57: return stringsToArray("You laugh...",
            "\"...\"",
            "\"You okay?\"");
            case 58: return stringsToArray("You stare at the ghost...",
            "The ghost stares back!",
            "How productive!");
            //alttalk
            case 59: return stringsToArray("You ate the apple...",
            "and gained 5 health!",
            "\"...\"");
            case 60: return stringsToArray("You ate the apple...",
            "and gained 5 health!",
            "\"...\"");
            case 61: return stringsToArray("You ate the apple...",
            "and gained 5 health!",
            "\"...\"");
            case 62: return stringsToArray("You ate the apple...",
            "and gained 5 health!",
            "\"...\"");
            //sentimental
            case 63: return stringsToArray("You showed the ball to the ghost...",
            "It felt... right.",
            "\"Thank you...\"");
            default: Debug.Log("got bad text id");return null;
        }
    }
}
