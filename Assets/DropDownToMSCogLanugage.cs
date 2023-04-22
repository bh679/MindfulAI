using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using TMPro;
using CognitiveServicesTTS;
using BrennanHatton.AI;

[System.Serializable]
public class LanguageOptions
{
	public string name, englishName, locale;
	public VoiceName[] voices;
	public bool include = true;
}


public class DropDownToMSCogLanugage : MonoBehaviour
{
	public LanguageOptions[] options;
	
	public MSCogVoiceToText voiceToText;
	public SpeechManager speechManager;
	public ArtPromptManager artPromptManager;
	public TMP_Dropdown dropDown;
	public int languageId, voiceId;
	
	public string[] values;//https://learn.microsoft.com/en-us/azure/cognitive-services/speech-service/language-support?tabs=stt
	public VoiceName[] voices; 
	public string textBeforeLang;
	
	void Reset()
	{
		dropDown = this.GetComponent<TMP_Dropdown>();
		voiceToText = GameObject.FindFirstObjectByType<MSCogVoiceToText>();
		speechManager = GameObject.FindFirstObjectByType<SpeechManager>();
		artPromptManager = GameObject.FindFirstObjectByType<ArtPromptManager>();
		
	}
	
	
    // Start is called before the first frame update
    void Start()
	{
		dropDown.ClearOptions();
    	
		for(int i = 0; i < options.Length; i++)
		{
			if(options[i].include)
				dropDown.options.Add(new TMP_Dropdown.OptionData(options[i].name));
		}
	    dropDown.onValueChanged.AddListener((int val)=>{ 
	    	Changed(val);
	    });
	}
    
	public void ChangeVoice(int id)
	{
		voiceId = id & options[languageId].voices.Length;
		
		speechManager.voiceName = options[languageId].voices[voiceId];
	}
    
	public void Changed(int val)
	{
		languageId = val;
		voiceToText.language = options[val].locale;
		speechManager.voiceName = options[val].voices[voiceId];
		artPromptManager.languagePrompt = textBeforeLang + options[val].name;
	}
}
