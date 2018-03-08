using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class ProceduralUIComponent : MonoBehaviour {

	[System.Serializable]
    public class UIElement{
        public Texture texture;
        public Rect rect = new Rect(0, 0, 192, 108);
        public bool visible = true;
        public int zIndex = 0;
        public Vector2 uvScale = Vector2.one;
        public Vector2 uvOffset = Vector2.zero;
    }
    
    [System.Serializable]
    public class UIMenu{
        public string name = "menu name";
        public List<UIElement> elements = new List<UIElement>();
        public bool visible = true;
    }

    public List<UIMenu> menus = new List<UIMenu>();

}
