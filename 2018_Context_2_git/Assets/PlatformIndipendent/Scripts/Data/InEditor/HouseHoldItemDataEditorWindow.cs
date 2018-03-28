#if UNITY_EDITOR
using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEditor;

public class HouseHoldItemDataEditorWindow : EditorWindow {

    private static System.Diagnostics.Stopwatch stopwatch = new System.Diagnostics.Stopwatch();

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

    private static int selectedTab = 0;
    private static int selectedTabWas = 0;
    private static float fSelectedTab = 0;

    private static EditorWindow thisWindow = null;

    private static Vector2 scroll0 = Vector2.zero;


    [MenuItem("Tools/HouseHoldItemData")]
    public static void OpenWindow(){
        thisWindow = HouseHoldItemDataEditorWindow.GetWindowWithRect<HouseHoldItemDataEditorWindow>(new Rect(100, 100, 400, 600), false, "HouseHoldItem-Data", true);
        EditorApplication.update -= ScrollToTabs;
        EditorApplication.update += ScrollToTabs;
    }

    private void OnDestroy() {
        EditorApplication.update -= ScrollToTabs;
    }

    private static void ScrollToTabs(){
        if(!stopwatch.IsRunning){ stopwatch.Start(); }
        if(stopwatch.ElapsedMilliseconds >= 3.33f*2f){
            stopwatch.Reset();
            fSelectedTab = Mathf.MoveTowards(fSelectedTab, selectedTab, 0.033f*2f * Mathf.Abs(selectedTabWas-selectedTab));
            thisWindow.Repaint();
        }
    }

