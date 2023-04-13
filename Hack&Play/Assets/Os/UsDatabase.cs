using System.Collections.Generic;
using UnityEngine;
using System.Data;
using Unity.VisualScripting.Dependencies.Sqlite;
using Mono.Data.SqliteClient;

public class UsDatabase : MonoBehaviour
{
    // Start is called before the first frame update
   private string connectionString;
    private IDbConnection dbConnection;

    void Start()
    {
        connectionString = "URI=file:" + Application.dataPath + "/UsDatabase.db";
        dbConnection = new SqliteConnection(connectionString);
        dbConnection.Open();

        // Create User table if it doesn't exist
        IDbCommand dbCommand = dbConnection.CreateCommand();
        dbCommand.CommandText = "CREATE TABLE IF NOT EXISTS User (id INTEGER PRIMARY KEY AUTOINCREMENT, username TEXT, password TEXT)";
        dbCommand.ExecuteNonQuery();
    }

    void OnDestroy()
    {
        dbConnection.Close();
    }



    public void CreateUser(string username, string password)
    {
        // Check if username already exists
        if (UserExists(username))
        {
            return ;
        }

        // Insert user into database
        IDbCommand dbCommand = dbConnection.CreateCommand();
        dbCommand.CommandText = "INSERT INTO User (username, password) VALUES (@username, @password)";
        dbCommand.Parameters.Add(new SqliteParameter("@username", username));
        dbCommand.Parameters.Add(new SqliteParameter("@password", password));
        dbCommand.ExecuteNonQuery();

        return ;
    }

    public bool UserExists(string username)
    {
        IDbCommand dbCommand = dbConnection.CreateCommand();
        dbCommand.CommandText = "SELECT COUNT(*) FROM User WHERE username = @username";
        dbCommand.Parameters.Add(new SqliteParameter("@username", username));
        long count = (long)dbCommand.ExecuteScalar();

        return count > 0;
    }

    public bool AuthenticateUser(string username, string password)
    {
        IDbCommand dbCommand = dbConnection.CreateCommand();
        dbCommand.CommandText = "SELECT COUNT(*) FROM User WHERE username = @username AND password = @password";
        dbCommand.Parameters.Add(new SqliteParameter("@username", username));
        dbCommand.Parameters.Add(new SqliteParameter("@password", password));
        long count = (long)dbCommand.ExecuteScalar();

        return count > 0;
    }

    public List<string> GetUsers()
    {
        List<string> users = new List<string>();

        IDbCommand dbCommand = dbConnection.CreateCommand();
        dbCommand.CommandText = "SELECT username FROM User";
        IDataReader reader = dbCommand.ExecuteReader();

        while (reader.Read())
        {
            users.Add(reader.GetString(0));
        }

        reader.Close();

        return users;
    }

    public bool DeleteUser(string username)
    {
        IDbCommand dbCommand = dbConnection.CreateCommand();
        dbCommand.CommandText = "DELETE FROM User WHERE username = @username";
        dbCommand.Parameters.Add(new SqliteParameter("@username", username));
        int rowsAffected = dbCommand.ExecuteNonQuery();

        return rowsAffected > 0;
    }

    public bool UpdatePassword(string username, string newPassword)
    {
        IDbCommand dbCommand = dbConnection.CreateCommand();
        dbCommand.CommandText = "UPDATE User SET password = @newPassword WHERE username = @username";
        dbCommand.Parameters.Add(new SqliteParameter("@newPassword", newPassword));
        dbCommand.Parameters.Add(new SqliteParameter("@username", username));
        int rowsAffected = dbCommand.ExecuteNonQuery();

        return rowsAffected > 0;
    }
    

}
