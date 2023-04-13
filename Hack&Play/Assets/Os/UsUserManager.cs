using System.Collections;
using System.Collections.Generic;
using UnityEngine;

using System.Collections.Generic;

public class UsUser
{
    public string username;
    public string password;
    public List<string> friends;

    public UsUser(string username, string password)
    {
        this.username = username;
        this.password = password;
        this.friends = new List<string>();
    }
}

public static class UsUserManager
{
    private static List<UsUser> users = new List<UsUser>();

    public static void CreateUser(string username, string password)
    {
        // Check if user already exists
        if (FindUser(username) != null)
        {
            Debug.Log("User already exists!");
            return;
        }

        // Create new user
        UsUser newUser = new UsUser(username, password);
        users.Add(newUser);
        Debug.Log("User created!");
    }

    public static void DeleteUser(string username)
    {
        // Find user by username
        UsUser user = FindUser(username);

        if (user != null)
        {
            users.Remove(user);
            Debug.Log("User deleted!");
        }
        else
        {
            Debug.Log("User not found!");
        }
    }

    public static void UpdateUserPassword(string username, string password)
    {
        // Find user by username
        UsUser user = FindUser(username);

        if (user != null)
        {
            user.password = password;
            Debug.Log("User password updated!");
        }
        else
        {
            Debug.Log("User not found!");
        }
    }

    public static void AddFriend(string username, string friendUsername)
    {
        // Find user by username
        UsUser user = FindUser(username);

        if (user != null)
        {
            // Check if friend already exists
            if (user.friends.Contains(friendUsername))
            {
                Debug.Log("Friend already added!");
                return;
            }

            // Add friend
            user.friends.Add(friendUsername);
            Debug.Log("Friend added!");
        }
        else
        {
            Debug.Log("User not found!");
        }
    }

    public static void RemoveFriend(string username, string friendUsername)
    {
        // Find user by username
        UsUser user = FindUser(username);

        if (user != null)
        {
            // Check if friend exists
            if (!user.friends.Contains(friendUsername))
            {
                Debug.Log("Friend not found!");
                return;
            }

            // Remove friend
            user.friends.Remove(friendUsername);
            Debug.Log("Friend removed!");
        }
        else
        {
            Debug.Log("User not found!");
        }
    }

    public static UsUser FindUser(string username)
    {
        // Find user by username
        foreach (UsUser user in users)
        {
            if (user.username == username)
            {
                return user;
            }
        }

        return null;
    }
}

