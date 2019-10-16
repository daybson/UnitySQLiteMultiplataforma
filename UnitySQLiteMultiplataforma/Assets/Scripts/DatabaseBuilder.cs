using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using Mono.Data.Sqlite;
using System.IO;
using UnityEngine.Networking;

public class DatabaseBuilder : MonoBehaviour
{
    public string DatabaseName;
    protected string databasePath;
    protected SqliteConnection Connection => new SqliteConnection($"Data Source = {this.databasePath};");

    private void Awake()
    {
        if (string.IsNullOrEmpty(this.DatabaseName))
        {
            Debug.LogError("Database name is empty!");
            return;
        }
        CopyDatabaseFileIfNotExists();
        //CreateDatabaseFileIfNotExists();
    }

    private void CopyDatabaseFileIfNotExists()
    {
        this.databasePath = Path.Combine(Application.persistentDataPath, this.DatabaseName);

        if (File.Exists(this.databasePath))
            return;

        var originDatabasePath = string.Empty;
        var isAndroid = false;

#if UNITY_EDITOR || UNITY_WP8 || UNITY_WINRT

        originDatabasePath = Path.Combine(Application.streamingAssetsPath, this.DatabaseName);

#elif UNITY_STANDALONE_OSX

        originDatabasePath = Path.Combine(Application.dataPath, "/Resources/Data/StreamingAssets/", this.DatabaseName);
        
#elif UNITY_IOS

        originDatabasePath = Path.Combine(Application.dataPath, "Raw", this.DatabaseName);        

#elif UNITY_ANDROID

        isAndroid = true;
        originDatabasePath = "jar:file://" + Application.dataPath + "!/assets" + this.DatabaseName;
        StartCoroutine(GetInternalFileAndroid(originalDatabasePath));

#elif UNITY_WEBGL

        isAndroid = true;
        originDatabasePath = Path.Combine(Application.streamingAssetsPath, this.DatabaseName);
        StartCoroutine(GetInternalFileAndroid(originalDatabasePath));

#endif

        if (!isAndroid)
            File.Copy(originDatabasePath, this.databasePath);
    }

    private void CreateDatabaseFileIfNotExists()
    {
        this.databasePath = Path.Combine(Application.persistentDataPath, this.DatabaseName);

        if (!File.Exists(this.databasePath))
        {
            SqliteConnection.CreateFile(this.databasePath);
            Debug.Log($"Database path: {this.databasePath}");
        }
    }


    private IEnumerator GetInternalFileAndroid(string path)
    {
        var request = UnityWebRequest.Get(path);
        yield return request.SendWebRequest();

        if (request.isHttpError || request.isNetworkError)
        {
            Debug.LogError($"Error reading android file!: {request.error}");
        }
        else
        {
            File.WriteAllBytes(this.databasePath, request.downloadHandler.data);
            Debug.Log("File copied!");
        }
    }

}
