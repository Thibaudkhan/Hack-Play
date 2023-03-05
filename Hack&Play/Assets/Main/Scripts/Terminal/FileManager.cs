using UnityEngine;
using System.Collections.Generic;
using System.IO;

public class FileManager : MonoBehaviour
{
    private string currentPath;
    private List<string> currentFolderContent;
    
    private void Start()
    {
        Debug.Log("start");
        // Initialize the current path to the root directory
        currentPath = Application.streamingAssetsPath+"/Os/";
        Debug.Log("currentPath" + currentPath);
        //currentPath = "/";
        LoadFolderContent(currentPath);

        // List of folders to create
        List<string> foldersToCreate = new List<string>(){
            "bin",
            "boot",
            "dev",
            "etc",
            "home",
            "lib",
            "media",
            "mnt",
            "opt",
            "proc",
            "root",
            "run",
            "sbin",
            "srv",
            "sys",
            "tmp",
            "usr/bin",
            "usr/lib",
            "usr/local",
            "usr/sbin",
            "usr/share",
            "var"
        };

        // Create each folder in the list
        foreach (string folderName in foldersToCreate)
        {
            CreateFolder(folderName);
        }

        // Load the contents of the root directory
    }

    private void LoadFolderContent(string path)
    {
        // Load the content of the folder at the given path
        currentFolderContent = new List<string>();

        if (Directory.Exists(path))
        {
            currentFolderContent.AddRange(Directory.GetDirectories(path));
            currentFolderContent.AddRange(Directory.GetFiles(path));
            Debug.Log(currentFolderContent);
        }
        else
        {
            Debug.LogError("Directory does not exist: " + path);
        }
    }

    public string CreateFolder(string folderName)
    {
        // Create a new folder at the current path
        string newPath = Path.Combine(currentPath, folderName);

        if (!Directory.Exists(newPath))
        {
            Directory.CreateDirectory(newPath);

            // Reload the current folder content to show the new folder
            LoadFolderContent(currentPath);
            return "Folder created: " + newPath ;
        }
            return "Folder already exists: " + newPath ;
    }

    public string CreateFile(string fileName, string fileContent)
    {
        // Create a new file at the current path
        string filePath = Path.Combine(currentPath, fileName);

        if (!File.Exists(filePath))
        {
            File.WriteAllText(filePath, fileContent);

            // Reload the current folder content to show the new file
            LoadFolderContent(currentPath);
            return "File created: " + fileName;
        }
        
        return "File already exists: " + fileName;
    }

    public string Remove(string path)
    {
        Debug.Log("currentPath : "+currentPath);
        // Remove the file or folder at the given path
        if (Directory.Exists(currentPath+"/"+path))
        {
            Directory.Delete(currentPath+"/"+path, true);

            LoadFolderContent(currentPath);
            return "Folder removed: " + path;

        }
        if (File.Exists(currentPath+"/"+path))
        {
            File.Delete(currentPath+"/"+path);

            LoadFolderContent(currentPath);
            return "File removed: " + path;

        }
        
        return "Path not found: " + path;
    }

    public void UpdateFile(string path, string newContent)
    {
        // Update the content of the file at the given path
        if (File.Exists(path))
        {
            File.WriteAllText(path, newContent);
            Debug.Log("File updated: " + path);
        }
        else
        {
            Debug.LogWarning("File not found: " + path);
        }
    }

    public string Navigate(string newPath)
    {
        // Navigate to the folder at the given path
        // Set myPath to "/Assets/StreamingAssets/" if it's null or empty
        newPath = string.IsNullOrEmpty(newPath) || newPath == "/" ? Application.streamingAssetsPath+"/Os/" : newPath;
        string absolutePath = Path.Combine(currentPath, newPath);
        
        if (!Path.GetFullPath(absolutePath).Replace("\\", "/").StartsWith(Application.streamingAssetsPath+"/Os"))
        {
            // The target path is outside the StreamingAssets folder, so don't execute the function
            return "";
        }
        
        // Use Path.Combine to concatenate paths
        //string fullPath = Path.Combine(newPath, "myFile.txt");
        
        if (Directory.Exists(absolutePath))
        {
            currentPath = absolutePath;
            LoadFolderContent(currentPath);
            return "\n";
        }
        else
        {
            return "Folder not found: " + newPath;
        }
    }

    public string ListCurrentFolderContent(string path)
    {
        // Print the contents of the current folder to the console
        string files = "";
        LoadFolderContent(currentPath+"/"+path);
        foreach (string paths in currentFolderContent)
        {
            if (Path.GetExtension(paths) != ".meta")
            {
                files += Path.GetFileName(paths) + " ";

            }
        }

        return files;
    }
}
