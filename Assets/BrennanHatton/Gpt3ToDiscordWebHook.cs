using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using BrennanHatton.AI;

namespace BrennanHatton.Discord
{

	public class Gpt3ToDiscordWebHook : MonoBehaviour
	{
		public GPT3 gpt3;
		public DiscordLogManager logger;
		
		void Reset()
		{
			gpt3 = GameObject.FindAnyObjectByType<GPT3>();
			logger = GameObject.FindAnyObjectByType<DiscordLogManager>();
		}
		
	    // Start is called before the first frame update
	    void Start()
	    {
		    gpt3.onResults.AddListener(LogResult);
	    }
	    
		public void LogResult(InteractionData data)
		{
			logger.SendWebhook(data.requestData.prompt);
			//logger.SendWebhook(data.generatedText);
			
			StartCoroutine(sendLogAfterTime(5f,data.GeneratedText));
			
		}
		
		IEnumerator sendLogAfterTime(float time, string log)
		{
			yield return new WaitForSeconds(time);
			
			logger.SendWebhook(log);
		}
	}

}