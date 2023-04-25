using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.Networking;

//http://13.237.241.168/Go/GalleryJson
//http://13.237.241.168/wp-content/uploads/2023/04/Gallery.json

public class RemoteJsonToArt : MonoBehaviour
{
	public string url;
	public ArtPromptManager artManager;
	void Reset ( )
	{
		artManager = this .GetComponent<ArtPromptManager> () ;
	}
	
    // Start is called before the first frame update
    void Start()
    {
	    StartCoroutine(GetText());
    }
    
	IEnumerator GetText() {
		UnityWebRequest www = UnityWebRequest.Get(url);
		yield return www.SendWebRequest();
 
		if (www.result != UnityWebRequest.Result.Success) {
			Debug.Log(www.error);
		}
		else {
			// Show results as text
			Debug.Log(www.downloadHandler.text);
 
			// Or retrieve results as binary data
			//byte[] results = www.downloadHandler.data;
			
			artManager.galleryData = JsonUtility.FromJson<GalleryData>(www.downloadHandler.text);
			artManager.Setup();
		}
	}
}
