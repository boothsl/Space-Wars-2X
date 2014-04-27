using UnityEngine;
using UnityEditor;
using System;
using System.IO;
using System.Collections;
using System.Collections.Generic;

public class UsedAssets
{
	public static string[] GetAllAssets()
	{
		string[] tmpAssets1 = Directory.GetFiles(Application.dataPath, "*.*", SearchOption.AllDirectories);
		string[] tmpAssets2 = Array.FindAll(tmpAssets1, name => !name.EndsWith(".meta"));
		string[] allAssets;
		
		allAssets = Array.FindAll(tmpAssets2, name => !name.EndsWith(".unity"));
		
		for (int i = 0; i < allAssets.Length; i++)
		{
			allAssets[i] = allAssets[i].Substring(allAssets[i].IndexOf("/Assets") + 1);
			allAssets[i] = allAssets[i].Replace(@"\", "/");
		}
		
		return allAssets;
	}
	
	public static void GetLists(ref List<string> assetResult, ref List<string> dependencyResult)
	{
		assetResult.Clear();
		dependencyResult.Clear();
		
		string LocalAppData = string.Empty;
		string UnityEditorLogfile = string.Empty;
		
		if (Application.platform == RuntimePlatform.WindowsEditor)
		{
			LocalAppData = Environment.GetFolderPath(Environment.SpecialFolder.LocalApplicationData);
			UnityEditorLogfile = LocalAppData + "\\Unity\\Editor\\Editor.log";
		}
		else if (Application.platform == RuntimePlatform.OSXEditor)
		{
			LocalAppData = Environment.GetFolderPath(Environment.SpecialFolder.Personal);
			UnityEditorLogfile = LocalAppData + "/Library/Logs/Unity/Editor.log";
		}
		
		try
		{
			// Have to use FileStream to get around sharing violations!
			FileStream FS = new FileStream(UnityEditorLogfile, FileMode.Open, FileAccess.Read, FileShare.ReadWrite);
			StreamReader SR = new StreamReader(FS);
			
			
			string line;
			while (!SR.EndOfStream && !(line = SR.ReadLine()).Contains("Mono dependencies included in the build"));
			while (!SR.EndOfStream && (line = SR.ReadLine()) != "")
			{
				dependencyResult.Add(line);
			}
			while (!SR.EndOfStream && !(line = SR.ReadLine()).Contains("Used Assets,"));
			while (!SR.EndOfStream && (line = SR.ReadLine()) != "")
			{
				
				line = line.Substring(line.IndexOf("% ") + 2);
				assetResult.Add(line);
			}
		}
		catch (Exception E)
		{
			Debug.LogError("Error: " + E);
		}
	}
}