    private void OnGUI() {

        Data.AlertAll();

        Undo.RegisterCompleteObjectUndo(Data, "Changed House hold item data");
        //Undo.RecordObject(Data, "Changed House hold item data");

        if (currentEditing != null){

            Rect widthRect = EditorGUILayout.BeginHorizontal();
            EditorGUILayout.EndHorizontal();

            Rect nameRect = widthRect;
            nameRect.height = 18;
            GUI.Box(nameRect, "");
            EditorGUI.IntField(nameRect, currentEditing.ID);

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
                if(GUI.Button(previewRect, "", GUIStyle.none)){
                    var a = currentEditing;
                    HouseHoldItemDataInspector.RequestSelection(
                        (HouseHoldItemData d) => {
                            a.upgradesToId = d.ID;
                        }
                    );
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
                GUI.Label(nameRect, "Category", EditorStyles.boldLabel);
                nameRect.position += new Vector2(0, 18);
                {
                    Rect tmpRect = nameRect;
                    tmpRect.width -= 90;
                    currentEditing.category = EditorGUI.Popup(tmpRect, currentEditing.category, Data.categories.ToArray());
                    tmpRect.position += new Vector2(tmpRect.width, 0);
                    tmpRect.width = 90;
                    tmpRect.height -= 2;
                    float heightWas = EditorStyles.toolbarButton.fixedHeight;
                    EditorStyles.toolbarButton.fixedHeight = 0;
                    if(GUI.Button(tmpRect, "edit categories", EditorStyles.toolbarButton)){
                        CategoryEditorWindow.GetWindowWithRect<CategoryEditorWindow>(new Rect(80, 110, 400, 600), false, "Category-Editor", true);
                    }
                    EditorStyles.toolbarButton.fixedHeight = heightWas;
                }

                nameRect.position -= new Vector2(fSelectedTab * 430, 0);

                //info
                nameRect.position += new Vector2(0, 25);
                GUI.Label(nameRect, "In-Game Description", EditorStyles.boldLabel);
                nameRect.position += new Vector2(0, 18);
                nameRect.height = 90;
                currentEditing.description = EditorGUI.TextArea(nameRect, currentEditing.description);

            }



            //info
            {

                Rect purchaseRect = new Rect(0, 349, widthRect.width, 18);
                purchaseRect.position -= new Vector2(fSelectedTab * 430, 0);

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

                GUI.Label(purchaseRect, "Energy", EditorStyles.boldLabel);
                purchaseRect.position += new Vector2(0, 18);
                purchaseRect.width -= 20;
                purchaseRect.position += new Vector2(20, 0);
                currentEditing.electricityUsage = EditorGUI.FloatField(purchaseRect, "Usage", currentEditing.electricityUsage);

                purchaseRect.position += new Vector2(0, 18);
                currentEditing.standardEffectRadius = EditorGUI.Slider(purchaseRect, "std Effect radius", currentEditing.standardEffectRadius, 0, 100);


            }



            //affects
            {
                Rect mainRect = widthRect;
                mainRect.position += new Vector2(430-(fSelectedTab*430), 232);

                Rect titleRect = mainRect;
                titleRect.height = 18;
                GUI.Label(titleRect, "Affects", EditorStyles.boldLabel);

                Rect scrollRect = titleRect;
                scrollRect.position += new Vector2(0, 18);
                scrollRect.height = 300;

                Rect viewRect = new Rect() { width = scrollRect.width - 16, height = 94 * (currentEditing.affectedObjects.Count + 1) };

                GUI.Box(scrollRect, "");
                scroll0 = GUI.BeginScrollView(scrollRect, scroll0, viewRect, false, true);

                Rect itemRect = new Rect() { width = 367, height = 90, position = new Vector2(20,4)};

                for(int i=0; i<currentEditing.affectedObjects.Count; i++)
                {

                    GUI.Box(itemRect, "");
                    itemRect.height -= 2;
                    itemRect.position += new Vector2(1, 1);
                    float heightWas = EditorStyles.toolbar.fixedHeight;
                    EditorStyles.toolbar.fixedHeight = 0;
                    GUI.Box(itemRect, "", EditorStyles.toolbar);
                    itemRect.height += 2;
                    itemRect.position -= new Vector2(1, 1);
                    EditorStyles.toolbar.fixedHeight = heightWas;

                    Rect removeRect = new Rect() { width = 16, height = 18, position = itemRect.position - new Vector2(20, 0) };
                    EditorGUI.ProgressBar(removeRect, 1f, "x");
                    if (GUI.Button(removeRect, "", GUIStyle.none)) {
                        currentEditing.affectedObjects.RemoveAt(i);
                        break;
                    }

                    Rect affectedObject = new Rect() { width = 62, height = 62, position = itemRect.position + new Vector2(4, 4) };

                    Texture objTxt = null;
                    var obj = Data.GetDataFromID(currentEditing.affectedObjects[i].affectedObjectId);
                    if(obj != null){
                        objTxt = AssetPreview.GetAssetPreview(obj.prefab);
                    }

                    if (objTxt != null) {
                        GUI.Box(affectedObject, objTxt);
                    }
                    else {
                        GUI.Box(affectedObject, "");
                        if(obj == null){
                            GUI.Label(affectedObject, "none");
                        }
                    }

                    if (GUI.Button(affectedObject, "", GUIStyle.none)) {
                        var a = currentEditing.affectedObjects[i];
                        HouseHoldItemDataInspector.RequestSelection(
                            (HouseHoldItemData d) => {
                                Undo.RegisterCompleteObjectUndo(Data, "Changed House hold item data");
                                a.affectedObjectId = d.ID;
                            }
                        );
                    }

                    Rect modifierTitle = new Rect() { width = 55, height = 18, position = itemRect.position + new Vector2(70, 4) };
                    Rect modifierSlider = modifierTitle;
                    Rect percentageLabel = modifierSlider;
                    modifierSlider.height = 14;

                    modifierSlider.position = modifierTitle.position + new Vector2(0, 16);
                    percentageLabel.position = modifierSlider.position + new Vector2(modifierSlider.width, 0);
                    GUI.Label(modifierTitle, "Summer");
                    currentEditing.affectedObjects[i].summerElectricityPercentage = EditorGUI.Slider(modifierSlider, currentEditing.affectedObjects[i].summerElectricityPercentage, -100, 100);
                    GUI.Label(percentageLabel, "% added");

                    modifierTitle.position += new Vector2(0, 30);
                    modifierSlider.position = modifierTitle.position + new Vector2(0, 16);
                    percentageLabel.position = modifierSlider.position + new Vector2(modifierSlider.width, 0);
                    GUI.Label(modifierTitle, "Fall");
                    currentEditing.affectedObjects[i].fallElectricityPercentage = EditorGUI.Slider(modifierSlider, currentEditing.affectedObjects[i].fallElectricityPercentage, -100, 100);
                    GUI.Label(percentageLabel, "% added");

                    modifierTitle.position += new Vector2(140, -30);
                    modifierSlider.position = modifierTitle.position + new Vector2(0, 16);
                    percentageLabel.position = modifierSlider.position + new Vector2(modifierSlider.width, 0);
                    GUI.Label(modifierTitle, "Winter");
                    currentEditing.affectedObjects[i].winterElectricityPercentage = EditorGUI.Slider(modifierSlider, currentEditing.affectedObjects[i].winterElectricityPercentage, -100, 100);
                    GUI.Label(percentageLabel, "% added");

                    modifierTitle.position += new Vector2(0, 30);
                    modifierSlider.position = modifierTitle.position + new Vector2(0, 16);
                    percentageLabel.position = modifierSlider.position + new Vector2(modifierSlider.width, 0);
                    GUI.Label(modifierTitle, "Spring");
                    currentEditing.affectedObjects[i].springElectricityPercentage = EditorGUI.Slider(modifierSlider, currentEditing.affectedObjects[i].springElectricityPercentage, -100, 100);
                    GUI.Label(percentageLabel, "% added");

                    Rect radiusLabel = new Rect() { width = 80, height = 18, position = itemRect.position + new Vector2(2, 68) };
                    GUI.Label(radiusLabel, "Effect radius");
                    radiusLabel.position += new Vector2(radiusLabel.width+20, 0);
                    radiusLabel.width = 250;
                    currentEditing.affectedObjects[i].effectRadius = EditorGUI.Slider(radiusLabel, currentEditing.affectedObjects[i].effectRadius, 0, 100);

                    itemRect.position += new Vector2(0, 94);

                }


                itemRect.height = 18;
                itemRect.width = 70;
                EditorGUI.ProgressBar(itemRect, 1, "new");
                if(GUI.Button(itemRect, "", GUIStyle.none)){
                    var nItem = new HouseHoldItemData.AffectsObject();
                    nItem.effectRadius = currentEditing.standardEffectRadius;
                    currentEditing.affectedObjects.Add(nItem);
                }


                GUI.EndScrollView(true);

            }

            //texture
            {

                Rect mainRect = widthRect;
                mainRect.position += new Vector2((430*2) - (fSelectedTab * 430), 260);

                mainRect.position += new Vector2(mainRect.width * 0.5f - 50, 0);
                mainRect.width = 100;
                mainRect.height = 100;

                currentEditing.sprite = EditorGUI.ObjectField(mainRect, "", currentEditing.sprite, typeof(Texture2D), false) as Texture2D;

            }





            Rect tabbar = new Rect(){
                width = widthRect.width,
                height = 40,
                position = new Vector2(0, 600-38)
            };
            GUI.Box(tabbar, "");

            {
                float heightWas = EditorStyles.toolbarButton.fixedHeight;
                EditorStyles.toolbarButton.fixedHeight = 0;
                int tWas = selectedTab;
                selectedTab = GUI.Toolbar(tabbar, selectedTab, new string[] { "info", "affects", "sprite" }, EditorStyles.toolbarButton);
                if(tWas != selectedTab){
                    selectedTabWas = tWas;
                }
                EditorStyles.toolbarButton.fixedHeight = heightWas;
            }

        }
    }


}
#endif