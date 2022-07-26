using System;
using UnityEngine;
using System.Collections;
using System.Collections.Generic;
using System.Linq;

public class BestSelfieLoader : MonoBehaviour {

	private string m_imageUrl;

	// Use this for initialization
	void Start () {

		//LoadSelfiePicture(Settings.BaseURL+"/bestselfi.php" );
		//AskServer.GetLatestCarouselData(OnBestSelfieData, OnErrorInSelfieData);
		AskServer.GetLastBestSelfieData(OnBestSelfieData,OnErrorInSelfieData);
	}
	
	// Update is called once per frame
	void Update () {
	
	}

	void LoadSelfiePicture(string url)
	{
		if (url.Length == 0)
			return;
		Debug.Log ("Downloading Image.."+url);
		AskServer.DowloadImage(url, OnLoadSelfiePicture, OnLoadSelfiePictureFail);
	}
	
	void OnLoadSelfiePicture(Texture2D texture)
	{
		gameObject.renderer.material.mainTexture = texture;

	}
	
	void OnLoadSelfiePictureFail(string error)
	{
		Debug.LogWarning("Failed to load profile picture");
	}

	void OnErrorInSelfieData(string Error)
	{
		Debug.Log ("Error "+Error);
	}

	void OnBestSelfieData(Hashtable data)
	{
		ArrayList dataArray;
		if(data.ContainsKey("images")){
			 dataArray = (ArrayList)data["images"];
						Hashtable imgurl = (Hashtable)dataArray[0];

			if (imgurl.ContainsKey("filename"))
				LoadSelfiePicture(WWW.UnEscapeURL( Convert.ToString(imgurl["filename"])));
	
		}
	
	}		


}
