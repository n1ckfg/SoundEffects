// http://www.develop-online.net/tools-and-tech/procedural-audio-with-unity/0117433
// https://www.youtube.com/watch?v=GqHFGMy_51c

using UnityEngine;
using System;  // Needed for Math

public class Oscillator : MonoBehaviour {

    public enum SynthMode { SINE, SQUARE, TRI, NOISE };
    public SynthMode synthMode = SynthMode.SINE;
    public double frequency = 440;
    public double gain = 0.05;

    private double increment;
    private double phase;
    private double sampling_frequency = 48000;

    private System.Random RandomNumber = new System.Random();
    private float offset = 0;

    void OnAudioFilterRead(float[] data, int channels) {
        // update increment in case frequency has changed
        increment = frequency * 2 * Math.PI / sampling_frequency;

        for (int i = 0; i < data.Length; i += channels) {
            phase += increment;

            // this is where we copy audio data to make them “available” to Unity
            if (synthMode == SynthMode.SINE) {
                data[i] = genSine();
            } else if (synthMode == SynthMode.SQUARE) {
                data[i] = genSquare();
            } else if (synthMode == SynthMode.TRI) {
                data[i] = genTri();
            } else if (synthMode == SynthMode.NOISE) {
                data[i] = genNoise();
            }

            // if we have stereo, we copy the mono data to each channel
            if (channels == 2) data[i + 1] = data[i];
            if (phase > 2 * Math.PI) phase = 0;
        }
    }

    float genSine() {
        return (float)(gain * Math.Sin((float)phase));
    }

    float genSquare() {
        if (gain * Mathf.Sin((float)phase) >= 0 * gain) {
            return (float)gain * 0.6f;
        } else {
            return (-(float)gain) * 0.6f;
        }
    }

    float genTri() {
        return (float)(gain * (double)Mathf.PingPong((float)phase, 1.0f));
    }

    float genNoise() {
        return (float)gain * (offset - 1.0f + (float)RandomNumber.NextDouble() * 2.0f);
    }

}