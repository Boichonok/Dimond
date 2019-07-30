using UnityEngine;
using System.Collections.Generic;
using System;

[Serializable]
public class GameSettings 
{
    public int CoinsCount = 0;
    public bool isFirstStartApp = true;
    public bool isAdvicersActive = true;
    public string SaveToJson()
    {
       return JsonUtility.ToJson(this);
    }
}
