using UnityEngine;
using System.Collections;

[RequireComponent(typeof(LipSyncMain))]

public class LipSyncRotate : MonoBehaviour {

	//[ExecuteInEditMode]
	public Transform[] mouth;
	public float[] inputScale;
	public float motionScale = 10.0f;
	public float rangeMinimum = 0.0f;
	public float rangeMaximum = 1.0f;
	//public Vector3 offset = new Vector3(0.0f,0.0f,0.0f);
	public enum JawAxis {X, Y, Z};
	public JawAxis jawAxis = JawAxis.X;

	private LipSyncMain lipSyncMain;
	private Vector3[] origRot;

	public void Awake() {
		lipSyncMain = GetComponent<LipSyncMain>();
	}
	
	public void Start() {
		if (!mouth[0]) mouth[0] = transform;
		if (inputScale.Length < 1) {
			inputScale = new float[mouth.Length];
			for (int i=0; i<inputScale.Length; i++) {
				inputScale[i] = 1.0f;
			}
		}
		origRot = new Vector3[mouth.Length];
		for (int i=0; i<mouth.Length; i++) {
			origRot[i] = mouth[i].localEulerAngles;
		}
	}

	public void LateUpdate() {
		float val;
		float rng = rangeMaximum - rangeMinimum;
		val = rng * lipSyncMain.volume * motionScale + rangeMinimum;

		for (int i=0; i<mouth.Length; i++) {
			switch(jawAxis){
				case JawAxis.X:
					//val += origRot.x + offset.x;
					val += origRot[i].x;
					mouth[i].localEulerAngles = new Vector3(val * inputScale[i], mouth[i].localEulerAngles.y, mouth[i].localEulerAngles.z);
					break;
				case JawAxis.Y:
					//val += origRot.y + offset.y;
					val += origRot[i].y;
					mouth[i].localEulerAngles = new Vector3(mouth[i].localEulerAngles.x, val * inputScale[i], mouth[i].localEulerAngles.z);
					break;
				case JawAxis.Z:
					//val += origRot.z + offset.z;
					val += origRot[i].z;
					mouth[i].localEulerAngles = new Vector3(mouth[i].localEulerAngles.x, mouth[i].localEulerAngles.y, val * inputScale[i]);
					break;
			}
		}
	}

}