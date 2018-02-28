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
                UnityEngine.Debug.Log(Path.Replace(System.IO.Path.GetFullPath(System.IO.Directory.GetParent(Application.dataPath).FullName), ""));
                pathFromAssets = Path.Replace(System.IO.Path.GetFullPath(System.IO.Directory.GetParent(Application.dataPath).FullName.Replace(@"..\","")), "");
                pathFromAssets = pathFromAssets.Substring(1,pathFromAssets.Length-1);
            }
            return pathFromAssets;
        }
    }

    private static string GetPathToThis(){
        StackFrame frame = new StackFrame(1, true);
        return System.IO.Path.GetFullPath(System.IO.Directory.GetParent(frame.GetFileName()).FullName);
    }

}
