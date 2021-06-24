using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;
public class OptionContainer : MonoBehaviour
{
    OptionsMenu menu;
    [SerializeField] InputField thisInputField;
    [SerializeField] Slider thisSlider;
    public float value;//val from 0 to 1, expressed as int from 1 to 100 in text
    void Start()
    {
        GameObject.Find("OptionsMenu").GetComponent<OptionsMenu>();
    }

    // Update is called once per frame
    public void ChangeValueExternal(float value){
        thisInputField.text = thisSlider.value.ToString();
        thisSlider.value = value;
    }

    void CheckVal(){
        value = Mathf.Clamp(value,0f,1f);
    }

    void UpdateInputs(){
        thisInputField.text = (Mathf.RoundToInt(value*100)).ToString();
        thisSlider.value = value;
    }

    public void ValueChanged(bool slider){

        if (slider){//slider changed
            value = thisSlider.value;
            CheckVal();
            UpdateInputs();
            
        }else{//text changed
            value = ((float)int.Parse(thisInputField.text))/100f;
            CheckVal();
            UpdateInputs();
        }
    }
}
