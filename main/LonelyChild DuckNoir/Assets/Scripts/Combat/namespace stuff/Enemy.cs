using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using System;

namespace Combat
{
    //This is the library that our combat systems will universally use.
    //Battles will use instances of these structs for battle logic.
    //Simply use "using Combat;" at the top of your script!
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
    public struct Enemy 
    {
        //An individual enemy, with that enemy's stats and attacks.
        public int Health;
        //Enemy's Health.
        public string weakness;
        public Attack[] Attacks;
        //Our attacks, plain and simple.
        public TextAsset[] dialogues;
        //The dialogue as text files.
        //We're doing it like this to make dialogue easier, and not tied to Unity specifically.
        //Also, we can fill one array with each text line, which is cool.
        public Hero hero;
        //And who is our enemy fighting, exactly?

    }
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