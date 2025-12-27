using UnityEngine;
using UnityEngine.Audio;

public class MusicVolumeUI : MonoBehaviour
{
    public AudioMixer mixer;

    public void SetMasterVolume(float value)
    {
        mixer.SetFloat("MasterVolume", value);
    }
}
