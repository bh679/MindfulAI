using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.Networking;
using BrennanHatton.AI;
using UnityEngine.Video;

//You are an autum forrest. You are gold, and orange and in the middle of autum. You are a ground full of leaves, you thuck tree trunks. And a human wants you to repsond to the folloing

[System.Serializable]
public class GalleryData
{
	public PaintingGroup[] paintingGroups;
	public int questionsToAsk = 1, wisdomToOffer = 2;
	
	
	public string instructions = ". A human wants you to repsond to the folloing";
	public string Questions,
		Wisdom;
}

[System.Serializable]
public class Painting
{
	public string title;
	//Sprite image;
	public string imageUrl;
	public string personality;
	public string artist;
	public string description;
	Texture2D texture; 
	public Texture2D Texture {get; set;} 
	
	bool videoSet = false, isVideo;
	
	//public Sprite Image {get; set;}
	public bool IsVideo {
		get 
		{
			if(!videoSet)
				return SetIsVideo();
				
			return isVideo;
		}
	}
	
	
	bool SetIsVideo()
	{
		videoSet = true;
		string[] str  = imageUrl.ToLower().Split(".");
		
		Debug.Log(str[1]);
		
		if(str[1] == "mp4")
			return true;
			
		return false;
	}
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
		Debug.Log("Setup");
		if(paintings == null)
			return;
			
		Debug.Log("Setup");
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
	
	public int paintingId = 0, questionId = 0;
	
	public GalleryData galleryData;
	public int groupId;
	
	public string[] Instruction;
	public string languagePrompt;
	public string valuesPrompt;
	
	public DropDownToMSCogLanugage lanugage;
	
	Painting CurrentPainting
	{
		get{
			return galleryData.paintingGroups[groupId].GetPainting(paintingId);
		}
	}
	
	void Reset()
	{
		GPTGen = this.GetComponent<MsCogVoiceToGPT>();
	}
	
	public void Setup()
	{
		Debug.Log("Setup");
		for(int i = 0; i < galleryData.paintingGroups.Length; i++)
		{
			Debug.Log("Setup");
			galleryData.paintingGroups[i].Setup();
			
			for(int p = 0; p < galleryData.paintingGroups[i].paintings.Length; p++)
			{
				LoadImage(galleryData.paintingGroups[i].paintings[p]);
			}
		}
		
		while(galleryData.paintingGroups[groupId].paintings.Length == 0)
			groupId = (groupId + 1) % galleryData.paintingGroups.Length;
			
		GPTGen.promptWrapper.prePrompt = CurrentPainting.personality  + valuesPrompt + "The artist is " + CurrentPainting.artist + galleryData.instructions + galleryData.Questions + languagePrompt;
	
		responseReader.wrapper.postPrompt = "";
		pictureChanger.SetTexture(CurrentPainting.Texture);//.background.sprite = CurrentPainting.Image;
		LoadImage(CurrentPainting);
	}
	
	public void AskQuestion()
	{
		if(questionId < galleryData.questionsToAsk)
			GPTGen.promptWrapper.prePrompt = CurrentPainting.personality + galleryData.instructions + galleryData.Questions + languagePrompt;
		else if(questionId < galleryData.questionsToAsk+galleryData.wisdomToOffer)
			GPTGen.promptWrapper.prePrompt = CurrentPainting.personality + galleryData.instructions + galleryData.Wisdom + languagePrompt;
		else
		{
			GPTGen.promptWrapper.prePrompt = CurrentPainting.personality + galleryData.instructions + languagePrompt;
			responseReader.wrapper.postPrompt = Instruction[Random.Range(0,Instruction.Length)];
		}
		
		questionId ++;
	}
	
	public void OnAudioFinished()
	{
		if(questionId > galleryData.questionsToAsk+galleryData.wisdomToOffer)
		{
			NextPicture();
			
		}
	}
	
	public void NextPicture()
	{
		paintingId++;// = (paintingId + 1) % paintingGroups[groupId].Length;
		if(paintingId >= galleryData.paintingGroups[groupId].paintings.Length)
		{
			groupId = (groupId + 1) % galleryData.paintingGroups.Length;
			
			while(galleryData.paintingGroups[groupId].paintings.Length == 0)
				groupId = (groupId + 1) % galleryData.paintingGroups.Length;
				
			paintingId = 0;
		}
		
		questionId = 0;
			
		Debug.Log(CurrentPainting);
		Debug.Log(CurrentPainting.Texture);
		Debug.Log(pictureChanger);
		
		if(CurrentPainting.IsVideo)
			pictureChanger.SetVideo(CurrentPainting.imageUrl);
		else
			pictureChanger.SetTexture(CurrentPainting.Texture);//pictureChanger.mySprite.sprite = CurrentPainting.Image;
		pictureChanger.gameObject.SetActive(true);
		responseReader.wrapper.postPrompt = "";
		
		lanugage.ChangeVoice(paintingId);
	}
	
	public void PreviousPicture()
	{
		//Debug.Log("PreviousPicture");
		paintingId--;// % painting.Length;
		if(paintingId < 0)
		{
			
			groupId = (groupId - 1);
			if(groupId < 0)
				groupId = galleryData.paintingGroups.Length - 1;
			
			while(galleryData.paintingGroups[groupId].paintings.Length == 0)
			{
				groupId = (groupId - 1);// % paintingGroups.Length;
				if(groupId < 0)
					groupId = galleryData.paintingGroups.Length - 1;
			}
			paintingId = galleryData.paintingGroups[groupId].paintings.Length - 1;
		}
			
		questionId = 0;
			
		pictureChanger.SetTexture(CurrentPainting.Texture);
		//pictureChanger.mySprite.sprite = CurrentPainting.Image;
		pictureChanger.gameObject.SetActive(true);
		responseReader.wrapper.postPrompt = "";
	}
	
	public void LoadImage(Painting painting)
	{
		StartCoroutine(_loadImage(painting));
	}
	  
	IEnumerator _loadImage(Painting painting) {
		
		Debug.Log(painting.imageUrl);
		
		if(painting.IsVideo == false)
		{
		
			UnityWebRequest www = UnityWebRequestTexture.GetTexture(painting.imageUrl);
			yield return www.SendWebRequest();
	
			if (www.isNetworkError || www.isHttpError)
				Debug.LogError(www.error + painting.imageUrl);
			else
			{
				Texture2D texture = ((DownloadHandlerTexture)www.downloadHandler).texture;
				painting.Texture  = texture;
				Debug.Log("Success " + texture.width + ":" + texture.height);
			}
		
			Debug.Log(painting.Texture);
		}
	}
}

