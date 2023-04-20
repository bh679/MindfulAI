using System.Collections;
using System.Collections.Generic;
using UnityEngine;

namespace BrennanHatton.AI
{

	public class ReadResponse : MonoBehaviour
	{
		public SpeechManager speechManager;
		public GPT3 gpt;
		int length = 0;
		
		void Reset()
		{
			speechManager = GameObject.FindObjectOfType<SpeechManager>();
			gpt = GameObject.FindObjectOfType<GPT3>();
		}
		
		// Update is called once per frame
		void Update()
		{
			if(length != gpt.interactions.Count)
			{
				speechManager.SpeakWithSDKPlugin(gpt.interactions[gpt.interactions.Count-1].GeneratedText);
				length = gpt.interactions.Count;
			}
		}
	}

}