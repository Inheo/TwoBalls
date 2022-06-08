namespace MistplaySDK
{

using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public static class PlayerID 
{
    private static string _ID;

    public static string ID
    {
        get
        {
            if (_ID == null)
            {
                _ID = SystemInfo.deviceUniqueIdentifier;
            }
            return _ID;
        }
    }
    
}

}