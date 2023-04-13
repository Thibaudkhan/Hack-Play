using System.Collections;
using System.Collections.Generic;
using Unity.VisualScripting.Dependencies.Sqlite;
using UnityEngine;

public class DatabaseManager : MonoBehaviour
{
    private SQLiteConnection _connection;

    void Awake()
    {
        var dbPath = Application.dataPath + "/myDatabase.db";
        _connection = new SQLiteConnection(dbPath, SQLiteOpenFlags.ReadWrite | SQLiteOpenFlags.Create);

        // create a table in the database
        //_connection.CreateTable<User>();
    }

    void OnDestroy()
    {
        _connection.Dispose();
    }
}
