using UnityEngine;
using System.Collections;

[RequireComponent(typeof(LipSyncMain))]

public class LipSyncTranslate : MonoBehaviour {

	public Transform[] mouth;
	public float[] inputScale;
	public float motionScale = 10.0f;
	public Vector3 rangeMinimum = new Vector3(0.0f,0.0f,0.0f);
	public Vector3 rangeMaximum = new Vector3(0.0f,-1.0f,0.0f);

	private LipSyncMain lipSyncMain;
	private Vector3[] origPos;

	public void Start(){
		lipSyncMain = GetComponent<LipSyncMain>();
		if (!mouth[0]) mouth[0] = transform;
		if (inputScale.Length < 1) {
			inputScale = new float[mouth.Length];
			for (int i=0; i<inputScale.Length; i++) {
				inputScale[i] = 1.0f;
			}
		}		
		origPos = new Vector3[mouth.Length];
		for (int i=0; i<mouth.Length; i++) {
			origPos[i] = mouth[i].localPosition;
		}
	}

	public void LateUpdate(){
		Vector3 val;
		Vector3 rng = rangeMaximum - rangeMinimum;
		val = rng * lipSyncMain.volume * motionScale + rangeMinimum;

		for (int i=0; i<mouth.Length; i++) {
			mouth[i].localPosition = origPos[i] + (val * inputScale[i]);
		}
	}

}