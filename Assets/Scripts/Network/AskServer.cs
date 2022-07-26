using UnityEngine;
using System;
using System.Collections;

public class AskServer : MonoBehaviour
{
	static AskServer s_instance = null;
	public static AskServer Instance
	{
		get { if (s_instance == null) throw new UnityException("AskServer: Missing instance!"); return s_instance; }
	}


	void Awake()
	{
		if (s_instance != null)
			throw new UnityException("AskServer: Only one instance allowed!");
	
			s_instance = this;
	}
	public static void GetLatestCarouselData(Action<Hashtable> onResponse, Action<string> onError = null)
	{
		s_instance.StartCoroutine(RequestJSON("get.php", null, onResponse, onError));
	}
	public static void DowloadImage(string targetURL, Action<Texture2D> onResponse, Action<string> onError = null)
	{
		s_instance.StartCoroutine(RequestImage(targetURL, onResponse, onError));
	}

	private static IEnumerator RequestImage(string targetURL, Action<Texture2D> onResponse, Action<string> onError = null)
	{
		Debug.Log("AskServer.RequestImage: " + targetURL);

		// Create www call
		WWW www = null;
		{
			www = new WWW(targetURL);
		}

		// Process
		yield return www;

		// Check for errors
		if (!string.IsNullOrEmpty(www.error))
		{
			Debug.LogError("AskServer.RequestJSON: Error response from: " + targetURL + ", error: " + www.error);
			if (onError != null)
				onError(www.error);

			yield break;
		}

		// Note: Use LoadImageIntoTexture for DXT compression

		if (onResponse != null)
			onResponse(www.texture);
	}

	private static IEnumerator RequestJSON(string targetURL, WWWForm form, Action<ArrayList> onResponse, Action<string> onError = null)
	{
		string url = Settings.BaseURL + "/" + targetURL;
		Debug.Log("AskServer.RequestJSON: " + url);

		// Create www call
		WWW www;
		if (form != null)
		{
			form.AddField("cache", Mathf.FloorToInt(UnityEngine.Random.value * 999999999f));
			www = new WWW(url, form);
		}
		else
		{
			www = new WWW(url);
		}

		// Process
		yield return www;

		// Check for errors
		if (!string.IsNullOrEmpty(www.error))
		{
			Debug.LogError("AskServer.RequestJSON: Error response from: " + url + ", error: " + www.error);
			if (onError != null)
				onError(www.error);

			yield break;
		}

		// Get data
		object json = MiniJSON.jsonDecode(www.text);

		ArrayList jsonArray = json as ArrayList;
		if (jsonArray != null)
		{
			if (onResponse != null)
				onResponse(jsonArray);
		}
		else
		{
			Hashtable jsonTable = (Hashtable)json;
			if (jsonTable == null)
			{
				string error = "AskServer.RequestJSON: Invalid JSON from: " + url + ", error: " + www.text;
				Debug.LogError(error);

				if (onError != null)
					onError(error);

				yield break;
			}
			else if (jsonTable.ContainsKey("error"))
			{
				string error = "AskServer.RequestJSON: Error response from: " + url + ", error: " + jsonTable["error"].ToString();
				Debug.LogError(error);

				if (onError != null)
					onError(error);

				yield break;
			}
			else if (jsonTable.ContainsKey("warning"))
			{
				Debug.LogWarning("AskServer.RequestJSON: Warning response from: " + url + ", warning: " + jsonTable["warning"].ToString());
			}
		}
	}

	private static IEnumerator RequestJSON(string targetURL, WWWForm form, Action<Hashtable> onResponse, Action<string> onError = null)
	{
		string url = Settings.BaseURL + "/" + targetURL;
		Debug.Log("AskServer.RequestJSON: " + url);
		
		// Create www call
		WWW www;
		if (form != null)
		{
			form.AddField("cache", Mathf.FloorToInt(UnityEngine.Random.value * 999999999f));
			www = new WWW(url, form);
		}
		else
		{
			www = new WWW(url);
		}
		
		// Process
		yield return www;
		
		// Check for errors
		if (!string.IsNullOrEmpty(www.error))
		{
			Debug.LogError("AskServer.RequestJSON: Error response from: " + url + ", error: " + www.error);
			if (onError != null)
				onError(www.error);
			
			yield break;
		}
		
		// Get data
		object json = MiniJSON.jsonDecode(www.text);
		
		Hashtable jsonTable = (Hashtable)json;
		if (jsonTable == null)
		{
			string error = "AskServer.RequestJSON: Invalid JSON from: " + url + ", error: " + www.text;
			Debug.LogError(error);
			
			if (onError != null)
				onError(error);
			
			yield break;
		}
		else if (jsonTable.ContainsKey("error"))
		{
			string error = "AskServer.RequestJSON: Error response from: " + url + ", error: " + jsonTable["error"].ToString();
			Debug.LogError(error);
			
			if (onError != null)
				onError(error);
			
			yield break;
		}
		else if (jsonTable.ContainsKey("warning"))
		{
			Debug.LogWarning("AskServer.RequestJSON: Warning response from: " + url + ", warning: " + jsonTable["warning"].ToString());
		}
		else
		{
			if (onResponse != null)
				onResponse(jsonTable);
		}
		
	}

	public static void GetLastBestSelfieData(Action<Hashtable> onResponse, Action<string> onError = null)
	{
		s_instance.StartCoroutine(RequestJSON("bestselfi.php", null, onResponse, onError));
	}






}
