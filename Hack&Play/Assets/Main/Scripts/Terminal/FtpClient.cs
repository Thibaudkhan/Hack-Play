using System;
using System.Collections.Generic;
using System.IO;
using System.Net;
using System.Net.Sockets;
using Main.Scripts.Network;
using UnityEngine;

public class FtpClient : MonoBehaviour
{
    public ComputerManager user;
    public Router routerManager;
    public ComputerManager cible;
    private string ipUser;

    private void Start()
    {
    }


    public string ExecuteCommand(string[] arguments)
    {
        ipUser = NetworkManager.GetIpAddress(user);

        // check the argument to execute different function
        switch (arguments[0])
        {
            case "open":
                if (arguments.Length < 4)
                {
                    return "Not enough arguments";
                }
                return ConnectToComputerManager(arguments[0],arguments[1],arguments[2],arguments[3]);
            case "close":
                if (arguments.Length < 1)
                {
                    return "Not enough arguments";
                }
                return DisconnectFromComputerManager(arguments[0]);
            default:
                if (arguments.Length < 1)
                {
                    return "Not enough arguments";
                }
                return OtherCommands(arguments[0],arguments);
        }
        
        // send a FTP package 
        
    }

    private string ConnectToComputerManager(string methode,string ipReceiver, string username, string password)
    {
       Packet packet = new Packet(senderIpAddress:ipUser, receiverIpAddress:ipReceiver, type:PacketType.FTP);
       packet.Content = "username:"+username+",password:"+password+",argument:"+ipReceiver+",ipSender:"+ipUser;
       packet.Methode = methode;
       if (!user.SendFTPRequest(packet))
       {
           return "";
       }
       Debug.Log("The content "+       NetworkManager.GetComputer(ipReceiver).CurrentPacket.Content);
       return user.CurrentPacket.Content;
    }

    public string DisconnectFromComputerManager(string methode)
    {
        string ipReceiver = user.lastFtpIp;
        Packet packet = new Packet(senderIpAddress:ipUser, receiverIpAddress:ipReceiver, type:PacketType.FTP);
        packet.Content = "argument:"+ipReceiver+",ipSender:"+ipUser;
        Debug.Log("argument:"+ipReceiver+",ipSender:"+ipUser);
        packet.Methode = methode;
        if (!user.SendFTPRequest(packet))
        {
            return "";
        }
       
        return user.CurrentPacket.Content;
    }

    public string UploadFile(string filePath, string destinationPath,string ipReceiver)
    {
        Packet packet = new Packet(senderIpAddress:ipUser, receiverIpAddress:ipReceiver, type:PacketType.FTP);
        packet.Methode = "get";
        
        if (!user.SendFTPRequest(packet))
        {
            return "";
        }
       
        return user.CurrentPacket.Content;
    }

    public void DownloadFile(string sourcePath, string destinationPath)
    {
       
    }

    public string OtherCommands(string methode, string[] arguments)
    {
        Debug.Log("last ip "+user.lastFtpIp);
        string ipFTP = user.lastFtpIp;
        if (ipFTP == "")
            return "";

        ComputerManager cible = NetworkManager.GetComputer(ipFTP);
        if (!cible.isConnectedFtp)
            return "";
        
        
        Packet packet = new Packet(senderIpAddress:ipUser, receiverIpAddress:ipFTP, type:PacketType.FTP);
        // convertt argument to string
        Debug.Log("senderIpAddress : "+packet.senderIpAddress+" receiverIpAddress :"+packet.receiverIpAddress);

        string argument = string.Join(",", arguments);
        
        packet.Content = "argument:"+argument;
        packet.Methode = methode;
        if (!user.SendFTPRequest(packet))
        {
            return "";
        }
       
        return user.CurrentPacket.Content;
        return "";
    }

}
