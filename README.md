# DoGPT - GPT for Dogs

A smart phone app using spech-to-text, open ai's GPT3 and text-to-speech to allow you to talk to your pets and have the ai respond on their behalf.

A small project I built over a weekend inspired by an idea that came up at my birthday party. 
What if we could use ai to speak to dogs?

I had a hypothesis that we could use ai to fake that pretty convincingly. 
The idea was to build an app you could run on the collar of a dog, and use to pretend you can speak to your dog, but really your speaking to the ai, who is pretending to be your dog. You speak with your voice, it speaks back to your in an ai-generated voice with ai-generated responses.
It was actually pretty easy to get working, only took a day to do.

human voice -> text -> promp -> GPT -> text result -> ai voice.

Its working, but GPT isn't as good at pending to be a dog as I thought it would be. GPT is usually rather good at role-playing. One of the things I think it does best, so maybe I just need to work the prompt a bit more. So far it si mostly just saying "woof" to what I say to it, or acting too human. hahahaha

## Latest Builds
[Google Drive](https://drive.google.com/drive/folders/1ECG7gGsVpLXEzRe00n2ucfhqfAdXYm4Z?usp=sharing)

## Requirements
[BrennanHattons Unity Tools](https://github.com/bh679/Unity-Tools) <br />
MS Cognative Services API Key<br />
OpenAI API Key<br />


## Pre-Installed <br />
[MS Congantive Services](https://github.com/Azure-Samples/cognitive-services-speech-sdk/blob/master/quickstart/csharp/unity/text-to-speech/README.md) <br />
[MSTextToSpeech](https://github.com/ActiveNick/Unity-Text-to-Speech/tree/master/Assets) <br />

## [Youtube Demo](https://youtu.be/1si3kkHxp9U)

[![Demo of DOGPT 0.0.3](https://user-images.githubusercontent.com/2542558/219532489-f9347879-f4d8-43c3-9e49-b316e0352fd1.png)](https://youtu.be/1si3kkHxp9U)


## Installation
 - Clone
 - Adding Unity Tools
 - Get API keys & reference in scene.
 - Import TMP Escentials. 

### Common Issues
 - If using MacOS, you may need to grant secuirty permissions before MS Cog can run.
<img width="300" alt="Screenshot 2023-02-24 at 2 22 35 pm" src="https://user-images.githubusercontent.com/2542558/221084769-837dafc3-34ad-40c9-b99f-2a4cf796fd52.png">
TO do this, click the ? and follow the instructions to allow permissions in Secuirty and Privacy.

- If you are getting this pop-up, be sure to import escentials
<img width="664" alt="Screenshot 2023-02-24 at 2 42 29 pm" src="https://user-images.githubusercontent.com/2542558/221086735-8f14eb3d-07fe-48c9-92d4-a7eaaae34526.png">
