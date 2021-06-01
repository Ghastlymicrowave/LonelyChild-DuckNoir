using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using Combat;
public static class EnemyLibrary{
    public static EnemyClass GetEnemyFromId(int id){
        switch(id){
        case 0: return new Enemy1();
        default: return null;
        }
    }
}
public abstract class EnemyClass{
    public int id;
    public string name;
    public string[] toScroll;
    public int hp;
    public int maxHP;
    public List<EnemyActionCase> sentiment;
}

public class HeroClass{
    public int hp = 10;
    public int maxHP = 10;
}

public class EnemyActionCase{
    int actionType;
    int actionID;
    public EnemyActionCase(int type, int action){
        actionType=type;
        actionID = action;
    }
}

public class Enemy1 : EnemyClass{//example of an actual enemy
    public Enemy1(){
        sentiment = new List<EnemyActionCase>{
            new EnemyActionCase((int)ButtonEnum.Talk,(int)TalkEnum.Chat)};
        name = "Enemy 1";
        hp = 10;
        maxHP = 10;
        id = 1;
    }
}