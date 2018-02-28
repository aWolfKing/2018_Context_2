#if UNITY_EDITOR
using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEditor;

public class HouseHoldItemDataEditorWindow : EditorWindow {

    private static AllHouseHoldItemData m_allHouseHoldItemData = null;
    public static AllHouseHoldItemData Data{
        get{
            if(m_allHouseHoldItemData == null){
                m_allHouseHoldItemData = AssetDatabase.LoadAssetAtPath<AllHouseHoldItemData>(System.IO.Path.Combine(Data_DataPath.PathRelativeToAssets, "AllHouseHoldItemData.asset"));
            }
            return m_allHouseHoldItemData;
        }
    }

    public static HouseHoldItemData currentEditing = null;


    [MenuItem("Tools/HouseHoldItemData")]
    public static void OpenWindow(){
        HouseHoldItemDataEditorWindow.GetWindowWithRect<HouseHoldItemDataEditorWindow>(new Rect(100, 100, 400, 600), false, "HouseHoldItem-Data", true);
    }

    private void OnGUI() {

        Data.AlertAll();

        Undo.RegisterCompleteObjectUndo(Data, "Changed House hold item data");

        if (currentEditing != null){

            Rect widthRect = EditorGUILayout.BeginHorizontal();
            EditorGUILayout.EndHorizontal();

            Rect nameRect = widthRect;
            nameRect.height = 18;
            GUI.Box(nameRect, "");


            { 

                Rect previewRect = new Rect(widthRect.width/2-55, 25, 110, 110);
                previewRect.position -= new Vector2(9, 0);

                previewRect.width += 18;
                currentEditing.prefab = EditorGUI.ObjectField(previewRect, "", currentEditing.prefab, typeof(GameObject), false) as GameObject;
                previewRect.width -= 18;

                if(currentEditing.prefab != null){
                    GUI.Box(previewRect, AssetPreview.GetAssetPreview(currentEditing.prefab), GUIStyle.none);
                }
                else{ 
                    GUI.Box(previewRect, "");
                }
                previewRect.width = 20;
                previewRect.position += new Vector2(110, 0);
                GUI.Box(previewRect, EditorGUIUtility.IconContent("Prefab Icon"));

                previewRect = new Rect(15, 40, 80, 80);

                Rect upgradeText = previewRect;
                upgradeText.height = 18;
                upgradeText.position -= new Vector2(0, 18);
                upgradeText.width += 30;
                GUI.Label(upgradeText, "Upgrades From", EditorStyles.miniBoldLabel);

                var up_or_down_grade = currentEditing.DownGrade;
                if(up_or_down_grade != null){ 
                    if(up_or_down_grade.prefab != null){
                        GUI.Box(previewRect, AssetPreview.GetAssetPreview(up_or_down_grade.prefab), GUIStyle.none);
                    }
                    else{
                        GUI.Box(previewRect, "");
                    }
                }
                else{ 
                    GUI.Box(previewRect, "");
                    Rect xRect = previewRect;
                    xRect.width = 36;
                    xRect.position += new Vector2(45 - 20, 45-14);
                    GUI.Label(xRect, "none");
                }


                previewRect.position = new Vector2(widthRect.width - 40 - 55, previewRect.position.y);

                upgradeText = previewRect;
                upgradeText.height = 18;
                upgradeText.position -= new Vector2(-6, 18);
                upgradeText.width += 30;
                GUI.Label(upgradeText, "Upgrades To", EditorStyles.miniBoldLabel);

                up_or_down_grade = currentEditing.Upgrade;
                if (up_or_down_grade != null) {
                    if (up_or_down_grade.prefab != null) {
                        GUI.Box(previewRect, AssetPreview.GetAssetPreview(up_or_down_grade.prefab), GUIStyle.none);
                    }
                    else {
                        GUI.Box(previewRect, "");
                    }
                }
                else {
                    GUI.Box(previewRect, "");
                    Rect xRect = previewRect;
                    xRect.width = 36;
                    xRect.position += new Vector2(45 - 20, 45 - 14);
                    GUI.Label(xRect, "none");
                }

                upgradeText = previewRect;
                upgradeText.height = 18;
                upgradeText.position += new Vector2(0, previewRect.height);
                GUI.Label(upgradeText, "(€" + (up_or_down_grade != null ? currentEditing.upgradeCost.ToString() : "0") + ")", EditorStyles.miniLabel);

                Rect arrowRect = new Rect(106, 70, 30, 18);
                GUI.Label(arrowRect, "->", EditorStyles.boldLabel);
                arrowRect.position += new Vector2(172, 0);
                GUI.Label(arrowRect, "->", EditorStyles.boldLabel);


            }


            {

                nameRect.position += new Vector2(0, 125);

                nameRect.position += new Vector2(0, 20);
                GUI.Label(nameRect, "Name", EditorStyles.boldLabel);
                nameRect.position += new Vector2(0, 18);
                currentEditing.name = EditorGUI.TextField(nameRect, currentEditing.name);

                nameRect.position += new Vector2(0, 25);
                GUI.Label(nameRect, "In-Game Description", EditorStyles.boldLabel);
                nameRect.position += new Vector2(0, 18);
                nameRect.height = 90;
                currentEditing.description = EditorGUI.TextArea(nameRect, currentEditing.description);

            }


            {

                Rect purchaseRect = new Rect(0, 306, widthRect.width, 18);
                GUI.Label(purchaseRect, "Costs", EditorStyles.boldLabel);

                purchaseRect.width -= 20;
                purchaseRect.position += new Vector2(20, 0);

                purchaseRect.position += new Vector2(0, 18);
                currentEditing.purchaseCost = EditorGUI.FloatField(purchaseRect, "Purchase", currentEditing.purchaseCost);

                purchaseRect.position += new Vector2(0, 18);
                currentEditing.repairCost = EditorGUI.FloatField(purchaseRect, "Repair", currentEditing.repairCost);

                purchaseRect.position += new Vector2(0, 18);
                currentEditing.upgradeCost = EditorGUI.FloatField(purchaseRect, "Upgrade", currentEditing.upgradeCost);

                purchaseRect.width += 20;
                purchaseRect.position -= new Vector2(20, 0);

                purchaseRect.position += new Vector2(0, 25);

                GUI.Label(purchaseRect, "Electricity", EditorStyles.boldLabel);
                purchaseRect.position += new Vector2(0, 18);
                purchaseRect.width -= 20;
                purchaseRect.position += new Vector2(20, 0);
                currentEditing.electricityUsage = EditorGUI.FloatField(purchaseRect, "Usage", currentEditing.electricityUsage);

            }


        }
    }

}
#endif