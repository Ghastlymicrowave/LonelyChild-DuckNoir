using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;
public class OptionContainer : MonoBehaviour
{
    public OptionsMenu menu;
    [SerializeField] InputField thisInputField;
    [SerializeField] Slider thisSlider;
    [SerializeField] int sfxOrMuisc = 0;//0 for no, 1 for sfx, 2 for music
    public float value;//val from 0 to 1, expressed as int from 1 to 100 in text

    // Update is called once per frame
    public virtual void ChangeValueExternal(float newValue){
        value = newValue;
        thisInputField.text = Mathf.RoundToInt(value*100).ToString();
        thisSlider.value = value;
    }

    void CheckVal(){
        value = Mathf.Clamp(value,0f,1f);
    }

    void UpdateInputs(){
        thisInputField.text = (Mathf.RoundToInt(value*100)).ToString();
        thisSlider.value = value;
    }

    public virtual void ValueChanged(bool slider){
        Debug.Log("value changed: "+value.ToString());
        Debug.Log("sfxOrMusic: "+sfxOrMuisc.ToString());
        Debug.Log(menu.ToString());
        if (slider){//slider changed
            value = thisSlider.value;
            CheckVal();
            UpdateInputs();
            
        }else{//text changed
            value = ((float)int.Parse(thisInputField.text))/100f;
            CheckVal();
            UpdateInputs();
        }
        if(sfxOrMuisc==1){
            menu.RealtimeUpdateSingular(5);
            Debug.Log("sent");
        }else if (sfxOrMuisc==2){
            menu.RealtimeUpdateSingular(4);
            Debug.Log("sent");
        }
    }

    public void Hover(){
        GameObject.FindObjectOfType<MenuSounds>().Hover();
    }
    public void Click(){
        GameObject.FindObjectOfType<MenuSounds>().Click();
    }
}
