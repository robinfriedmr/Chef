using System.Collections;
using UnityEngine;
using UnityEngine.Audio;

public class MixLevels : MonoBehaviour {

    public AudioMixer masterMixer;

   /* public void setGlitchLvl(float glitchLvl)
    {
        masterMixer.SetFloat("Glitch Track", glitchLvl);
    }

    public void setAprtmntMusicLvl(float musicLvl)
    {
        masterMixer.SetFloat("Amibent Music", musicLvl);
    } */

    public void SetVolume(float volume)
    {
        masterMixer.SetFloat("volume", volume);
    }
}

