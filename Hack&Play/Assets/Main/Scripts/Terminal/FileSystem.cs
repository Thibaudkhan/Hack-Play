using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class FileSystem : MonoBehaviour
{
public FolderSystem RootFolder { get; }

    private Dictionary<string, List<string>> subfolders;



    public FileSystem() {
        Debug.Log("Create FileSystem");
        RootFolder = new FolderSystem("/","/");
    }

    private void Start()
    {
        //CreateFolder("/", "bin");
        subfolders = new Dictionary<string, List<string>>()
        {
            { "bin", new List<string>() },
            { "boot", new List<string>() },
            { "dev", new List<string>() },
            { "etc", new List<string>() },
            { "home", new List<string>() },
            { "lib", new List<string>() },
            { "mnt", new List<string>() },
            { "opt", new List<string>() },
            { "proc", new List<string>() },
            { "root", new List<string>() },
            { "run", new List<string>() },
            { "sbin", new List<string>() },
            { "snap", new List<string>() },
            { "srv", new List<string>() },
            { "sys", new List<string>() },
            { "tmp", new List<string>() },
            {
                "usr", new List<string>()
                {
                    "bin",
                    "include",
                    "lib",
                    "local",
                    "sbin",
                    "share",
                    "src"
                }
            },
            {
                "var", new List<string>()
                {
                    "cache",
                    "lib",
                    "local",
                    "log",
                    "opt",
                    "run",
                    "spool",
                    "tmp"
                }
            }
        };

        // Loop through list and create folders
        foreach (string folderName in subfolders.Keys)
        {
            CreateFolder("/",folderName);

            foreach (string subfolderName in subfolders[folderName])
            {
                string subfolderPath = folderName + "/" + subfolderName;
                CreateFolder(folderName, subfolderName);
            }
        }
        CreateFolder("bin", "coucou");
    }

    
    
    public FolderSystem Navigate(string path) {
        Debug.Log("Navigate: " + path);
        if (path == "/") {
            Debug.Log("Navigate to root");
            return RootFolder;
        }
        
        string[] parts = path.Split('/');
        FolderSystem currentFolder = RootFolder;
        foreach (string part in parts) {
            if (part == "") {
                continue;
            }
            
            bool found = false;
            Debug.Log("Navigate to " + currentFolder.Name + " // " + part);
            foreach (FolderSystem folder in currentFolder.Folders) {
                if (folder.Name == part) {
                    currentFolder = folder;
                    found = true;
                    break;
                }
            }
            
            if (!found) {
                return null;
            }
        }
        
        return currentFolder;
    }
    
    public string  Ls(string path)
    {
        // Find the folder that corresponds to the given path
        FolderSystem currentFolder = Navigate(path);
        Debug.Log("path " + path + " currentFolder " + currentFolder.Name + "");
        if (currentFolder == null)
        {
            Debug.Log("Path not found");
            return "Path not found" ;
        }

        string folders = "";
        // Display the folders
        Debug.Log("Folders:");
        foreach (FolderSystem folder in currentFolder.Folders)
        {
            folders += "\n" + folder.Name + " ";
            Debug.Log(folder.Name);
        }

        // Display the files
        Debug.Log("Files:");
        foreach (string file in currentFolder.Files)
        {
            folders += "\n" + file + " ";
            Debug.Log(file);
        }

        return folders;
    }


    public bool CreateFolder(string path, string name) {
        Debug.Log("CreateFolder: " + path + " " + name);
        FolderSystem parentFolder = Navigate(path);
        Debug.Log("parentFolder " + parentFolder.Name);

        if (parentFolder.Name == "") {

            return false;
        }
        
        foreach (FolderSystem folder in parentFolder.Folders) {
            if (folder.Name == name) {
                return false;
            }
        }
        
        FolderSystem newFolder = new FolderSystem(name,parentFolder.Path + "/" + name);

        parentFolder.AddFolder(newFolder);

        return true;
    }

    public bool DeleteFolder(string path) {
        if (path == "/") {
            return false;
        }
        
        string[] parts = path.Split('/');
        string folderName = parts[parts.Length - 1];
        string parentPath = path.Substring(0, path.Length - folderName.Length - 1);
        FolderSystem parentFolder = Navigate(parentPath);
        
        if (parentFolder == null) {
            return false;
        }
        
        foreach (FolderSystem folder in parentFolder.Folders) {
            if (folder.Name == folderName) {
                return parentFolder.RemoveFolder(folder);
            }
        }
        
        return false;
    }

    public bool CreateFile(string path, string fileName) {
        FolderSystem parentFolder = Navigate(path);
        
        if (parentFolder == null) {
            return false;
        }
        
        foreach (string file in parentFolder.Files) {
            if (file == fileName) {
                return false;
            }
        }
        
        parentFolder.AddFile(fileName);
        return true;
    }

    public bool DeleteFile(string path, string fileName) {
        FolderSystem parentFolder = Navigate(path);
        
        if (parentFolder == null) {
            return false;
        }
        
        return parentFolder.RemoveFile(fileName);
    }
    
    public string GetFoldersToString(FolderSystem currentFolder)
    {
        string folders = "";
        foreach (FolderSystem folder in currentFolder.Folders)
        {
            Debug.Log("GetFoldersToString " + folder.Name);
            folders += "\n" + folder.Name + " ";
        }

        return folders;
    }
}
