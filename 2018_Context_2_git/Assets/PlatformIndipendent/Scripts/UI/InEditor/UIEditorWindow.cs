#if UNITY_EDITOR
using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEditor;

public class UIEditorWindow : EditorWindow {

    private static UIEditorWindow thisWindow = null;
    private ProceduralUIComponent editing = null;
    private int editingMenuIndex = 0;
    private System.Diagnostics.Stopwatch stopwatch = new System.Diagnostics.Stopwatch();

    private Vector2 previewScroll = Vector2.zero;
    private float zoomPercentage = 1;

    private ProceduralUIComponent.UIElement editingElement = null;


    private static void Update50(){
        if(!thisWindow.stopwatch.IsRunning){ thisWindow.stopwatch.Start(); }
        if(thisWindow.stopwatch.ElapsedMilliseconds >= (1f/50)*100){
            thisWindow.Repaint();
            thisWindow.stopwatch.Reset();
            thisWindow.stopwatch.Start();
        }
    }

    public static void OpenWindow(ProceduralUIComponent c){
        thisWindow = UIEditorWindow.GetWindowWithRect<UIEditorWindow>(new Rect() { position = new Vector2(100, 100), size = new Vector2(1050, 700) }, true, "UI component", false);
        thisWindow.editing = c;
        EditorApplication.update -= Update50;
        EditorApplication.update += Update50;
    }

    private void OnDestroy() {
        EditorApplication.update -= Update50;
    }

    private void OnGUI() {

        UpdateInput();

        Undo.RegisterCompleteObjectUndo(editing, "UI Component");

        if(editingMenuIndex >= editing.menus.Count){
            editingMenuIndex = 0;
        }

        Rect windowRect = new Rect() {
            position = new Vector2(10, 10),
            size = new Vector2(800, 450)
        };

        //Color colWas = GUI.color;
        //GUI.color = new Color(1, 0.8f, 0.8f);
        GUI.Box(windowRect, "");
        //GUI.color = colWas;

        Rect itemRect = new Rect(){
            position = new Vector2(835, 10),
            size = new Vector2(195, 18)
        };


        if(editing.menus.Count == 0){
            EditorGUI.BeginDisabledGroup(true);
            GUI.Label(itemRect, "menu name", EditorStyles.boldLabel);

            itemRect.position += new Vector2(0, 18);

            EditorGUI.Popup(itemRect, 0, new string[] { "no menus available" });
            EditorGUI.EndDisabledGroup();
        }
        else{
            GUI.Label(itemRect, editing.menus[editingMenuIndex].name, EditorStyles.boldLabel);
            itemRect.position += new Vector2(0, 18);
            string[] menuNames = new string[editing.menus.Count];
            for(int i=0; i<editing.menus.Count; i++){
                menuNames[i] = editing.menus[i].name;
            }
            editingMenuIndex = EditorGUI.Popup(itemRect, editingMenuIndex, menuNames);
        }

        ProceduralUIComponent.UIMenu editingMenu = null;
        if(editing.menus.Count > editingMenuIndex){ editingMenu = editing.menus[editingMenuIndex]; }

        EditorGUI.BeginDisabledGroup(!(editing.menus.Count > 0 && editingMenu != null));

        itemRect.position += new Vector2(0, 25);
        if (editingMenu != null) {
            editingMenu.name = EditorGUI.TextField(itemRect, editingMenu.name);
        }
        else{
            EditorGUI.TextField(itemRect, "Menu name");
        }

        itemRect.position += new Vector2(0, 18);

        EditorGUI.BeginDisabledGroup(!itemRect.Contains(Event.current.mousePosition));

        if(GUI.Button(itemRect, "Remove this menu", EditorStyles.toolbarButton)){
            editing.menus.RemoveAt(editingMenuIndex);
        }

        EditorGUI.EndDisabledGroup();

        EditorGUI.EndDisabledGroup();

        itemRect.position += new Vector2(0, 32);

        if(GUI.Button(itemRect, "Add new menu", EditorStyles.toolbarButton)){
            var menu = new ProceduralUIComponent.UIMenu();
            List<string> names = new List<string>();
            for(int i=0; i<editing.menus.Count; i++){
                names.Add(editing.menus[i].name);
            }
            int m = 0;
            do {
                string nm = "Menu " + m;
                if(names.Contains(nm)){
                    m++;
                }
                else{
                    menu.name = nm;
                    break;
                }
            }
            while (true);
            editing.menus.Add(menu);
        }

        DrawUIPreview(windowRect);

        Rect elementRect = new Rect(){
            position = new Vector2(830,200),
            size = new Vector2(200,480)
        };

        DrawElementInspector(elementRect);

    }


    private void UpdateInput(){
    
    }


