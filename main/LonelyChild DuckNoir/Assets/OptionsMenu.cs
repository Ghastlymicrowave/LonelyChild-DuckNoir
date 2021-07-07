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
    [SerializeField] OptionContainer CameraFollowSmoothContainer;
    [SerializeField] OptionContainer CameraRotateSmoothContainer;
    //[SerializeField] OptionContainerBool

    public void Cancel(){
        settings.Load();
        VisuallyUpdateSettings();
        settings.UpdateSounds();
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
        options.Add(CameraFollowSmoothContainer.value);
        options.Add(CameraRotateSmoothContainer.value);
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
        CameraFollowSmoothContainer.ChangeValueExternal(options[8]);
        CameraRotateSmoothContainer.ChangeValueExternal(options[9]);
    }

    public void RealtimeUpdateSingular(int option){
        if (option == 4){
            settings.ChangeOptionSingular(option, MusicVolContainer.value);
        }else if (option == 5){
            settings.ChangeOptionSingular(option, SfxVolContainer.value);
        }
    }
    public void InitMenu(){
        settings = GameObject.Find("PersistentManager").GetComponent<SettingsManager>();
        settings.Load();
        VisuallyUpdateSettings();
    }
}
