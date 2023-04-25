using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.Networking;
using BrennanHatton.AI;

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
	
	//public Sprite Image {get; set;}
	
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
	
	public int paintingId = 0, questionId = 0;
	
	//public PaintingGroup[] paintingGroups;
	public GalleryData galleryData;
	public int groupId;
	
	public string[] Instruction;
	public string languagePrompt;
	
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
		for(int i = 0; i < galleryData.paintingGroups.Length; i++)
		{
			galleryData.paintingGroups[i].Setup();
			
			for(int p = 0; p < galleryData.paintingGroups[i].paintings.Length; p++)
			{
				LoadImage(galleryData.paintingGroups[i].paintings[p]);
			}
		}
		
		while(galleryData.paintingGroups[groupId].paintings.Length == 0)
			groupId = (groupId + 1) % galleryData.paintingGroups.Length;
			
		GPTGen.promptWrapper.prePrompt = CurrentPainting.personality + galleryData.instructions + galleryData.Questions + languagePrompt;
	
		responseReader.wrapper.postPrompt = "";
		pictureChanger.SetTexture(CurrentPainting.Texture);//.background.sprite = CurrentPainting.Image;
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
		
		/*WWW www = new WWW(painting.imageUrl);
		yield return www;
		
		Texture2D texture = new Texture2D(64,64);*/
		/*Debug.Log(www);
		Debug.Log(www.text);
		Debug.Log(www.Current);
		Debug.Log(www.bytes);
		Debug.Log(www.texture);*/
		//www.LoadImageIntoTexture(texture as Texture2D);
		
		UnityWebRequest www = UnityWebRequestTexture.GetTexture(painting.imageUrl);
		yield return www.SendWebRequest();

		if (www.isNetworkError || www.isHttpError)
		{
			Debug.LogError(www.error + painting.imageUrl);
		}
		else
		{
			Texture2D texture = ((DownloadHandlerTexture)www.downloadHandler).texture;
			//Sprite sprite = Sprite.Create(texture, new Rect(0,0, texture.width, texture.height), new Vector2());
			painting.Texture  = texture;
			Debug.Log("Success " + texture.width + ":" + texture.height);
		}
	
		//	painting.image = Sprite.Create(www.texture, new Rect(0, 0, www.texture.width, www.texture.height), new Vector2(0, 0));
	
		Debug.Log(painting.Texture);
	}
}

