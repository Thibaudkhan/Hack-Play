using UnityEngine;
using System.Collections.Generic;
using System.IO;
using UnityEditor;

public class FileManager : MonoBehaviour
{
    private string currentPath;
    private List<string> currentFolderContent;
    private ComputerManager computerManager;
    private void Start()
    {
        Debug.Log("start FileManager");
        Debug.Log(name);

        // get FolderPath from ComputerManager parent object
        //computerManager = transform.parent.GetComponent<ComputerManager>();
        //ComputerManager computerManager = transform.parent.GetComponent<ComputerManager>();
        //GameObject parentObject = GameObject.Find("ParentObject");
        computerManager = GetComponent<ComputerManager>();
        Debug.Log("currentPath" + computerManager.name);

        
        currentPath = computerManager.folderPath;
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
        // Normalize the new path
        newPath = newPath.Replace('\\', '/');

        // Navigate to the folder at the given path
        if (string.IsNullOrEmpty(newPath) || newPath[0].Equals('/'))
        {
            // Remove the first character if it's a slash   
            newPath = newPath.Substring(1);
            newPath = Application.streamingAssetsPath + "/OsData/" + computerManager.name + "/Os/" + newPath;
        }

        Debug.Log("newPath: " + newPath);
    
        // Combine the current path and the new path
        string combinedPath = Path.Combine(currentPath, newPath);

        // Normalize the combined path
        combinedPath = Path.GetFullPath(combinedPath).Replace('\\', '/');

        // Ensure the combined path is within the allowed OS folder
        if (!combinedPath.StartsWith(Application.streamingAssetsPath + "/OsData/" + computerManager.name + "/Os"))
        {
            return "";
        }

        // Use Path.Combine to concatenate paths
        if (Directory.Exists(combinedPath))
        {
            currentPath = combinedPath;
            LoadFolderContent(currentPath);
            return "\n";
        }

        return "Folder not found: " + newPath;
    }


    public string ListCurrentFolderContent(string path)
    {
        // Print the contents of the current folder to the console
        Debug.Log("currentPath : ");
        Debug.Log(currentPath);
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
    
    public string GetFileName(string path)
    {
        // Print the contents of the current folder to the console
        string fileName = Path.GetFileName(path); // Get the file name with extension
        string extension = Path.GetExtension(path);

        return fileName;
    }
    
    public bool CheckIfFileExists(string filePath)
    {
        string fullPath = Path.Combine(currentPath, filePath);
        string absolutePath = Path.GetFullPath(fullPath).Replace("\\", "/");
        Debug.Log("absolutePath: " + absolutePath);

        if (!absolutePath.StartsWith(Application.streamingAssetsPath + "/OsData/" + computerManager.name + "/Os"))
        {
            return false;
        }

        return File.Exists(absolutePath);
    }
    
    public bool SendFile(string filePath, string destinationPath)
    {


        // Vérifier si le fichier source existe
        if (!File.Exists(filePath))
        {
            Debug.Log("Source file does not exist.");
            return false;
        }
        if (File.Exists(destinationPath))
        {
            Debug.Log("Source file already exists");
            return false;
        }
        
        //FileUtil.CopyFileOrDirectory(filePath, destinationPath);
        Debug.Log("File sent successfully.");

        return true;
    }

}
