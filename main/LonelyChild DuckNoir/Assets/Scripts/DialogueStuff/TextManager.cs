using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using Combat;

public class TextManager : MonoBehaviour
{
    public static string[] stringsToArray(params string[] inputstr){
        return inputstr;
    }

    public static string[] GetTextByID(int id){//using switch because no loaded memory and fast
        switch(id){
            case 0: return stringsToArray("That didn't work");
            case 1: return stringsToArray("This is the tutorial for The Lonely Child.\nYou may click anywhere to read more.",
            "Use WASD To move and the mouse to interact with things.",
            "Interactables show up as '!' and can be clicked on to engage text and maybe add an item to your inventory.",
            "The ghosts walking around out there... if they touch you...",
            "You'll be sucked into turn based combat.",
            "You'll be able to attack with four weapons, with enemies having various weaknesses and resistances to them.",
            "During the enemy's turn, you'll have to dodge their attacks on your ghost finder using the mouse.",
            "Deal enough damage, and you'll be able to end the battle in one of two ways:",
            "Crucify (Mean and unfulfilling, but fast)\nOr...",
            "Show them the right sentimental item from your inventory to save their soul.\n(Long, but you're a good person I guess.)",
            "Get to the end and defeat the boss to beat the Alpha version!~",
            "But don't think it'll be that easy, however...",
            "There's puzzles to solve... An orphanage to navigate...",
            "And...\nOf course...",
            "This place IS haunted, you know..."
            );
            case 2: return stringsToArray("This is the end.",
            "Walk through this door, and you won't come back.",
            "Consider your decision wisely.");
            //items / attacks
            case 3: return stringsToArray("You picked up a Theremin!",
            "This is an attack dealing sound damage in battle.",
            "So THIS is where you left it.");
            case 4: return stringsToArray("You picked up a Fire Poker!",
            "This is an attack dealing physical damage in battle.",
            "The orphanage doesn't have a fireplace. What's this doing here?");
            case 5: return stringsToArray("You picked up some Garlic!",
            "This is an attack dealing smell damage in battle.",
            "How long has this been sitting here?");
            case 6: return stringsToArray("You picked up a ball.",
            "Looks like it'd be fun to play with.");
            case 7: return stringsToArray("You picked up an apple.",
            "Who keeps leaving fruits and veggies everywhere?");
            case 8: return stringsToArray("You picked up a Photo.",
            "It looks rather mundane... but it might mean something more to someone.");
            //Descriptors
            case 9:
                return stringsToArray("A cute little playroom table set.",
                "You've got to wonder why they make 'em so big.");
            case 10:
                return stringsToArray("You've always wanted to learn how to play the piano.",
                "Too bad all you have is this big freakin' capsule.");
            case 11:
                return new string[] {"Looks like that worked"};
            //Another Item
            case 12:
                return stringsToArray("You picked up a key!", "Go to a door you need to open and press the button on the left side of your screen...", 
                "Press 'Use with' to get the party started...");
            default: Debug.Log("got bad text id");return null;
        }
    }

    /*
    //this will eventually be replace by the same method in battleBehavior, i'm keeping this here in case
    //there are or will be any other dependencies for it for the time being.
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
    }*/
}
