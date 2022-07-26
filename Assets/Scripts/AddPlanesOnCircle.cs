using System;
using UnityEngine;
using System.Collections;
using System.Collections.Generic;
using System.Linq;

public class AddPlanesOnCircle : MonoBehaviour {

	public int m_noOf_images = 8;
	private float m_anglePerObject;
	public GameObject frameprefab;
	public float radius = 5;
	public Camera cam;

	public static ArrayList elementsList;
	private bool loadingDone = false;
	List<ImagePlaneData>		m_imagePlanesDataList = new List<ImagePlaneData>();
	/*
	GameObject plane = GameObject.CreatePrimitive(PrimitiveType.Plane);
	GameObject cube = GameObject.CreatePrimitive(PrimitiveType.Cube);
	cube.transform.position = new Vector3(0, 0.5F, 0);
	GameObject sphere = GameObject.CreatePrimitive(PrimitiveType.Sphere);
	sphere.transform.position = new Vector3(0, 1.5F, 0);
	GameObject capsule = GameObject.CreatePrimitive(PrimitiveType.Capsule);
	capsule.transform.position = new Vector3(2, 1, 0);
	GameObject cylinder = GameObject.CreatePrimitive(PrimitiveType.Cylinder);
	cylinder.transform.position = new Vector3(-2, 1, 0);
	*/

	// Use this for initialization
	void Start () {

		m_imagePlanesDataList.Clear();
		elementsList = new ArrayList();
		elementsList.Clear();
		Debug.Log ("Loading ...");
		//Fetch and Parse
		StartCoroutine(GetSelfieFromServer() );

	}
	
	// Update is called once per frame
	void Update () {
	
		foreach(GameObject g in elementsList )
		{
			//g.transform.LookAt(cam.transform.position,cam.transform.left);
			Quaternion newRotation = Quaternion.LookRotation(cam.transform.position) * Quaternion.Euler(90, 0, 0);
			g.transform.rotation = Quaternion.Slerp(g.transform.rotation, newRotation, (Time.deltaTime * 1.6f));

		}
		if(ImagePlaneData.DownloadCount < (m_noOf_images+1)){

			for (int i = 0; i< elementsList.Count; i++)
			{
				GameObject g = elementsList[i] as GameObject;

				GameObject framePlane = getChildGameObject(g, "pPlane1");
				framePlane.renderer.material.mainTexture = m_imagePlanesDataList[i].SelfieImage;

				//g.renderer.material.mainTexture = m_imagePlanesDataList[i].SelfieImage;
			}

		}



	}

	void OnGUI()
	{

//		if(GUI.Button(new Rect (100,100,200,100), "Load "))
//		{
//			m_imagePlanesDataList.Clear();
//			if(elementsList.Count > 0)
//			{
//				foreach(GameObject g in elementsList )
//				{
//					Destroy(g);
//				}
//				
//			}
//			elementsList.Clear();
//			Debug.Log ("Loading ...");
//			//Fetch and Parse
//			StartCoroutine(GetSelfieFromServer() );
//			ImagePlaneData.DownloadCount = 0;
//
//		}
		if(Input.GetKeyDown(KeyCode.L ))
		{
			m_imagePlanesDataList.Clear();
			Debug.Log ("elementsList.Count "+elementsList.Count);
			if(elementsList.Count > 0)
			{
				for (int i = 0; i< elementsList.Count; i++)
				{

					GameObject g = (GameObject) elementsList[i];
					Destroy(g);
					Debug.Log ("Destroy ....");
				}
				
			}
			elementsList.Clear();
			Debug.Log ("Loading ...");
			//Fetch and Parse
			StartCoroutine(GetSelfieFromServer() );
			Debug.Log ("elementsList.Count "+elementsList.Count);
			ImagePlaneData.DownloadCount = 0;

		}


	}



	void CreatePlanesAndAdd()
	{
		//gameObject.transform.Rotate (350.0f, 0.0f, 0.0f);
		elementsList = new ArrayList();
		float center_x = gameObject.transform.position.x;
		float center_y = gameObject.transform.position.y;
		float center_Z = gameObject.transform.position.z;
		//SphereCollider collider = (SphereCollider) m_SphereGameObject.GetComponent("Collider") ;//as SphereCollider;
		float circleRadius = radius ;//collider.radius;
		float currentAngel = 0;
		
		for(int i = 0; i < m_noOf_images; i++)
		{
			Debug.Log ("Calling For i "+i);
			//GameObject imagePlane = GameObject.CreatePrimitive(PrimitiveType.Plane);
			GameObject imagePlane = (GameObject) Instantiate(frameprefab);//, typeof(GameObject)));
			imagePlane.transform.localScale = new Vector3 (0.00520f, 0.00300f, 0.0040f);
			imagePlane.transform.Rotate (90.0f,180.0f, 0.0f);
			imagePlane.transform.position = new Vector3 ( (center_x + (circleRadius * Mathf.Cos(currentAngel))),
			                                             -2.0f, 
			                                             (center_Z + (circleRadius * Mathf.Sin(currentAngel)))
			                                             );	
			Debug.Log ("transform Position of imagePlane"+imagePlane.transform.position);



			GameObject framePlane = getChildGameObject(imagePlane, "pPlane1");
			framePlane.renderer.material.mainTexture = m_imagePlanesDataList[i].SelfieImage;

			//imagePlane.renderer.material.mainTexture = m_images[i];
			//imagePlane.renderer.material.mainTexture = m_imagePlanesDataList[i].SelfieImage;
			//imagePlane.imageTexture = m_images[i];
			imagePlane.transform.parent = gameObject.transform;	
			imagePlane.transform.Rotate (0, 0.0f, 90 );
			elementsList.Add(imagePlane);
			currentAngel = i * Mathf.PI * 2 / m_noOf_images;
			 
			Debug.Log("currentAngel" +currentAngel);
		}

	}

	IEnumerator GetSelfieFromServer()
	{
		AskServer.GetLatestCarouselData(OncarosuelData, OnErrorInCarouselData);
		yield break;
	}
	void OncarosuelData(Hashtable data)
	{
		if(data.ContainsKey("data")){
			m_noOf_images = Convert.ToInt32(data["data"]);
		}
		Debug.Log("m_noOf_images  "+m_noOf_images);

		ArrayList dataArray = (ArrayList)data["images"];
		m_noOf_images = dataArray.Count;
		//m_noOf_images = 5;
		for(int i = 0; i < dataArray.Count; i++){

			m_imagePlanesDataList.Add(new ImagePlaneData((Hashtable) dataArray[i])); 

		}
		m_anglePerObject = (float) 360 / m_noOf_images;
		CreatePlanesAndAdd();

	}

	void OnErrorInCarouselData(string Error)
	{
		Debug.Log (""+Error );
	}

	static public GameObject getChildGameObject(GameObject fromGameObject, string withName) {
		//Author: Isaac Dart, June-13.
		Transform[] ts = fromGameObject.transform.GetComponentsInChildren<Transform>(true);
		foreach (Transform t in ts) if (t.gameObject.name == withName) return t.gameObject;
		return null;
	}
	
}
