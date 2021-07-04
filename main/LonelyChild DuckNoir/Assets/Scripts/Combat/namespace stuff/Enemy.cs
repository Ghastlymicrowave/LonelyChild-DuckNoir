using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using System;

namespace Combat
{
    //This is the library that our combat systems will universally use.
    //Battles will use instances of these structs for battle logic.
    //Simply use "using Combat;" at the top of your script!
    [System.Serializable]
    public enum GamePosition { PlayerChoice, EnemyDialogue, EnemyAttack };
    //The position of a turn in battle.
    [System.Serializable]
    public enum SubmenuPosition { Regular, Attack, Talk, Inventory };
    //The Submenu state. Do we display attack and talk and stuff, or the inventory for example
    [System.Serializable]
    public enum ButtonEnum { Attack,Talk,Items,Run,Crucifix,Any };
    //An enum for button presses
    [System.Serializable]
    public struct Attack
    {
        //This is for Enemy's attack turn, containing basic information about the attack.

        public GameObject[] projectiles;
        //The prefabs to spawn as projectiles.

        public float[] spawnTimes;
        //When to Instantiate from projectiles.
        public Transform[] spawnLocations;
        //Where are we spawning these things?
        public float duration;
        //How long is the attack?
        public int damage;
        //The total damage done by the attack. 
        public string status;
        //If a projectile inflicts a status effect, this string is changed.
        //We can then apply that string to the Hero if this status has been updated.
        public int minDamage;
        //The minimum damage an attack can do, should probably stay at 0.

        public int maxDamage;
        //The maximum damage an attack can do. If this is reached, the attack ends.
    }

    [System.Serializable]
    public struct Hero
    {
        //O, basically. Player health, status effects, inventory, whatever the player needs.
        public int health;
        //The current health of the player.
        public int maxHealth;
        //The max health we can't exceed. The min health should probably remain zero.
        public string status;
        //A keyword Containing the current status.

    }
    [System.Serializable]

    public struct Projectile
    {
        //This is for the individual projectiles used within prefabs spawned during attacks.
        public Attack baseAttack;
        //The attack this belongs to, used to add damage on projectile collision.
        public int damage;
        //How much damage this projectile should do on collision.
        public float speed;
        //how fast does the projectile move?
        public float secondarySpeed;
        //optional value for the speed of, like, sine or whatever
        public float secondaryValue;
        //optional value for the size of, like, sine or whatever
        public string status;
        //If this contains a keyword for a status effect, the status for our player

    }

    [System.Serializable] 
    public enum AttackActions{//actions defined here must also be defined in TextManager with a default value
        Flashlight,
        Theremin,
        Fire_Poker,
        Garlic,
        Ruler
    }

    [System.Serializable] 
    public enum ItemsEnum{//actions defined here must also be defined in TextManager with a default value
        Apple,
        Ball,
        Photo,
        Key,
        StaircaseKey,
        MasterBedroomKey,
        ShowingRoomKey,
        LibraryKey,
        Hourglass,
        Fire_Poker,
        Manual,
        Teddy_Bear,
        Russian_Doll,
        Eraser,
        Spinning_Toy,
        KeyRing,
        Scissors
    }

    public enum TalkEnum{
        Talk,
        Requrest_Health,
        Requrest_Proceed,
        Flirt,
        Chat,
        ChatTwo,
        Pet,
        Fake_Throw,
        Call_Him_Bald,
        Gloat,
        Compliment,
        Insult,
        Lecture,
        Time,
        Clocks,
        Eternity,
        Present,
        Past,
        Hope,
        Future,
        Doom,
        Cycles,
        Bid_Farewell,
        Scold,
        Console,
        Encourage
    }

}
