using System;
using UnityEngine;
using System.Collections;
using System.Collections.Generic;
using System.Linq;


public class ImagePlaneData : MonoBehaviour {

	Texture	m_selfieImageTexture  	= null;
	string	m_contectNo	 			= "";
	float 	m_ratings				= 0.0f;
	int 	m_id					= -1;
	string 	m_downLoadURL			= "";
	static int m_downloadCount = 0;


	public Texture SelfieImage
	{
		get { return m_selfieImageTexture; }
	}

	public string ContectNo {
		get {
			return m_contectNo;
		}
	}

	public float Ratings {
		get {
			return m_ratings;
		}
	}

	public int id {
		get {
			return m_id;
		}
	}

	public string downLoadURL {
		get {
			return m_downLoadURL;
		}
	}

	public static int DownloadCount {
				get {
						return m_downloadCount;
				}
				set {
						m_downloadCount = value;
				}
		}

	// Use this for initialization
	void Start () {
	
		m_selfieImageTexture = Resources.Load("loading") as Texture;


	}
	
	// Update is called once per frame
	void Update () {
	
	}


	void LoadSelfiePicture(string url)
	{
		if (url.Length == 0)
			return;
		Debug.Log ("Downloading Image..");
		AskServer.DowloadImage(url, OnLoadSelfiePicture, OnLoadSelfiePictureFail);
	}
	
	void OnLoadSelfiePicture(Texture2D texture)
	{
		m_selfieImageTexture = texture;
		m_downloadCount++;
	}
	
	void OnLoadSelfiePictureFail(string error)
	{
		Debug.LogWarning("Failed to load profile picture");
	}

	public ImagePlaneData(Hashtable data)
	{
		Debug.LogWarning(MiniJSON.jsonEncode(data));
		Debug.Log ("HashTable Data "+ data["id"] +" "+data["contact_no."] +" "+ data["rating"] + "" +data["filename"] );
//		if (!Validate(data, false))
//		{
//			ErrorMessage.Display("User.Construct: Invalid data: " + data.toJson());
//			return;
//		}
//		
//		SetBaseData(data);
		
		if((data.ContainsKey("id")))
		   m_id = Convert.ToInt32(data["id"]);

		if (data.ContainsKey("contact_no."))
		   m_contectNo = Convert.ToString(data["contact_no."]);
		
		if (data.ContainsKey("rating"))
		   m_ratings = (float) Convert.ToDecimal(data["rating"]);
		
		if (data.ContainsKey("filename"))
			LoadSelfiePicture(WWW.UnEscapeURL( Convert.ToString(data["filename"])));
		   


	}


}
