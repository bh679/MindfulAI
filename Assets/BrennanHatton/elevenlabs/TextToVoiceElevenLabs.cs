using System.Collections;
using System.Text;
using UnityEngine;
using UnityEngine.Networking;
using System.IO;

namespace BrennanHatton.AI
{

	public class TextToVoiceElevenLabs : MonoBehaviour
	{
		private string XI_API_KEY = "<xi-api-key>";
		public string VOICE_ID = "21m00Tcm4TlvDq8ikWAM";
		public string model_id = "eleven_multilingual_v1";
		private AudioSource audioSource;
		public TextAsset key;
		public string message = "Your text to be converted to voice";
		public bool runOnStart;
		public bool stream;
	
		private void Start()
		{
			XI_API_KEY = key.text;
			audioSource = gameObject.AddComponent<AudioSource>();
			if(runOnStart)
				StartCoroutine(GetVoiceData(message,stream));
		}
	
		private IEnumerator GetVoiceData(string text, bool _stream = true)
		{
			string url = $"https://api.elevenlabs.io/v1/text-to-speech/{VOICE_ID}" + (_stream?"/stream":"");
	
			UnityWebRequest request = UnityWebRequest.Post(url, "");
			request.SetRequestHeader("accept", (_stream?"*/*":"audio/mpeg"));
			request.SetRequestHeader("xi-api-key", XI_API_KEY);
			request.SetRequestHeader("Content-Type", "application/json");
			
			// Set the request body
			string requestBody = "{\"text\":\"" + text + "\",\"model_id\":\"" + model_id + "\"}";
			byte[] bodyRaw = Encoding.UTF8.GetBytes(requestBody);
			request.uploadHandler = new UploadHandlerRaw(bodyRaw);
	
			yield return request.SendWebRequest();
	
			if (request.result == UnityWebRequest.Result.Success)
			{
				Debug.Log(request.downloadHandler.data);
				Debug.Log(request.uri);
				Debug.Log(request.downloadedBytes);
				
				//audioSource.clip = request.GetAudioClip(request.url.ToString(), AudioType.MPEG);
				//audioSource.Play();
				
				/*
				WWUtils.Audio.WAV wav = new WWUtils.Audio.WAV(request.downloadHandler.data);
				Debug.Log(wav);
				AudioClip audioClip = AudioClip.Create("testSound", wav.SampleCount, 1,wav.Frequency, false, false);
				audioClip.SetData(wav.LeftChannel, 0);
				audioSource.clip = audioClip;
				audioSource.Play();//*/
				
				
				// Get the downloaded data as a byte array
				//*
				byte[] audioData = request.downloadHandler.data;
				
				Debug.Log(audioData.Length);

				// Create a new AudioClip from the byte array
				AudioClip clip = WavUtility.ToAudioClip(audioData);

				Debug.Log(clip);
				// Play the audio clip
				audioSource.clip = clip;
				audioSource.Play();//*/
				
				
				
				/*DownloadHandlerAudioClip handler = new DownloadHandlerAudioClip(request.uri, AudioType.MPEG);

				// Wait for the handler to finish loading the audio clip
				while (!handler.isDone)
				{
					Debug.Log("downlaoding");
					yield return null;
				}

				// Get the AudioClip from the handler
				AudioClip clip = handler.audioClip;

				// Play the audio clip
				audioSource.clip = clip;
				audioSource.Play();
				//isPlaying = true;//*/
				
				
				
				
				/*
				Debug.Log(DownloadHandlerAudioClip.GetContent(request));
				AudioClip audioClip = DownloadHandlerAudioClip.GetContent(request);
				PlayAudioClip(audioClip);//*/
			}
			else
			{
				Debug.Log(request.result);
				Debug.LogError(request.error);
			}
		}
	
		private void PlayAudioClip(AudioClip audioClip)
		{
			if (audioSource != null && audioClip != null)
			{
				audioSource.clip = audioClip;
				audioSource.Play();
			}
		}
	}

}


/*

[
  {
    "model_id": "eleven_monolingual_v1",
    "name": "Eleven Monolingual v1",
    "can_be_finetuned": true,
    "can_do_text_to_speech": true,
    "can_do_voice_conversion": false,
    "token_cost_factor": 1.0,
    "description": "Use our standard English language model to generate speech in a variety of voices, styles and moods.",
    "languages": [
      {
        "language_id": "en",
        "name": "English"
      }
    ]
  },
  {
    "model_id": "eleven_multilingual_v1",
    "name": "Eleven Multilingual v1",
    "can_be_finetuned": true,
    "can_do_text_to_speech": true,
    "can_do_voice_conversion": true,
    "token_cost_factor": 1.0,
    "description": "Generate lifelike speech in multiple languages and create content that resonates with a broader audience.",
    "languages": [
      {
        "language_id": "en",
        "name": "English"
      },
      {
        "language_id": "de",
        "name": "German"
      },
      {
        "language_id": "pl",
        "name": "Polish"
      },
      {
        "language_id": "es",
        "name": "Spanish"
      },
      {
        "language_id": "it",
        "name": "Italian"
      },
      {
        "language_id": "fr",
        "name": "French"
      },
      {
        "language_id": "pt",
        "name": "Portuguese"
      },
      {
        "language_id": "hi",
        "name": "Hindi"
      }
    ]
  }
]

*/

