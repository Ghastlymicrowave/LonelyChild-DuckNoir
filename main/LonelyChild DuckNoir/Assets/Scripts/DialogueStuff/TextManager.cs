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
            case 1: return stringsToArray("This is the tutorial for The Lonely Child!\nYou may click anywhere to read more.",
            "Use WASD To move and the mouse to interact with things.",
            "Interactables show up as '!' and can be clicked on to engage text and maybe add an item to your inventory.",
            "The ghosts walking around out there... if they touch you...",
            "You'll be sucked into turn based combat!",
            "You'll be able to attack with four weapons, with enemies having various weaknesses and resistances to them.",
            "During the enemy's turn, you'll have to dodge their attacks on your ghost finder using the mouse.",
            "Deal enough damage, and you'll be able to end the battle in one of two ways:",
            "Crucify (Mean and unfulfilling, but fast)\nOr...",
            "Select the right 'Talk' functionality, and then...",
            "Show them the right sentimental item from your inventory to save their soul.\n(Long, but you're a good person, I guess.)",
            "Get to the end and defeat the boss to beat the Game!",
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
            "This is a bizarre instrument that deals sound damage in battle.",
            "So THIS is where you left it.");
            case 4: return stringsToArray("You picked up a Fire Poker!",
            "This is an attack dealing physical damage in battle.",
            "The orphanage doesn't have a fireplace. What's this doing here?");
            case 5: return stringsToArray("You picked up some Garlic!",
            "This is an attack dealing smell damage in battle.",
            "How long has this been sitting here?");
            case 6: return stringsToArray("You picked up a ball.",
            "It appears to be covered in drool.");
            case 7: return stringsToArray("You picked up an apple.",
            "How long has this been here?");
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
                return new string[] {"Looks like that worked.", "Press E on the door again."};
            //Another Item
            case 12:
                return stringsToArray("You picked up a key!", "Go to a door you need to open and press the button on the left side of your screen...");
                // door stuff
            case 13:
                return stringsToArray("A locked door...", "You'll need to unlock it before you can use it.");
            case 14:
                return stringsToArray("You unlocked the door!", "Click on the exclamation point to open it!");
            case 15:
                return stringsToArray("The door opened!");
            case 16://friendly ghost text
            //actually ghost
                return new string[]{"“You know, I’ve read every book in that library...",
                "The living have no comprehension of ghosts. It’s all ‘ghosts are evil, they haunt old mansions and flee at the sound of theremins...’",
                "Actually, most ghosts I know are very nice if you give ‘em the chance...",
                "I happen to like theremins, and I only hang out in this old dump because, well..."};
            case 17://complaney ghost
                return new string[]{"This bathroom is waaaay nicer than that nasty one downstairs.  I wonder why that is..."};
                //cool ghost
            case 18: return new string[]{"Hey man, I been waiting for this elevator for the last two centuries.  I’m startin’ to think the ole girl aint workin…"};
            case 19: return new string[]{"If you wanna get downstairs...best find another way, kid"};
            case 20: return new string[]{"You’re pretty fly...for an alive guy."};
            //grief stricken nun
            case 21: return new string[]{"???"};
            case 22: return new string[]{"???"};
            case 23: return new string[]{"???"};
            //Manual, hourglass
            case 24: return stringsToArray("You picked up a ghost hunting manual.",
            "In the inventory, you can press inspect to read this thing.");

            case 25: return new string[]{"It's an evil looking eye. You could probably get rid of it if you stabbed it with something."};//eye interact
            case 26: return new string[]{"You ready your fire poker and get ready to jam it in the eye."};//eye firepoker used
            case 27: return new string[]{"The eye lets out a wheezing sound as parts of it wither away...",
            "You feel the floor rumble beneath you, whatever this is- it's getting weaker."};
            case 28: return new string[]{"The eye writhes in pain, thrashing wildly before stiffening...",
            "The tendrils underneath it seem to recoil, you're dealt this thing a great blow."};
            case 29: return new string[]{"As the eye makes a loud poping sound, splashing some dark oily liquid over you before becoming still.",
            "You can tell that hurt."};

            case 30: return stringsToArray("You picked up an hourglass.");
            //specific doors
            case 31:
                return stringsToArray("The master bedroom, locked as usual...", "You'll need to unlock it before you can use it.",
                "Press 'Use with' to get the party started...");
            case 32:
                return stringsToArray("The staircase, believe it or not, sits behind a locked door...", "You'll need to unlock it before you can use it.");
            case 33:
                return stringsToArray("The showing room, where lucky children get a chance at a home...\n...and the door's locked.", "You'll need to unlock it before you can use it.");
            case 34:
                return stringsToArray("His bed.", "You feel shivers down your spine.");
            case 35:
                return stringsToArray("Crayon drawings are strewn about.\nYou almost feel nostalgic.", "You remember your peers who drew this. Are they okay?");
            case 36:
                return stringsToArray("He ate here.\nNever let you anywhere near.", "Look at you now, breaking the rules!\nYou little anti-establishmentarian, you!");


            case 37: return new string[]{"This is some kind of massive gaping hole... or is it a mouth?",
            "You think you can see the floor below it, but there's huge teeth in the way."};

            default: Debug.Log("got bad text id");return null;
            case 38:
                return stringsToArray("You know, I’ve read every book in that library...The living have no comprehension of ghosts. ",
                "It’s all ‘ghosts are evil, they haunt old mansions and flee at the sound of theremins...’",
                "Actually, most ghosts I know are very nice if you give ‘em the chance...",
                "I happen to like theremins, and I only hang out in this old dump because, well…");
            //More items
            case 39: return stringsToArray("You picked up a teddy bear.",
            "It's painfully familiar.");
            case 40: return stringsToArray("You picked up a russian doll.",
            "Or, at least you think it's russian?\nYou're an orphan, not a history professor.");
            case 41: return stringsToArray("You picked up an eraser.",
            "Put it down, if you know what's good for you.",
            "I know you can hear me.",
            "Lets make this easier for both of us.");
            case 42: return stringsToArray("You picked up a spinning toy.",
            "If every other orphan wasn't mysteriously missing, you wouldn't want to be seen with this thing.");
            //Inspects
            case 43: return stringsToArray("The bedroom. Locked.");
            case 44: return stringsToArray("The urinal.\nBelieve it or not, locked every night.");
            case 45: return stringsToArray("\"Zzzz....\"\n\"...what are you lookin' at?....\"");
            //overworld Tutorial Text
            case 46: return stringsToArray("You found a page from the ghost hunting manual.\nPress E or LMB to continue.",
            "\"Out there, when you're hunting for ghosts inside of haunted areas, you might just need to read something.\"",
            "\"Not everyone knows this, but reading in a haunted house is actually much different from normal reading.\"\n\"Here's how you read in a haunted house:\"",
            "\"First, approach the thing you want to read and press 'E' on your keyboard.\"\n\"(The weird looks you will get for walking around with a keyboard are irrelevant, ignore them.)\"",
            "\"Second, press E again until you're done reading!\"\n\"Happy hunting!\"");
            case 47:
                return stringsToArray("The library door, locked.\nYou'll need to unlock it before you can use it.", "If you find a key, pick it up with E.");
            case 48:
                return stringsToArray("It's a locked door leading to the other part of the hallway, but...\n...the lock doesn't have a key hole.", "Instead, a reaaaaally small rope is holding the doorknob in place.\nIt's practically invisible to the human eye.",
                "it seems that you'll need to use something other than a key on this door.");
            case 49:
                return stringsToArray("You picked up the library key!","To open doors with keys, or use any item on anything, approach the door and press 'Tab' or 'I' to open up the menu.",
                    "From there, scroll down to the proper key and press 'use with'.");
            case 50:
                return stringsToArray("You picked up the master bedroom key!");
            case 51:
                return stringsToArray("You picked up the staircase key!");
            case 52:
                return new string[]{"You picked up these scissors, man these feel heavy... and very worn."};
            case 53:
                return new string[]{"The door won't open! It looks like someone's tied a rope to the handle?",
                "You'll need something sharp to cut it off."};
            case 54:
                return new string[]{"It takes a lot of effort, but you manage to cut the rope, freeing the door to be opened.",
                "...But not without breaking the scissors in the process. Maybe it was just too worn down?",
                "Interact with the door again to open it."};
            case 55:
                return new string[]{"You picked up the keyring! It makes you feel like a janitor.",
                "With this, you won't need to use a key on a door from your inventory, simply interact with a door, and if you have a key for it, it'll open!"};
            case 56:
                return stringsToArray("You picked up the showing room key!");
            case 57:
                return new string[] {"Looks like that worked."};
            case 58:
                return new string[] {"This is the door to the basement.\nUnlocked.","Upon opening, however, the staircase leading to the basement door, along with the basement itself, falls about seven stories downward.", "You should have known it wouldn't be that simple."};
            case 59:
                return new string[] {"\"Hey, dude.\"\n\"Don't use the door behnd me.\""};
            case 60:
                return new string[]{
                    "If you're trying to get to the basement, I don't think the stairs will be much help to you.",
                    "However, I think this weird mouth...portal thing might take you down there. But it's teeth would get in the way so you'll need to get rid of those somehow.",
                    "There are a few creepy eye-things on this floor, so you might try checking out those."
                };
            case 61:
                return new string[]{"It's a shame the stairs are broken, if you're serious about going into the basement I'd reccomend messing up the eyes with something sharp, like the fire poker by that new fireplace.",
                "Do you ever remember a fireplace in that room? It must have been a new addition, i've never seen it before now."};
            
        }
    }
}
