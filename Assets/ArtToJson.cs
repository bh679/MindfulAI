using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class ArtToJson : MonoBehaviour
{
	public string json;
	public ArtPromptManager artManager;
	
	void Reset()
	{
		artManager = this.GetComponent<ArtPromptManager>();
		
		json = JsonUtility.ToJson(artManager.galleryData, true);
	}
}