/*
namespace WWUtils.Audio {
	public class WAV  {
 
		// convert two bytes to one float in the range -1 to 1
		static float bytesToFloat(byte firstByte, byte secondByte) {
			// convert two bytes to one short (little endian)
			short s = (short)((secondByte << 8) | firstByte);
			// convert to range from -1 to (just below) 1
			return s / 32768.0F;
		}
 
		static int bytesToInt(byte[] bytes,int offset=0){
			int value=0;
			for(int i=0;i<4;i++){
				value |= ((int)bytes[offset+i])<<(i*8);
			}
			return value;
		}
 
		private static byte[] GetBytes(string filename){
			return File.ReadAllBytes(filename);
		}
		// properties
		public float[] LeftChannel{get; internal set;}
		public float[] RightChannel{get; internal set;}
		public int ChannelCount {get;internal set;}
		public int SampleCount {get;internal set;}
		public int Frequency {get;internal set;}
         
		// Returns left and right double arrays. 'right' will be null if sound is mono.
		public WAV(string filename):
		this(GetBytes(filename)) {}
 
		public WAV(byte[] wav){
             
			// Determine if mono or stereo
			ChannelCount = wav[22];     // Forget byte 23 as 99.999% of WAVs are 1 or 2 channels
 
			// Get the frequency
			Frequency = bytesToInt(wav,24);
             
			// Get past all the other sub chunks to get to the data subchunk:
			int pos = 12;   // First Subchunk ID from 12 to 16
             
			// Keep iterating until we find the data chunk (i.e. 64 61 74 61 ...... (i.e. 100 97 116 97 in decimal))
			while(!(wav[pos]==100 && wav[pos+1]==97 && wav[pos+2]==116 && wav[pos+3]==97)) {
				pos += 4;
				int chunkSize = wav[pos] + wav[pos + 1] * 256 + wav[pos + 2] * 65536 + wav[pos + 3] * 16777216;
				pos += 4 + chunkSize;
			}
			pos += 8;
             
			// Pos is now positioned to start of actual sound data.
			SampleCount = (wav.Length - pos)/2;     // 2 bytes per sample (16 bit sound mono)
			if (ChannelCount == 2) SampleCount /= 2;        // 4 bytes per sample (16 bit stereo)
             
			// Allocate memory (right will be null if only mono sound)
			LeftChannel = new float[SampleCount];
			if (ChannelCount == 2) RightChannel = new float[SampleCount];
			else RightChannel = null;
             
			// Write to double array/s:
			int i=0;
			while (pos < wav.Length) {
				LeftChannel[i] = bytesToFloat(wav[pos], wav[pos + 1]);
				pos += 2;
				if (ChannelCount == 2) {
					RightChannel[i] = bytesToFloat(wav[pos], wav[pos + 1]);
					pos += 2;
				}
				i++;
			}
		}
 
		public override string ToString ()
		{
			return string.Format ("[WAV: LeftChannel={0}, RightChannel={1}, ChannelCount={2}, SampleCount={3}, Frequency={4}]", LeftChannel, RightChannel, ChannelCount, SampleCount, Frequency);
		}
	}
 
}

public static class WavUtility
{
	public static AudioClip ToAudioClip(byte[] wavData)
	{
		int headerSize = 44;
		if (wavData.Length < headerSize)
			return null;

		// Determine if the data is in the correct format
		string riff = System.Text.Encoding.ASCII.GetString(wavData, 0, 4);
		string wave = System.Text.Encoding.ASCII.GetString(wavData, 8, 4);
		if (riff != "RIFF" || wave != "WAVE")
			return null;

		// Get the audio format
		string fmt = System.Text.Encoding.ASCII.GetString(wavData, 12, 4);
		if (fmt != "fmt ")
			return null;

		// Get the format data length
		int formatLength = wavData[16];
		if (formatLength < 16)
			return null;

		// Get the audio format (1 = PCM)
		int audioFormat = wavData[20];
		if (audioFormat != 1)
			return null;

		// Get the number of audio channels
		int channels = wavData[22];

		// Get the sample rate
		int sampleRate = System.BitConverter.ToInt32(wavData, 24);

		// Get the byte rate
		int byteRate = System.BitConverter.ToInt32(wavData, 28);

		// Get the block align
		int blockAlign = wavData[32];

		// Get the bits per sample
		int bitsPerSample = wavData[34];

		// Determine the audio data size
		int dataSize = 0;
		for (int i = 0; i < 4; i++)
			dataSize |= (wavData[headerSize - 4 + i] << (i * 8));

		// Check that the data size is correct
		if (dataSize != wavData.Length - headerSize)
			return null;

		// Create a new AudioClip
		AudioClip audioClip = AudioClip.Create("clip", dataSize / 2, channels, sampleRate, false);

		// Set the audio data
		audioClip.SetData(ExtractAudioData(wavData, headerSize, dataSize), 0);

		return audioClip;
	}

	private static float[] ExtractAudioData(byte[] wavData, int headerSize, int dataSize)
	{
		int bytesPerSample = wavData[34] / 8;
		int sampleCount = dataSize / bytesPerSample;
		float[] audioData = new float[sampleCount];

		int sampleIndex = 0;
		for (int i = headerSize; i < headerSize + dataSize; i += bytesPerSample)
		{
			audioData[sampleIndex++] = BytesToFloat(wavData[i], wavData[i + 1]);
		}

		return audioData;
	}

	private static float BytesToFloat(byte firstByte, byte secondByte)
	{
		short s = (short)((secondByte << 8) | firstByte);
		return s / 32768.0f;
	}
}
//*/
