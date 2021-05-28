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
            case 0: return stringsToArray("text could be this",
            "or this",
            "maybe this");
            default: Debug.Log("got bad text id");return null;
        }
    }
}
