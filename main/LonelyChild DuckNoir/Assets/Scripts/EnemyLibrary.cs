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
    public void DamageEnemy(int val){
        hp-=val;
        if (hp<0){hp=0;}
    }
    protected int hp;
    protected int maxHP;
    protected EnemyActionCase[] sentiment;
}

public class EnemyActionCase{
    int actionType;
    int actionID;
    public EnemyActionCase(int type, int action){
        actionType=type;
        actionID = action;
    }
}

public class Enemy1 : EnemyClass{
    public Enemy1(){
        base.sentiment = new EnemyActionCase[1]{
            new EnemyActionCase((int)ButtonEnum.Items,(int)ItemsEnum.Ball)};
    }
}