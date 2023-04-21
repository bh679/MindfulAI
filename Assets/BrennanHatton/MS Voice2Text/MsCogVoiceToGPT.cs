using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.Events;

namespace BrennanHatton.AI
{

	public class MsCogVoiceToGPT : MonoBehaviour
	{
		public GPT3 gpt;
		public MSCogVoiceToText text2Speech;
		public PromptWrapper promptWrapper;
		public UnityEvent onVoiceFinish;
	
		int resultsCount = 0;
		
		void Reset()
		{
			gpt = FindObjectOfType<GPT3>();
			text2Speech = FindObjectOfType<MSCogVoiceToText>();
		}
	
	    // Update is called once per frame
	    void Update()
	    {
		    if(text2Speech.results.Count > resultsCount)
		    {
		    	string prompt;
		    	
		    	gpt.Execute(promptWrapper.wrapPromopt(text2Speech.results[resultsCount].message));
		    	
		    	resultsCount = text2Speech.results.Count;
		    	
		    	onVoiceFinish.Invoke();
		    	
		    }
	    }
	}

}