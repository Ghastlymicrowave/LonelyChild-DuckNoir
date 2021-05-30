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
    public enum ButtonEnum { One, Two, Three, Four, Five };
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
    public struct Enemy
    {
        //An individual enemy, with that enemy's stats and attacks.
        public string sceneToLoad;
        //The scene to load when we win.
        public string name;
        //The enemy's name to be displayed.
        public int IDBase;
        //The ID for the earliest string. all battles will have consecutive ID's.
        public int Health;
        //Enemy's health.
        public int maxHealth;
        //Enemy's max health. Set this to the health.
        public string weakness;
        public string resistance;
        public int roundNum;
        //how many rounds/turns have taken place?

        public string sentimental;
        public Attack[] Attacks;
        //Our attacks, plain and simple.

        //The dialogue will be text files.
        //We're doing it like this to make dialogue easier, and not tied to Unity specifically.
        //Also, we can fill one array with each text line, which is cool.
        //public TextAsset[] fillerDialogue;
        //For filler text at start of round, to be chosen from at random.
        //public TextAsset[] attackDialogue;
        //For the attacks.

        //public TextAsset[] talkDialogue;
        //For the 8 possible talks in-game.
        //public TextAsset[] generalDialogue;
        //Need more textAssets for any specific battle script? Use this!
        public string[] attackChoices;
        public string[] talkChoices;
        public string[] altTalkChoices;
        public string[] itemsChoices;
        //These are the choices the buttons will be renamed to within their respective submenus.
        public string[] toScroll;
        //When a textasset needs to be used, its contents are dumped here first.
        //public IDictionary<string, TextAsset> inventoryDialogue;
        //names of items / what's said when they're used.
        public Hero hero;
        //And who is our enemy fighting, exactly?

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
        public string status;
        //If this contains a keyword for a status effect, the status for our player

    }

}
