using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using Mono.Data.Sqlite;
using System.IO;

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

        CreateDatabaseFileIfNotExists();
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
}
