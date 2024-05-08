using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public abstract class DataTable
{
    public readonly string FormatPath = "DataTable/{0}";
    public abstract void Load(string Path);
}
