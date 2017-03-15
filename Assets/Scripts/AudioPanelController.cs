using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.Audio;

public class AudioPanelController : MonoBehaviour {

	public AudioMixer mixer;

	public void SetMasterVolume(float soundLvl)
	{
		mixer.SetFloat ("MasterVolume", soundLvl);
	}

	public void SetSoundEffectsVolume(float soundLvl)
	{
		mixer.SetFloat ("SoundEffectsVolume", soundLvl);
	}

	public void SetMusicVolume(float soundLvl)
	{
		mixer.SetFloat ("MusicVolume", soundLvl);
	}
}
