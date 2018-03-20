#if UNITY_EDITOR
using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEditor;

public class ApplyAssetChanges {

    [MenuItem("Assets/Apply changes")]
    public static void ApplyChanges(){
        AssetDatabase.SaveAssets();
    }    

}
#endif