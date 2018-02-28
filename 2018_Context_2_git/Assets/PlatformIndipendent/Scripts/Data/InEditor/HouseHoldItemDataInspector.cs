#if UNITY_EDITOR
using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEditor;

[CustomEditor(typeof(AllHouseHoldItemData))]
public class HouseHoldItemDataInspector : Editor {

    private Vector2 scroll = Vector2.zero;

    public override void OnInspectorGUI() {
        AllHouseHoldItemData targ = (AllHouseHoldItemData)target;

        targ.AlertAll();

        Undo.RegisterCompleteObjectUndo(targ, "Changed House hold item data");

        Rect r = EditorGUILayout.BeginHorizontal();
        EditorGUILayout.EndHorizontal();
        r.height = EditorGUIUtility.singleLineHeight;

        Rect scrollRect = new Rect() { width = r.width+18, height = 200, position = new Vector2(0,50)};
        Rect viewRect = new Rect() { width = scrollRect.width-15, height = (70 * (targ.data.Count+1)) + 2};

        GUI.Box(new Rect(0, scrollRect.position.y-1, scrollRect.width + EditorGUIUtility.singleLineHeight*2, scrollRect.height+2), "");

        scroll = GUI.BeginScrollView(scrollRect, scroll, viewRect, false, true);
        float heightWas = EditorStyles.toolbar.fixedHeight;
        EditorStyles.toolbar.fixedHeight = 0;
        {
            int itemY = 0;
            /*
            Rect itemRect = new Rect(18, itemY + 4, viewRect.width-18, 64);
            GUI.Box(itemRect, "");
            itemRect.height -= 2;
            itemRect.width -= 1;
            itemRect.position += new Vector2(1, 1);
            */

            for(int i=0; i<targ.data.Count; i++){
                if(i >= targ.data.Count){ break; }
                itemY = i * 70;

                Rect itemRect = new Rect(18, itemY + 4, viewRect.width - 18, 64);
                GUI.Box(itemRect, "");
                itemRect.height -= 2;
                itemRect.width -= 1;
                itemRect.position += new Vector2(1, 1);

                GUI.Box(itemRect, "", EditorStyles.toolbar);

                Rect removeRect = itemRect;
                removeRect.width = 17;
                removeRect.height = 18;
                removeRect.position -= new Vector2(21, 1);

                EditorGUI.ProgressBar(removeRect, 1, "x");
                if(GUI.Button(removeRect, "", GUIStyle.none)){
                    targ.data.Remove(targ.data[i]);
                    break;
                }

                Rect previewRect = new Rect(itemRect.position + new Vector2(2,2), new Vector2(58, 58));

                Rect objGUI = previewRect;
                objGUI.width += 18;

                targ.data[i].prefab = EditorGUI.ObjectField(objGUI, "", targ.data[i].prefab, typeof(GameObject), false) as GameObject;

                if (targ.data[i].prefab != null) {
                    //Editor objEditor = Editor.CreateEditor(targ.data[i].prefab);
                    //objEditor.OnInteractivePreviewGUI(previewRect, GUIStyle.none);
                    //DestroyImmediate(objEditor);
                    GUI.Box(previewRect, AssetPreview.GetAssetPreview(targ.data[i].prefab), GUIStyle.none);
                }
                else{
                    GUI.Box(previewRect, "");
                    previewRect.position += new Vector2(12, 22);
                    GUI.Label(previewRect, "none");
                    previewRect.position -= new Vector2(12, 22);
                }

                objGUI.width = 20;
                objGUI.position += new Vector2(57, 0);
                //EditorGUI.ProgressBar(objGUI, 1, "");
                GUI.Box(objGUI, EditorGUIUtility.IconContent("Prefab Icon"));

                Rect nameRect = previewRect;
                nameRect.height = 18;
                nameRect.position += new Vector2(80, 0);
                nameRect.width = 150;

                targ.data[i].name = EditorGUI.TextField(nameRect, targ.data[i].name);

                Rect descriptionRect = nameRect;
                descriptionRect.position += new Vector2(160, 0);
                descriptionRect.height = 58;

                targ.data[i].description = EditorGUI.TextArea(descriptionRect, targ.data[i].description);


                { 
                    Rect upgradeLineRect = nameRect;
                    upgradeLineRect.height = 22;
                    upgradeLineRect.width = 22;
                    upgradeLineRect.position += new Vector2(0, 
                    20//36
                    );

                    var down = targ.data[i].DownGrade;
                    if(down != null){
                        if (down.prefab != null) {
                            GUI.Box(upgradeLineRect, AssetPreview.GetAssetPreview(targ.data[i].DownGrade.prefab), GUIStyle.none);
                        }
                        else{
                            GUI.Box(upgradeLineRect, "");
                        }
                        if (GUI.Button(upgradeLineRect, "", GUIStyle.none)) {
                            MonoBehaviour.print("Implement this...");
                        }
                    }
                    else{
                        GUI.Box(upgradeLineRect, "x");
                        if (GUI.Button(upgradeLineRect, "", GUIStyle.none)) {
                            MonoBehaviour.print("Implement this...");
                        }
                    }
                    upgradeLineRect.position += new Vector2(63, 0);
                    if(targ.data[i].prefab != null) {
                        GUI.Box(upgradeLineRect, AssetPreview.GetAssetPreview(targ.data[i].prefab), GUIStyle.none);
                    }
                    else{
                        GUI.Box(upgradeLineRect, "");
                    }
                    upgradeLineRect.position += new Vector2(64, 0);

                    var up = targ.data[i].Upgrade;
                    if(up != null){
                        if (up.prefab != null) {
                            GUI.Box(upgradeLineRect, AssetPreview.GetAssetPreview(targ.data[i].Upgrade.prefab), GUIStyle.none);
                        }
                        else{
                            GUI.Box(upgradeLineRect, "");
                        }
                        if(GUI.Button(upgradeLineRect, "", GUIStyle.none)){
                            MonoBehaviour.print("Implement this...");
                        }
                    }
                    else{ 
                        GUI.Box(upgradeLineRect, "x");
                        if (GUI.Button(upgradeLineRect, "", GUIStyle.none)) {
                            MonoBehaviour.print("Implement this...");
                        }
                    }

                    Rect arrowRect = upgradeLineRect;
                    arrowRect.position -= new Vector2(30, -2);
                    GUI.Label(arrowRect, "->");

                    arrowRect.position -= new Vector2(64, 0);
                    GUI.Label(arrowRect, "->");

                    //Rect editRect = itemRect;
                    //editRect.width = 40;
                    //editRect.height = 16;
                    //editRect.position += new Vector2(itemRect.width - editRect.width, itemRect.height - editRect.height-2);
                    Rect editRect = nameRect;
                    editRect.position += new Vector2(0, 44);
                    editRect.height = 14;
                    EditorGUI.ProgressBar(editRect, 1, "");

                    if(GUI.Button(editRect, "", GUIStyle.none)){
                        HouseHoldItemDataEditorWindow.currentEditing = targ.data[i];
                        HouseHoldItemDataEditorWindow.OpenWindow();
                    }

                    editRect.height = 18;
                    editRect.position += new Vector2(60, -2);
                    GUI.Label(editRect, "edit", EditorStyles.miniLabel);

                }

            }

            {
                itemY = targ.data.Count;
                Rect itemRect = new Rect(18, itemY * 70 + 4, 80, 18);

                EditorGUI.ProgressBar(itemRect, 1, "new");
                if(GUI.Button(itemRect, "", GUIStyle.none)){
                    targ.data.Add(new HouseHoldItemData(targ));
                }

            }

        }
        EditorStyles.toolbar.fixedHeight = heightWas;
        GUI.EndScrollView(true);

    }

}
#endif
