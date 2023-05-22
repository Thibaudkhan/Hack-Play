using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Website : MonoBehaviour
{
    private ComputerManager _computerManager;
    private string _name;
    private string url;
    private string fullUrl;
    
    public ComputerManager ComputerManager
    {
        get => _computerManager;
        set => _computerManager = value;
    }
    public string Url
    {
        get => url;
        set => url = value;
    }
    public string Name
    {
        get => _name;
        set => _name = value;
    }
    public string FullUrl
    {
        get => fullUrl;
        set => fullUrl = value;
    }


}
