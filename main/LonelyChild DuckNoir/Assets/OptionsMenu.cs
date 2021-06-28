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
    [SerializeField] OptionContainer MusicVolContainer;
    [SerializeField] OptionContainer SfxVolContainer;
    [SerializeField] OptionContainer CamSmoothContainer;
    [SerializeField] OptionContainer OverShoulderContainer;
    //[SerializeField] OptionContainerBool

    public void Cancel(){
        this.gameObject.SetActive(false);
    }
    public void Close(){
        CloseOptions();
    }
    public void Default(){
        settings.Defaults();
        VisuallyUpdateSettings();
    }
    public void CloseOptions(){
        List<float> options = new List<float>();
        options.Add(HMouseSmoothingContainer.value);
        options.Add(VMouseSmoothingContainer.value);
        options.Add(HSensitivityContainer.value);
        options.Add(VSensitivityContainer.value);
        options.Add(MusicVolContainer.value);
        options.Add(SfxVolContainer.value);
        options.Add(CamSmoothContainer.value);
        options.Add(OverShoulderContainer.value);
        Debug.Log(settings);
        settings.ChangeOptions(options.ToArray());
        this.gameObject.SetActive(false);
    }

    public void VisuallyUpdateSettings(){
        float[] options = settings.GetOptions();
        HMouseSmoothingContainer.ChangeValueExternal(options[0]);
        VMouseSmoothingContainer.ChangeValueExternal(options[1]);
        HSensitivityContainer.ChangeValueExternal(options[2]);
        VSensitivityContainer.ChangeValueExternal(options[3]);
        MusicVolContainer.ChangeValueExternal(options[4]);
        SfxVolContainer.ChangeValueExternal(options[5]);
        CamSmoothContainer.ChangeValueExternal(options[6]);
        OverShoulderContainer.ChangeValueExternal(options[7]);
    }

    public void InitMenu(){
        settings = GameObject.Find("PersistentManager").GetComponent<SettingsManager>();
        settings.Load();
        VisuallyUpdateSettings();
    }
}
