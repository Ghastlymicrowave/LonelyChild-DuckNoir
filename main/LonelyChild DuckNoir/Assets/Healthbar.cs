using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;
public class Healthbar : MonoBehaviour
{
    [SerializeField] Image healthbarFill;
    [SerializeField] Color damageColor;
    [SerializeField] Color healColor;
    [SerializeField] Color defaultColor;
    Color currentCol;
    Color targetColor;
    [SerializeField] float transitonSpd;
    [SerializeField] float colorTransitonSpd;
    float defaultWidth = 0;
    public float currentValue;
    public float targetValue;
    void Start()
    {
        defaultWidth = healthbarFill.rectTransform.sizeDelta.x;
        currentCol = defaultColor;
        targetColor = defaultColor;
    }
    void Update()
    {
        currentCol = Color.Lerp(currentCol,targetColor,colorTransitonSpd);
        currentValue = Mathf.Lerp(currentValue,targetValue,transitonSpd);
        
        healthbarFill.rectTransform.sizeDelta = new Vector2(currentValue*defaultWidth,healthbarFill.rectTransform.sizeDelta.y);
        healthbarFill.color = currentCol;
    }
    public void Damage(){
        currentCol = damageColor;
    }
    public void Heal(){
        currentCol = healColor;
    }
}
