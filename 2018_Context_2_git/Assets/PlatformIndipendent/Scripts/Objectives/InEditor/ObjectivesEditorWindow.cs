#if UNITY_EDITOR
using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEditor;

public class ObjectivesEditorWindow : EditorWindow {

    public static void Open(){
        ObjectivesEditorWindow.GetWindowWithRect<ObjectivesEditorWindow>(new Rect() { position = new Vector2(100, 50), size = new Vector2(600, 800) });
    }

    private void OnGUI() {
        
    }

}
#endif