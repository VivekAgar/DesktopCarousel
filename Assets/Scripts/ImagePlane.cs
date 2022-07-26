using UnityEngine;
using System.Collections;

public class ImagePlane : MonoBehaviour {


	public Texture imageTexture;

	// Use this for initialization
	void Start () {
		renderer.material.mainTexture = imageTexture;

	}
	
	// Update is called once per frame
	void Update () {
	
	}
}
