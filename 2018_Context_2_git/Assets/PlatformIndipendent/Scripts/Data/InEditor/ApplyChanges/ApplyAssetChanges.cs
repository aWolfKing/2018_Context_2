#if UNITY_EDITOR
using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEditor;

public class ApplyAssetChanges {

    [MenuItem("Assets/Apply changes")]
    public static void ApplyChanges(){
        AssetDatabase.SaveAssets();
        AssetDatabase.Refresh();

        if(Selection.activeObject != null){
            AssetDatabase.ForceReserializeAssets(GetPathToSelection(), ForceReserializeAssetsOptions.ReserializeAssets);
        }

    }    

    private static IEnumerable<string> GetPathToSelection(){
        yield return AssetDatabase.GetAssetPath(Selection.activeObject);
    }

}
#endif