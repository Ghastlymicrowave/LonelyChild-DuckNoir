using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class OptionsMenu : MonoBehaviour
{
    public SettingsManager settings;
    [SerializeField] OptionContainer HMouseSmoothingContainer;
    [SerializeField] OptionContainer VMouseSmoothingContainer;
    [SerializeField] OptionContainer HSensitivityContainer;
    [SerializeField] OptionContainer VSensitivityContainer;

    public void Cancel(){
        this.gameObject.SetActive(false);
    }
    public void Close(){
        CloseOptions();
    }
    public void CloseOptions(){
        List<float> options = new List<float>();
        options.Add(HMouseSmoothingContainer.value);
        options.Add(VMouseSmoothingContainer.value);
        options.Add(HSensitivityContainer.value);
        options.Add(VSensitivityContainer.value);
        settings.ChangeOptions(options.ToArray());
        this.gameObject.SetActive(false);
    }

    public void InitMenu(){
        settings = GameObject.Find("PersistentManager").GetComponent<SettingsManager>();
        float[] options = settings.GetOptions();
        HMouseSmoothingContainer.ChangeValueExternal(options[0]);
        VMouseSmoothingContainer.ChangeValueExternal(options[1]);
        HSensitivityContainer.ChangeValueExternal(options[2]);
        VSensitivityContainer.ChangeValueExternal(options[3]);
    }
}
