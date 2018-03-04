#if UNITY_EDITOR
using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEditor;

public class CategoryEditorWindow : EditorWindow {

    private static Vector2 scroll = Vector2.zero
    ;

    private void OnGUI() {

        Undo.RegisterCompleteObjectUndo(HouseHoldItemDataEditorWindow.Data, "Edited categories");

        Rect scrollRect = new Rect(){
            width = 400,
            height = 600
        };
        scroll = GUI.BeginScrollView(scrollRect, scroll, new Rect() { width = 400-16, height = (HouseHoldItemDataEditorWindow.Data.categories.Count+1)*18}, false, true);

        Rect itemRect = new Rect(){
            width = 400-16-50,
            height = 18
        };
        for(int i=0; i<HouseHoldItemDataEditorWindow.Data.categories.Count; i++){
            HouseHoldItemDataEditorWindow.Data.categories[i] = GUI.TextField(itemRect, HouseHoldItemDataEditorWindow.Data.categories[i]);
            Rect removeRect = itemRect;
            removeRect.position += new Vector2(itemRect.width, 0);
            removeRect.width = 50;
            itemRect.position += new Vector2(0, 18);
            if(GUI.Button(removeRect, "remove", EditorStyles.toolbarButton)){
                HouseHoldItemDataEditorWindow.Data.categories.RemoveAt(i);
                break;
            }
        }
        itemRect.width = 70;
        if(GUI.Button(itemRect, "Add new", EditorStyles.toolbarButton)){
            HouseHoldItemDataEditorWindow.Data.categories.Add("new category");
        }

        GUI.EndScrollView();

    }

}
#endif