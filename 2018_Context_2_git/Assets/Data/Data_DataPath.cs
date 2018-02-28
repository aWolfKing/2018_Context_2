using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using System.Diagnostics;

public static class Data_DataPath {

    private static string path = "";
    private static string pathFromAssets = "";

    public static string Path{
        get{
            if(path == ""){
                path = GetPathToThis();
            }
            return path;
        }
    }

    public static string PathRelativeToAssets{
        get{
            if(pathFromAssets == ""){
                pathFromAssets = Path.Replace(System.IO.Path.GetFullPath(System.IO.Path.Combine(Application.dataPath, @"..\")), "");
            }
            return pathFromAssets;
        }
    }

    private static string GetPathToThis(){
        StackFrame frame = new StackFrame(1, true);
        return System.IO.Path.GetFullPath(System.IO.Path.Combine(frame.GetFileName(), @"..\"));
    }

}
