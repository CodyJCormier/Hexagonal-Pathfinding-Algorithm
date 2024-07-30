// © 2024 Cody Cormier. All rights reserved. Created on 2024-07-29.

using UnityEngine;

public static class Extensions
{
    public static void Log(this object obj, string log, bool error = false)
    {
        if (error)
        {
            Debug.LogError($"Error_{obj} : {log}");
        }
        else
            Debug.Log($"{obj} : {log}");
    }
}