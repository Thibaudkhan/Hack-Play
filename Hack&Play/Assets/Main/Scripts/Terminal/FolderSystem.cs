using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using Michsky.DreamOS;
using Unity.VisualScripting;
using Unity.VisualScripting.FullSerializer;
using UnityEngine.Events; // namespace


public class FolderSystem : MonoBehaviour
{
    public string Name { get; }
    public string Path { get; }

    public List<FolderSystem> Folders { get; }
    public List<string> Files { get; }
    public bool isChanging = false;

    // private List<string> folderNames;

    // private Dictionary<string, List<string>> subfolders;

    public List<FolderSystem> listOfFolders = new List<FolderSystem>();


    public FolderSystem(string name,string path)
    {
        Name = name;
        Path = path;
        Folders = new List<FolderSystem>();
        Files = new List<string>();
        
    }

    private void Start()
    {
       
        Debug.Log("FolderSys");
        // tempo = new CreateFolderCommand();
    }

    public void AddFolder(FolderSystem folder)
    {
        Folders.Add(folder);
    }

    public void AddFile(string fileName)
    {
        Files.Add(fileName);
    }

    public bool RemoveFolder(FolderSystem folder)
    {
        return Folders.Remove(folder);
    }

    public bool RemoveFile(string fileName)
    {
        return Files.Remove(fileName);
    }

    public string GetFoldersToString()
    {
        string folders = "";
        foreach (FolderSystem folder in listOfFolders)
        {
            folders += "\n" + folder.Name + " ";
        }

        return folders;
    }
}