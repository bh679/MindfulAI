using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using BrennanHatton.AI;

//You are an autum forrest. You are gold, and orange and in the middle of autum. You are a ground full of leaves, you thuck tree trunks. And a human wants you to repsond to the folloing

[System.Serializable]
public class Painting
{
	public Sprite image;
	public string personality;
}

public class ArtPromptManager : MonoBehaviour
{
	public MsCogVoiceToGPT GPTGen;
	public ReadResponse responseReader;
	public SpriteRenderer pictureChanger;
	
	
	public int paintingId = 0, questionId = 0, questionsToAsk = 5, wisdomToOffer = 2;
	
	public Painting[] painting;
	public string instructions = ". A human wants you to repsond to the folloing";
	public string Questions,
		Wisdom;
	public string[] Instruction;
	public string languagePrompt;
	
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
		if(questionId < questionsToAsk)
			GPTGen.promptWrapper.prePrompt = painting[paintingId].personality + instructions + Questions + languagePrompt;
		else if(questionId < questionsToAsk+wisdomToOffer)
			GPTGen.promptWrapper.prePrompt = painting[paintingId].personality + instructions + Wisdom + languagePrompt;
		else
		{
			GPTGen.promptWrapper.prePrompt = painting[paintingId].personality + instructions + languagePrompt;
			responseReader.wrapper.postPrompt = Instruction[Random.Range(0,Instruction.Length)];
		}
		
		questionId ++;
	}
	
	public void OnAudioFinished()
	{
		if(questionId > questionsToAsk+wisdomToOffer)
		{
			paintingId++;
			questionId = 0;
			
			pictureChanger.sprite = painting[paintingId % painting.Length].image;
			pictureChanger.gameObject.SetActive(true);
			responseReader.wrapper.postPrompt = "";
		}
	}
}
