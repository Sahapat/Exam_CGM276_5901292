using System;
using UnityEngine;

[Serializable]
public class PlayerJSON
{
    public string name;
    public int number;

    public PlayerJSON(string name,int number)
    {
        this.name = name;
        this.number = number;
    }
    public static PlayerJSON CreateFromJSON(string data)
    {
        return JsonUtility.FromJson<PlayerJSON>(data);
    }
}
[Serializable]
public class CheckJSON
{
    public string name;
    public int status;

    public static CheckJSON CreateFromJSON(string data)
    {
        return JsonUtility.FromJson<CheckJSON>(data);
    }
}

public static class PlayerData
{
    public static string name;
}