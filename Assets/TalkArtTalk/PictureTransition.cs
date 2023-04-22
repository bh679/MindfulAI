using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class PictureTransition : MonoBehaviour
{
	public float delay = 2f;
	public float timeToScale = 5f;
	public Vector3 targetScale;
	Texture2D texture;
    
	//public SpriteRenderer background, mySprite;
	public MeshRenderer mainPainting, myMeshRenderer;
    
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
