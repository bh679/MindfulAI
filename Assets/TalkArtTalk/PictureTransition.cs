using System.Collections;
using System.Collections.Generic;
using UnityEngine.Video;
using UnityEngine;

public class PictureTransition : MonoBehaviour
{
	public float delay = 2f;
	public float timeToScale = 5f;
	public Vector3 targetScale;
	Texture2D texture;
	public VideoPlayer videoPlayer;
	public AudioSource audioSource;
    
	//public SpriteRenderer background, mySprite;
	public MeshRenderer mainPainting, myMeshRenderer;
    
	void Reset()
	{
		videoPlayer = this.GetComponent<VideoPlayer>();
	}
	
    
	public void SetVideo(string url)
	{
		StartCoroutine(DownloadAndPlayVideo(url));
	}
	
	private IEnumerator DownloadAndPlayVideo(string videoUrl)
	{
		// Download video
		using (var www = new UnityEngine.Networking.UnityWebRequest(videoUrl, UnityEngine.Networking.UnityWebRequest.kHttpVerbGET))
		{
			www.downloadHandler = new UnityEngine.Networking.DownloadHandlerBuffer();
			yield return www.SendWebRequest();

			if (www.result == UnityEngine.Networking.UnityWebRequest.Result.ConnectionError || www.result == UnityEngine.Networking.UnityWebRequest.Result.ProtocolError)
			{
				Debug.LogError("Error downloading video: " + www.error);
			}
			else
			{
				// Setup VideoPlayer
				videoPlayer.source = VideoSource.Url;
				videoPlayer.url = www.url;
				videoPlayer.renderMode = VideoRenderMode.CameraFarPlane;
				videoPlayer.audioOutputMode = VideoAudioOutputMode.AudioSource;
				videoPlayer.SetTargetAudioSource(0, audioSource);

				// Play video
				videoPlayer.Prepare();
				videoPlayer.prepareCompleted += PrepareCompleted;
				videoPlayer.loopPointReached += LoopPointReached;
			}
		}
	}

	private void PrepareCompleted(VideoPlayer source)
	{
		source.Play();
		audioSource.Play();
	}

	private void LoopPointReached(VideoPlayer source)
	{
		source.Stop();
		audioSource.Stop();
	}
    
	public void SetTexture(Texture2D _texture)
	{
		texture = _texture;
		myMeshRenderer.material.mainTexture = texture;
		
		mainPainting.transform.localScale = new Vector3(
			mainPainting.transform.localScale.y*texture.width/texture.height,
			mainPainting.transform.localScale.y,
			mainPainting.transform.localScale.z);
		
		
		targetScale = mainPainting.transform.localScale;
		
	}
    
	void OnEnable()
	{
		this.transform.localScale = Vector3.zero;
		targetScale = mainPainting.transform.localScale;
		
		StartCoroutine(_scale());
	}

	
	IEnumerator _scale()
	{
		yield return new WaitForSeconds(delay);
		
		float complete = 0;
		float tikTime = 0.03f;
		
		while(complete < timeToScale)
		{
			yield return new WaitForSeconds(tikTime);
			
			complete += tikTime;//timeToScale;
			
			this.transform.localScale = targetScale*complete/timeToScale;
		}
		
		mainPainting.material.mainTexture = texture;
		this.gameObject.SetActive(false);
	}
	
	// Update is called once per frame
    void Update()
    {
        
    }
}