    private void DrawUIPreview(Rect r, bool navigation = true){
        bool rightClickMenuUsed = false;

        Undo.RegisterCompleteObjectUndo(editing, "UI Component");
        if (editingMenuIndex >= editing.menus.Count) {
            editingMenuIndex = 0;
        }
        ProceduralUIComponent.UIMenu editingMenu = null;
        if (editing.menus.Count > editingMenuIndex) { editingMenu = editing.menus[editingMenuIndex]; }

        float mp = r.width / 1920f;

        GUI.BeginClip(r, previewScroll * mp, Vector2.zero, !navigation);

        Rect bRect = new Rect() { size = new Vector2(1920, 1080)};
        bRect.position = Vector2.zero;
        bRect.size *= zoomPercentage * mp;
        Color colWas = GUI.color;
        GUI.color = new Color(1, 0.8f, 0.8f);
        GUI.Box(bRect, "");
        GUI.color = colWas;

        
        if (editingMenu != null) {

            List<Rect> rects = new List<Rect>();
            List<ProceduralUIComponent.UIElement> elements = new List<ProceduralUIComponent.UIElement>();

            List<ProceduralUIComponent.UIElement> sortedElements = new List<ProceduralUIComponent.UIElement>();
            for(int i=0; i<editingMenu.elements.Count; i++){
                if (sortedElements.Count == 0) {
                    sortedElements.Add(editingMenu.elements[i]);
                }
                else {
                    bool added = false;
                    for (int o = 0; o < sortedElements.Count; o++) {
                        if (editingMenu.elements[i].zIndex <= sortedElements[o].zIndex) {
                            sortedElements.Insert(i, editingMenu.elements[i]);
                            added = true;
                            break;
                        }
                    }
                    if(!added){
                        sortedElements.Add(editingMenu.elements[i]);
                    }
                }
            }
            
            for(int i=0; i<sortedElements.Count; i++) {
                var element = sortedElements[i];

                Rect elementRect = element.rect;
                elementRect.position *= zoomPercentage * mp;
                elementRect.size *= zoomPercentage * mp;

                Rect maskRect = elementRect;

                GUI.BeginClip(maskRect, new Vector2(maskRect.width * element.uvOffset.x, maskRect.height * element.uvOffset.y) * mp, Vector2.zero, false);
                
                elementRect.size = new Vector2(elementRect.width * element.uvScale.x, elementRect.height * element.uvScale.y);
                elementRect.position = Vector2.zero;

                element.texture = EditorGUI.ObjectField(maskRect, "", element.texture, typeof(Texture), false) as Texture;
                if (element.texture != null) {
                    GUI.DrawTexture(elementRect, element.texture);
                }
                else{
                    GUI.Box(elementRect, "");
                }

                GUI.EndClip();

                rects.Add(maskRect);
                elements.Add(element);

            }

            ProceduralUIComponent.UIElement selecting = null;
            for(int i=0; i<rects.Count; i++){
                if (rects[i].Contains(Event.current.mousePosition)) {
                    if (selecting == null || (selecting != null && selecting.zIndex <= elements[i].zIndex)) {
                        selecting = elements[i];
                    }
                }
            }
            if (selecting != null) {
                EditorGUI.DrawRect(rects[elements.IndexOf(selecting)], new Color(0,0.3f,1,0.3f));
                if(Event.current.type == EventType.ContextClick && rects[elements.IndexOf(selecting)].Contains(Event.current.mousePosition)){

                    GenericMenu menu = new GenericMenu();

                    var d = editingMenu;
                    menu.AddItem(new GUIContent("New element"), false, () => {
                        var a = new ProceduralUIComponent.UIElement();
                        for (int i = 0; i < d.elements.Count; i++) {
                            if (a.rect.Overlaps(d.elements[i].rect, true)) {
                                if (a.zIndex <= d.elements[i].zIndex) {
                                    a.zIndex = d.elements[i].zIndex + 1;
                                }
                            }
                        }
                        d.elements.Add(a);
                    });

                    var _d = selecting;
                    menu.AddItem(new GUIContent("Remove element"), false, () => {
                        d.elements.Remove(_d);
                    });

                    menu.AddItem(new GUIContent("Edit element"), false, () => {
                        editingElement = _d;
                    });

                    menu.ShowAsContext();

                    rightClickMenuUsed = true;

                }
            }

        }
        
        GUI.EndClip();
        
        if(!rightClickMenuUsed && Event.current.type == EventType.ContextClick && r.Contains(Event.current.mousePosition)){

            GenericMenu rightClickMenu = new GenericMenu();

            var d = editingMenu;
            rightClickMenu.AddItem(new GUIContent("New element"), false, () => {
                var a = new ProceduralUIComponent.UIElement();
                for(int i=0; i<d.elements.Count; i++){
                    if(a.rect.Overlaps(d.elements[i].rect, true)){
                        if(a.zIndex <= d.elements[i].zIndex){
                            a.zIndex = d.elements[i].zIndex + 1;
                        }
                    }
                }
                d.elements.Add(a);                 
            });

            rightClickMenu.AddItem(new GUIContent("Reset selection"), false, () => {
                editingElement = null;
            });

            rightClickMenu.ShowAsContext();

        }

        if(navigation){
            r.position += new Vector2(0, r.height);
            float originalWidth = r.size.x;
            r.size = new Vector2(originalWidth * 0.65f, 18);
            zoomPercentage = EditorGUI.Slider(r, zoomPercentage, 0.1f, 10);
            r.position += new Vector2(r.size.x, 0);
            r.size = new Vector2(originalWidth - r.size.x, 18);
            previewScroll = EditorGUI.Vector2Field(r, "", previewScroll);
        }

    }


    private void DrawElementInspector(Rect r){
        Undo.RegisterCompleteObjectUndo(editing, "UI Component");
        GUI.Box(r, "");

        r.height = 18;

        if (editingElement != null) {
            editingElement.rect = EditorGUI.RectField(r, "Rect:", editingElement.rect);
            r.position += new Vector2(0, 18 * 4);
            editingElement.zIndex = EditorGUI.IntField(r, "Z-index", editingElement.zIndex);
        }
        else{

        }

    }

}
#endif