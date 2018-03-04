#if UNITY_EDITOR
using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEditor;

public class HouseHoldItemDataSelectEditorWindow : EditorWindow {

    public delegate void selectVoid(HouseHoldItemData data);

    private static selectVoid onSelectionFinish = null;

    private static Vector2 scroll = Vector2.zero;


    public static void GetSelector(selectVoid onSelected){
        HouseHoldItemDataSelectEditorWindow.GetWindowWithRect<HouseHoldItemDataSelectEditorWindow>(new Rect(110, 90, 400, 600), false, "HouseHoldItem-Selecting", true);
        onSelectionFinish = onSelected;
    }


    private void OnGUI() {

        Rect scrollRect = new Rect(){
            width = 400,
            height = 600
        };
        Rect viewRect = new Rect() {
            width = 400 - 16,
            height = HouseHoldItemDataEditorWindow.Data.data.Count*74
        };

        scroll = GUI.BeginScrollView(scrollRect, scroll, viewRect, false, true);


        Rect itemRect = new Rect(){
            width = 400-14,
            height = 70,
            position = new Vector2(0,4)
        };

        
        for(int i=0; i<HouseHoldItemDataEditorWindow.Data.data.Count; i++){

            GUI.Box(itemRect, "");
            itemRect.position += new Vector2(1, 1);
            itemRect.height -= 2;
            itemRect.width -= 1;
            float heightWas = EditorStyles.toolbar.fixedHeight;
            EditorStyles.toolbar.fixedHeight = 0;
            GUI.Box(itemRect, "", EditorStyles.toolbar);
            EditorStyles.toolbar.fixedHeight = heightWas;
            itemRect.position -= new Vector2(1, 1);
            itemRect.height += 2;
            itemRect.width += 1;

        }


        GUI.EndScrollView(true);

    }


}
#endif