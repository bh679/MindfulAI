using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using BrennanHatton.AI;

//You are an autum forrest. You are gold, and orange and in the middle of autum. You are a ground full of leaves, you thuck tree trunks. And a human wants you to repsond to the folloing

[System.Serializable]
public class Painting
{
	public string title;
	public Sprite image;
	public string personality;
	public string artist;
	public string description;
}

[System.Serializable]
public class PaintingGroup
{
	public string name;
	public Painting[] paintings;
	int[] paintOrder;
	
	public Painting GetPainting(int i)
	{
		if(paintOrder == null)
			return null;
			
		return paintings[paintOrder[i]];
	}
	
	public void Setup()
	{
		if(paintings == null)
			return;
			
		paintOrder = new int[paintings.Length];
		
		for(int i = 0 ;i < paintOrder.Length; i++)
			paintOrder[i] = i;
		
		paintOrder = Shuffle.ShuffleArray<int>(paintOrder);
	}
}

public class ArtPromptManager : MonoBehaviour
{
	public MsCogVoiceToGPT GPTGen;
	public ReadResponse responseReader;
	public PictureTransition pictureChanger;
	
	public int paintingId = 0, questionId = 0, questionsToAsk = 5, wisdomToOffer = 2;
	
	public PaintingGroup[] paintingGroups;
	public int groupId;
	public string instructions = ". A human wants you to repsond to the folloing";
	public string Questions,
		Wisdom;
	public string[] Instruction;
	public string languagePrompt;
	
	Painting CurrentPainting
	{
		get{
			return paintingGroups[groupId].GetPainting(paintingId);
		}
	}
	
	void Reset()
	{
		GPTGen = this.GetComponent<MsCogVoiceToGPT>();
	}
	
	void Start()
	{
		for(int i = 0; i < paintingGroups.Length; i++)
			paintingGroups[i].Setup();
		
		
		while(paintingGroups[groupId].paintings.Length == 0)
			groupId = (groupId + 1) % paintingGroups.Length;
			
		GPTGen.promptWrapper.prePrompt = CurrentPainting.personality + instructions + Questions + languagePrompt;
	
		responseReader.wrapper.postPrompt = "";
		pictureChanger.background.sprite = CurrentPainting.image;
	}
	
	public void AskQuestion()
	{
		if(questionId < questionsToAsk)
			GPTGen.promptWrapper.prePrompt = CurrentPainting.personality + instructions + Questions + languagePrompt;
		else if(questionId < questionsToAsk+wisdomToOffer)
			GPTGen.promptWrapper.prePrompt = CurrentPainting.personality + instructions + Wisdom + languagePrompt;
		else
		{
			GPTGen.promptWrapper.prePrompt = CurrentPainting.personality + instructions + languagePrompt;
			responseReader.wrapper.postPrompt = Instruction[Random.Range(0,Instruction.Length)];
		}
		
		questionId ++;
	}
	
	public void OnAudioFinished()
	{
		if(questionId > questionsToAsk+wisdomToOffer)
		{
			NextPicture();
			
		}
	}
	
	public void NextPicture()
	{
		paintingId++;// = (paintingId + 1) % paintingGroups[groupId].Length;
		if(paintingId >= paintingGroups[groupId].paintings.Length)
		{
			groupId = (groupId + 1) % paintingGroups.Length;
			
			while(paintingGroups[groupId].paintings.Length == 0)
				groupId = (groupId + 1) % paintingGroups.Length;
				
			paintingId = 0;
		}
		
		questionId = 0;
			
		pictureChanger.mySprite.sprite = CurrentPainting.image;
		pictureChanger.gameObject.SetActive(true);
		responseReader.wrapper.postPrompt = "";
	}
	
	public void PreviousPicture()
	{
		//Debug.Log("PreviousPicture");
		paintingId--;// % painting.Length;
		if(paintingId < 0)
		{
			
			groupId = (groupId - 1);
			if(groupId < 0)
				groupId = paintingGroups.Length - 1;
			
			while(paintingGroups[groupId].paintings.Length == 0)
			{
				groupId = (groupId - 1);// % paintingGroups.Length;
				if(groupId < 0)
					groupId = paintingGroups.Length - 1;
			}
			paintingId = paintingGroups[groupId].paintings.Length - 1;
		}
			
		questionId = 0;
			
		pictureChanger.mySprite.sprite = CurrentPainting.image;
		pictureChanger.gameObject.SetActive(true);
		responseReader.wrapper.postPrompt = "";
	}
}
