using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class HelpPanel : MonoBehaviour
{
    [SerializeField] GameObject[] panels;
    [SerializeField] Button left;
    [SerializeField] Button right;
    int panel = 0;
    void OnEnable(){
        UpdatePanel();
    }
    public void RightButton(){
        panel++;
        UpdatePanel();
    }
    public void LeftButton(){
        panel--;
        UpdatePanel();
    }
    void UpdatePanel(){
        if (panel > panels.Length){
            panel = panels.Length-1;
        }else if (panel < 0){
            panel = 0;
        }
        for(int i= 0; i < panels.Length; i++){
            if (i == panel){
                panels[i].gameObject.SetActive(true);
            }else{
                panels[i].gameObject.SetActive(false);
            }
        }
        if (panel == 0){
            left.interactable = false;
        }else{
            left.interactable = true;
        }

        if (panel == panels.Length-1){
            right.interactable = false; 
        }else{
            right.interactable = true;
        }
    }
}
