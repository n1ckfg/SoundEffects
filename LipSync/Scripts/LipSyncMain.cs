using UnityEngine;
using System.Collections;

[RequireComponent (typeof(AudioSource))]

public class LipSyncMain : MonoBehaviour {

	[HideInInspector]
	public float volume;
	public enum Smoothing { Noisiest = 0, Noisier, Normal, Smoother, Smoothest };
	public Smoothing smoothing = Smoothing.Normal;
	public bool useMicrophone = false;
	public bool debug = false;
	public int micNumber = 0;
	public int micSampleRate = 44100;
	public int delay = 1;

	public float noiseFloor = 0.0f;
	public float noiseCeiling = 1.0f;

	private int winWidth = 1024*8;
	private	 float[] samples;

	public void Start() {
		SetSmoothing(smoothing);
		if (useMicrophone){
			GetComponent<AudioSource>().playOnAwake = false;
			GetComponent<AudioSource>().loop = true;
			if (delay < 1) Debug.LogError ("Microphone Delay must be at least 1");
			//audio.clip = Microphone.Start("", true, delay, micSampleRate);
			if (Microphone.devices.Length > 0 && Microphone.devices.Length > micNumber){
				GetComponent<AudioSource>().clip = Microphone.Start(Microphone.devices[micNumber], true, delay, micSampleRate);
				//*** this is necessary or you get extreme latency problems ***
				while(Microphone.GetPosition(GetComponent<AudioSource>().name) <= 0);
			}
			GetComponent<AudioSource>().Play();
		}
	}

	public void SetSmoothing(Smoothing s) {
		smoothing = s;
		switch(s){
			case Smoothing.Noisiest:
				winWidth = 1024;
				break;
			case Smoothing.Noisier:
				winWidth = 1024*2;
				break;
			case Smoothing.Normal:
				winWidth = 1024*4;
				break;
			case Smoothing.Smoother:
				winWidth = 1024*8;
				break;
			case Smoothing.Smoothest:
				winWidth = 1024*16;
				break;
		}
		samples = new float[winWidth];
	}

	public void Update() {
		//if (!audio.isPlaying){
		if (GetComponent<AudioSource>() == null) {
			volume = 0.0f;
		} else {
			float min = 10000000.0f;
			float max = -10000000.0f;
			GetComponent<AudioSource>().GetOutputData (samples, 0);
			
			float average = 0.0f;
			for (int i = 0; i < winWidth; i++){
				float abs = Mathf.Abs(samples[i]);
				if (abs < min)
					min = abs;
				if (abs > max)
					max = abs;
				average += abs;
			}
			average /= winWidth;

			if (average < noiseFloor) {
				volume = Mathf.Lerp(average, 0.0f, noiseFloor);
			} else if (average > noiseCeiling) {
				volume = noiseCeiling;
			} else {
				volume = average;
			}

			if (debug) Debug.Log("volume: " + volume);
		}
	}

}