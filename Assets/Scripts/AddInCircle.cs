using UnityEngine;
using System.Collections;

public class AddInCircle : MonoBehaviour {


	public GameObject prefab;
	public int numberOfObjects = 20;
	public float radius = 5f;
	
	void Start() {
		for (int i = 0; i < numberOfObjects; i++) {
			float angle = i * Mathf.PI * 2 / numberOfObjects;
			Vector3 pos = new Vector3(Mathf.Cos(angle), 0, Mathf.Sin(angle)) * radius;
			//Instantiate(prefab, pos, Quaternion.Euler(new Vector3(-90.0f,0.0f.0.0f)));
		}
	}
	
	// Update is called once per frame
	void Update () {
	
	}
}
