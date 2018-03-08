#if UNITY_EDITOR
using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEditor;

[CustomEditor(typeof(ProceduralUIComponent))]
public class UITool : Editor {

    public override void OnInspectorGUI() {
        
        ProceduralUIComponent targ = (ProceduralUIComponent)target;
        Undo.RegisterCompleteObjectUndo(targ, "UI component changed");

        GUILayout.Space(10);

        if(GUILayout.Button("Edit", EditorStyles.toolbarButton)){
            UIEditorWindow.OpenWindow(targ);
        }

        GUILayout.Space(10);

    }

}
#endif