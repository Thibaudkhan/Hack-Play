using System.Collections;
using System.Collections.Generic;
using Michsky.DreamOS;
using UnityEngine;
using UnityEngine.Events;

public class OsSystem : MonoBehaviour
{
    private CommanderManager commanderApp;

    public FileManager folders;
    // Start is called before the first frame update
    void Start()
    {
        //FolderSystem folders = gameObject.AddComponent<FolderSystem>();
        commanderApp = GameObject.Find("Commander").GetComponent<CommanderManager>();
        folders = GameObject.Find("Folde").GetComponent<FileManager>();
        ls();
        cd();
        rm();
        nano();
        mkdir();
    }


    public void ls()
    {
        CommanderManager.CommandItem item = createItem("ls", "ls");
        folders = GameObject.Find("Folde").GetComponent<FileManager>();
        commanderApp = GameObject.Find("Commander").GetComponent<CommanderManager>();

        item.onProcessEvent.AddListener(delegate
        {
            commanderApp.returnedText = folders.ListCurrentFolderContent(commanderApp.argument);
            Debug.Log("artg :"+commanderApp.argument);

        });

        commanderApp.commands.Add(item); // Add new item to the manager

        commanderApp.AddToHistory("Your text here", false, 0);
    } 
    
    public void cd()
    {
        CommanderManager.CommandItem item = createItem("cd", "cd");

        item.onProcessEvent.AddListener(delegate
        {
            commanderApp.returnedText = folders.Navigate(commanderApp.argument);
        });

        commanderApp.commands.Add(item); // Add new item to the manager

        commanderApp.AddToHistory("Your text here", false, 0);
    } 
    
    public void rm()
    {
        CommanderManager.CommandItem item = createItem("rm", "rm");


        item.onProcessEvent.AddListener(delegate
        {
            if (commanderApp.argument != "")
            {
                commanderApp.returnedText = folders.Remove(commanderApp.argument);
            }
            folders.Navigate(commanderApp.argument);
            //commanderApp.returnedText = folders.Navigate();

        });

        commanderApp.commands.Add(item); // Add new item to the manager

        commanderApp.AddToHistory("Your text here", false, 0);
    }    
    public void nano()
    {
        CommanderManager.CommandItem item = createItem("nano", "nano");


        item.onProcessEvent.AddListener(delegate
        {
            commanderApp.returnedText =  folders.CreateFile(commanderApp.argument,"");

        });

        commanderApp.commands.Add(item); // Add new item to the manager

        commanderApp.AddToHistory("Your text here", false, 0);
    }    
    public void mkdir()
    {
        CommanderManager.CommandItem item = createItem("mkdir", "mkdir");


        item.onProcessEvent.AddListener(delegate
        {
            commanderApp.returnedText =  folders.CreateFolder(commanderApp.argument);
        });

        commanderApp.commands.Add(item); // Add new item to the manager

        commanderApp.AddToHistory("Your text here", false, 0);
    }
    
    

    private CommanderManager.CommandItem createItem(string name,string command,string feedbackText = "null",float feedbackDelay = 0.1f,float onProcessDelay = 1f)
    {
        CommanderManager.CommandItem item = new CommanderManager.CommandItem();
        item.commandName = name; // Not important - only for editor
        item.command = command; // Actual command - user needs to type this
        item.feedbackText = feedbackText; // Feedback text
        item.feedbackDelay = feedbackDelay;
        item.onProcessDelay = onProcessDelay;
        item.isNewCommand = true;
        // item.onProcessEvent = null;
        if (item.onProcessEvent == null)
            item.onProcessEvent = new UnityEvent();
        
        return item;        
    }

    
}
