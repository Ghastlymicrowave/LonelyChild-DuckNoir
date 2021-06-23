using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class SoundHolder : MonoBehaviour
{
    //this script allows you to assign audioclips to different actions.
    //You can make these sounds play by placing an event trigger on UI buttons and other things.
    public AudioSource audioSource;
    public AudioClip hover;
    public AudioClip click;
    public AudioClip otherOne;
    public AudioClip otherTwo;
    public void Hover()
    {
        audioSource.PlayOneShot(hover);
    }
    public void Click()
    {
        audioSource.PlayOneShot(click);
    }
    public void OtherOne()
    {
        audioSource.PlayOneShot(otherOne);
    }
    public void OtherTwo()
    {
        audioSource.PlayOneShot(otherTwo);
    }
}
