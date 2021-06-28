using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class OptionContainerBool : OptionContainer
{
    OptionsMenu menu;
    [SerializeField] Toggle thisToggle;

    public override void ChangeValueExternal(float newValue){
        value = newValue;
        Debug.Log("new value: "+value.ToString());
        UpdateInputs();
    }

    void UpdateInputs(){
        thisToggle.isOn = BoolFromFloat(value);
    }

    bool BoolFromFloat(float i){
        return (Mathf.RoundToInt(i)==1);
    }

    public override void ValueChanged(bool slider = false){
        value = 0f;
        if (thisToggle.isOn){
            value=1f;
        }
        Debug.Log("value set to :"+value.ToString());
    }
}
