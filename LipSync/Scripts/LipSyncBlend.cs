using UnityEngine;
using System.Collections;

[RequireComponent(typeof(LipSyncMain))]

public class LipSyncBlend : MonoBehaviour {

	public SkinnedMeshRenderer mouth;
	public int blendShapeNum = 0;
	public float motionScale = 10.0f;
	public float rangeMinimum = 0.0f;
	public float rangeMaximum = 1.0f;

	private LipSyncMain lipSyncMain;

	public void Awake() {
		lipSyncMain = GetComponent<LipSyncMain>();
	}

	public void Start() {
		if (!mouth) mouth = GetComponent<SkinnedMeshRenderer>();
	}

	public void LateUpdate() {
		float val;
		float rng = rangeMaximum - rangeMinimum;
		val = rng * lipSyncMain.volume * motionScale + rangeMinimum;
		mouth.SetBlendShapeWeight(blendShapeNum, val * 100.0f);
	}

}