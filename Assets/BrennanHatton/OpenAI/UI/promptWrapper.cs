using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using TMPro;

namespace BrennanHatton.AI
{
	/// <summary>
	/// strings and text files for prompts to wrap data
	/// </summary>
	[System.Serializable]
	public class PromptWrapper
	{
	
		[Tooltip("prePompt + data + postPromp")]
		public string prePrompt, postPrompt;
		
		[Tooltip("prePromptText + prePompt + data + postPromp + postPromptText")]
		public TextAsset[] prePromptText, postPromptText;
		
	
		[Tooltip("preInput + prePromptText + prePompt + data + postPromp + postPromptText + postInput")]
		public TMP_InputField[] preInput, postInput;
		
		string prompt;
		public string Prompt {get {return prompt;}}
		
		public string wrapPromopt(string _prompt)
		{
			string prompt = "";
			
			for(int i = 0; i < preInput.Length; i++)
				prompt += preInput[i].text;
			
			for(int i = 0; i < prePromptText.Length; i++)
				prompt += prePromptText[i].text;
			
			prompt += prePrompt + _prompt + postPrompt;
			
			for(int i = 0; i < prePromptText.Length; i++)
				prompt += postPromptText[i].text;
			
			for(int i = 0; i < postInput.Length; i++)
				prompt += postInput[i].text;
				
			Debug.Log(prompt);
				
			return prompt;
		}
	}
}
