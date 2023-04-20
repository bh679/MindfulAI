using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using CognitiveServicesTTS;

public class ApplyRandomVoice : MonoBehaviour
{
	public SpeechManager speech;
	public bool onStart;
	
	void Start()
	{
		if(onStart)
			PickRandomVoice();
	}
	
	public void PickRandomVoice()
	{
		speech.voiceName = (VoiceName)Random.Range(0,(int)VoiceName.Total);
	}
}
