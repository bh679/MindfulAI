using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class PictureTransition : MonoBehaviour
{
	public float delay = 2f;
	public float timeToScale = 5f;
	public Vector3 targetScale;
    
	public SpriteRenderer background, mySprite;
    
	void OnEnable()
	{
		this.transform.localScale = Vector3.zero;
		targetScale = background.transform.localScale;
		
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
		
		background.sprite = mySprite.sprite;
		this.gameObject.SetActive(false);
	}
	
	// Update is called once per frame
    void Update()
    {
        
    }
}
