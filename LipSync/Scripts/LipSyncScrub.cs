using UnityEngine;
using System.Collections;

[RequireComponent(typeof(LipSyncMain))]

public class LipSyncScrub : MonoBehaviour {

	public Animator animator;
	public string clipName = "SpriteAnimation";
	public int clipLayer = 0;
	public int goToFrame = 12;
	public int totalFrames = 30;
	public int fps = 30;

	public float motionScale = 10.0f;
	public float rangeMinimum = 0.0f;
	public float rangeMaximum = 1.0f;

	private LipSyncMain lipSyncMain;

	public void Awake() {
		lipSyncMain = GetComponent<LipSyncMain>();
	}
	
	public void Start() {
		if (!animator) animator = GetComponent<Animator>();
	}

	public void LateUpdate() {
		float val;
		float rng = rangeMaximum - rangeMinimum;
		val = rng * lipSyncMain.volume * motionScale + rangeMinimum;
		animator.Play(clipName, clipLayer, val);
	}

}