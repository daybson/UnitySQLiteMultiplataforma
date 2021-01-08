﻿using Assets.Scripts.Persistence;
using Assets.Scripts.Persistence.DAO.Specification;

using Mono.Data.Sqlite;

using System;
using System.Collections;
using System.IO;

using UnityEngine;
using UnityEngine.Networking;


public class SQLiteDataSource : MonoBehaviour, ISQLiteConnectionProvider
{
    [SerializeField]
    protected string databaseName;
    protected string databasePath;
    public string DatabaseName => this.databaseName;

    public SqliteConnection Connection => new SqliteConnection($"Data Source = {this.databasePath};");

    [SerializeField]
    protected bool copyDatabase;

    public static Action registerInitialLoad;


    protected void Awake()
    {
        var game = FindObjectOfType<MyNewGame>();
        registerInitialLoad += game.LoadDataAwake;

        if (string.IsNullOrEmpty(this.databaseName))
        {
            Debug.LogError("Database name is empty!");
            return;
        }

        try
        {
            if (this.copyDatabase)
            {
                CopyDatabaseFileIfNotExists();
            }
            else
            {
                CreateDatabaseFileIfNotExists();
            }
        }
        catch (Exception e)
        {
            Debug.LogError(e.Message);
        }
    }

    #region Create database

    /// <summary>
    /// NEW 2021
    /// </summary>
    protected void DeleteDatabase()
    {
        this.databasePath = Path.Combine(Application.persistentDataPath, this.databaseName);

        if (!File.Exists(this.databasePath))
            return;

        File.Delete(this.databasePath);
    }


    protected void CopyDatabaseFileIfNotExists()
    {
        this.databasePath = Path.Combine(Application.persistentDataPath, this.databaseName);
        Debug.Log("PATH: " + this.databasePath);

        if (File.Exists(this.databasePath))
        {
            registerInitialLoad?.Invoke();
            return;
        }

        var originDatabasePath = string.Empty;
        var isAndroid = false;

#if UNITY_EDITOR || UNITY_WP8 || UNITY_WINRT || UNITY_STANDALONE_WIN || UNITY_STANDALONE_LINUX

        originDatabasePath = Path.Combine(Application.streamingAssetsPath, this.databaseName);

#elif UNITY_STANDALONE_OSX

        originDatabasePath = Path.Combine(Application.dataPath, "/Resources/Data/StreamingAssets/", this.DatabaseName);
        
#elif UNITY_IOS

        originDatabasePath = Path.Combine(Application.dataPath, "Raw", this.DatabaseName);        

#elif UNITY_ANDROID

        isAndroid = true;
        originDatabasePath = "jar:file://" + Application.dataPath + "!/assets/" + this.DatabaseName;
        StartCoroutine(GetInternalFileAndroid(originDatabasePath));

#endif

        if (!isAndroid)
        {
            Debug.Log($"COPY FILE: {originDatabasePath} to {this.databasePath}");
            File.Copy(originDatabasePath, this.databasePath);
            registerInitialLoad?.Invoke();
        }
    }

    protected void CreateDatabaseFileIfNotExists()
    {
        this.databasePath = Path.Combine(Application.persistentDataPath, this.databaseName);

        if (!File.Exists(this.databasePath))
        {
            SqliteConnection.CreateFile(this.databasePath);
            Debug.Log($"Database path: {this.databasePath}");
        }
    }

    protected IEnumerator GetInternalFileAndroid(string path)
    {
        var request = UnityWebRequest.Get(path);
        yield return request.SendWebRequest();

        if (request.isHttpError || request.isNetworkError)
        {
            Debug.LogError($"Error reading android file!: {request.error}");
            throw new Exception($"Error reading android file!: { request.error }");
        }
        else
        {
            File.WriteAllBytes(this.databasePath, request.downloadHandler.data);
            Debug.Log("File copied! ->" + this.databasePath);

            registerInitialLoad?.Invoke();
        }
    }


    #endregion


}