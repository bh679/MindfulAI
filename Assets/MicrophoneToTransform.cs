using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.Audio;


public class MicrophoneToTransform : MonoBehaviour
{

    // Update is called once per frame
    void Update()
	{
		for(int i  = 0; i < Microphone.devices.Length; i++)
		{
			if(Microphone.IsRecording(Microphone.devices[i]))
			{
			
				float y = Microphone.GetPosition(Microphone.devices[i]);
				Debug.Log(y);
				this.transform.localScale = new Vector3(1,y,1);
				
				return;
			}
		}
    }
}
