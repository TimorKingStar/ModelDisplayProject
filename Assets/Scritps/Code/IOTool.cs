using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using System.IO;

public class IOTool 
{
    public static void WriteFile(byte[] file,string path)
    {
        File.WriteAllBytes(path, file);
    }
}
