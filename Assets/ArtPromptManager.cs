using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using BrennanHatton.AI;



public class ArtPromptManager : MonoBehaviour
{
	public MsCogVoiceToGPT GPTGen;
	
	public int paintingId = 0, questionId = 0;
	
	public string[] PaintingPersonality,
		Questions,
		Wisdom;
	
	void Reset()
	{
		GPTGen = this.GetComponent<MsCogVoiceToGPT>();
	}
	
	void Start()
	{
		AskQuestion();
	}
	
	public void AskQuestion()
	{
		GPTGen.promptWrapper.prePrompt = PaintingPersonality[paintingId]+Questions[questionId];
		
		questionId = (questionId + 1) % Questions.Length;
		
	}
}
