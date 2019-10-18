using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using Mono.Data.Sqlite;
using System.IO;
using UnityEngine.Networking;
using System;

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

        try
        {
            //CreateTableWeapon();
            //CreateTableCharacter();

            //InsertDataWeapon("Sword", 10, 25.89d);
            InsertDataCharacter("Kaz", 2, 1, 3, 10, 1);
            //Debug.Log(GetCharacter(1));
            //Debug.Log("DELETE CHARACTER: " + DeleteCharacter(1));
            Debug.Log(UpdateCharacter(1, "Kaz Kazyamof", 4, 5, 6, 7, 2));
        }
        catch (Exception e)
        {
            Debug.LogError(e.Message);
        }

    }


    #region Create database

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
        StartCoroutine(GetInternalFileAndroid(originDatabasePath));

#elif UNITY_WEBGL

        isAndroid = true;
        originDatabasePath = Path.Combine(Application.streamingAssetsPath, this.DatabaseName);
        StartCoroutine(GetInternalFileAndroid(originDatabasePath));

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

    #endregion


    protected void CreateTableCharacter()
    {
        var commandText =
            "CREATE TABLE Character " +
            "(" +
            "   Id INTEGER PRIMARY KEY, " +
            "   Name TEXT NOT NULL, " +
            "   Attack INTEGER NOT NULL, " +
            "   Defense INTEGER NOT NULL, " +
            "   Agility INTEGER NOT NULL, " +
            "   Health INTEGER NOT NULL, " +
            "   WeaponId INTEGER, " +
            "   FOREIGN KEY (WeaponId) REFERENCES Weapon(Id) ON UPDATE CASCADE ON DELETE RESTRICT" +
            "); ";

        using (var connection = Connection)
        {
            connection.Open();
            using (var command = connection.CreateCommand())
            {
                command.CommandText = commandText;
                command.ExecuteNonQuery();
                Debug.Log("CreateTableCharacter");
            }
        }
    }

    protected void CreateTableWeapon()
    {
        var commandText =
            "CREATE TABLE Weapon " +
            "(" +
            "   Id INTEGER PRIMARY KEY, " +
            "   Name TEXT NOT NULL, " +
            "   Attack INTEGER NOT NULL, " +
            "   Price REAL NOT NULL" +
            "); ";

        using (var connection = Connection)
        {
            connection.Open();
            using (var command = connection.CreateCommand())
            {
                command.CommandText = commandText;
                command.ExecuteNonQuery();
                Debug.Log("CreateTableWeapon");
            }
        }
    }


    protected void InsertDataWeapon(string name, int attack, double price)
    {
        var commandText = "INSERT INTO Weapon(Name, Attack, Price) VALUES (@name, @attack, @price);";

        using (var connection = Connection)
        {
            connection.Open();

            using (var command = connection.CreateCommand())
            {
                command.CommandText = commandText;

                command.Parameters.AddWithValue("@name", name);
                command.Parameters.AddWithValue("@attack", attack);
                command.Parameters.AddWithValue("@price", price);

                var result = command.ExecuteNonQuery();
                Debug.Log($"INSERT WEAPON: {result.ToString()}");
            }
        }
    }

    protected void InsertDataCharacter(string name, int attack, int defense, int agility, int health, int weaponId)
    {
        var commandText = "INSERT INTO Character(Name, Attack, Defense, Agility, Health, WeaponId) " +
            " VALUES (@name, @attack, @defense, @agility, @health, @weaponId);";

        using (var connection = Connection)
        {
            connection.Open();

            using (var command = connection.CreateCommand())
            {
                command.CommandText = commandText;

                command.Parameters.AddWithValue("@name", name);
                command.Parameters.AddWithValue("@attack", attack);
                command.Parameters.AddWithValue("@defense", defense);
                command.Parameters.AddWithValue("@agility", agility);
                command.Parameters.AddWithValue("@health", health);
                command.Parameters.AddWithValue("@weaponId", weaponId);

                var result = command.ExecuteNonQuery();
                Debug.Log($"INSERT CHARACTER: {result.ToString()}");
            }
        }
    }

    protected string GetCharacter(int id)
    {
        var commandText = "SELECT * FROM Character WHERE Id = @id;";
        var result = "None";

        using (var connection = Connection)
        {
            connection.Open();
            using (var command = connection.CreateCommand())
            {
                command.CommandText = commandText;
                command.Parameters.AddWithValue("@id", id);

                var reader = command.ExecuteReader();

                if (reader.Read())
                {
                    result = $"Id: {reader.GetInt32(0).ToString()}, " +
                        $"Name: {reader.GetString(1)}, " +
                        $"Attack: {reader.GetInt32(2)}, " +
                        $"Defense: {reader["Defense"]}, " +
                        $"Agility: {reader["Agility"]}," +
                        $"Health: {reader["Health"]}, " +
                        $"Weapon Id: {reader["WeaponId"]}";
                }

                return result;
            }
        }
    }


    protected int DeleteCharacter(int id)
    {
        var commandText = "DELETE FROM Character WHERE Id = @id;";

        using (var connection = Connection)
        {
            connection.Open();
            using (var command = connection.CreateCommand())
            {
                command.CommandText = commandText;
                command.Parameters.AddWithValue("@id", id);

                return command.ExecuteNonQuery();
            }
        }
    }


    protected int UpdateCharacter(int id, string name, int attack, int defense, int agility, int health, int weaponId)
    {
        var commandText = 
            "UPDATE Character SET " +
            "Name = @name, " +
            "Attack = @attack, " +
            "Defense = @defense, " +
            "Agility = @agility, " +
            "Health = @health, " +
            "WeaponId = @weaponId " +
            "WHERE Id = @id;";

        using (var connection = Connection)
        {
            connection.Open();

            using (var command = connection.CreateCommand())
            {
                command.CommandText = commandText;

                command.Parameters.AddWithValue("@id", id);
                command.Parameters.AddWithValue("@name", name);
                command.Parameters.AddWithValue("@attack", attack);
                command.Parameters.AddWithValue("@defense", defense);
                command.Parameters.AddWithValue("@agility", agility);
                command.Parameters.AddWithValue("@health", health);
                command.Parameters.AddWithValue("@weaponId", weaponId);

                return command.ExecuteNonQuery();                
            }
        }
    }
}