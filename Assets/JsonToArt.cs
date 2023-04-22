using System.Collections;
using System.Collections.Generic;
using UnityEngine;


public class JsonToArt : MonoBehaviour
{
	public TextAssetBehaviour textAssBev;
	public TextAsset textAsset;
	public ArtPromptManager artManager;
	void Reset ( )
	{
		artManager = this .GetComponent<ArtPromptManager> () ;
		textAssBev = this. GetComponent<TextAssetBehaviour> ( ) ;
		if(textAssBev != null && textAssBev.textAsset != null)
		{
			textAsset = textAssBev.textAsset;
			ReadData();
		}
		
	}
	
	void ReadData()
	{
		artManager.galleryData = JsonUtility.FromJson<GalleryData>(textAsset.text);
	}
			
			// Start is called before the first frame update 
	void Start ( )
	{
		ReadData();
	}
	
	// Update is called once per frame
	void Update()
	{
        
	}
}