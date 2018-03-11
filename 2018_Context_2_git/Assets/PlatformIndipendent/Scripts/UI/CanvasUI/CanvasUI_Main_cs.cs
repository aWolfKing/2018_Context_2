using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class CanvasUI_Main_cs : MonoBehaviour {

    [System.Serializable]
    public class ShopOptions{
        public Transform shopUI;
        /*From open to closed*/
        public AnimationCurve xPositionAnimation;
        public float animationDuration = 0.6f;
        public Dropdown categoryDropDown;
    }


    [SerializeField] private ShopOptions shopOptions = new ShopOptions();

    private void OnEnable() {

        /*Set category dropdown*/
        {
            List<Dropdown.OptionData> categories = new List<Dropdown.OptionData>();
            for (int i = 0; i < MainGameManager.Data.categories.Count; i++){
                categories.Add(new Dropdown.OptionData(MainGameManager.Data.categories[i]));
            }
            shopOptions.categoryDropDown.options = categories;
        }

    }

    public void OpenShop(){
        StartCoroutine(OpenOrCloseShop(-1));
    }
    public void CloseShop(){
        StartCoroutine(OpenOrCloseShop(1));
    }

    /// <summary>
    /// 
    /// </summary>
    /// <param name="direction">Positive : close, Negative : open</param>
    /// <returns></returns>
    private IEnumerator OpenOrCloseShop(int direction){
        float s = (1f/shopOptions.animationDuration);
        float t = direction * 0.5f + 0.5f;
        float i = 1 - t;
        var wait = new WaitForFixedUpdate();
        do {
            Vector3 p = shopOptions.shopUI.localPosition;
            p.x = shopOptions.xPositionAnimation.Evaluate(Mathf.Clamp01(i));
            shopOptions.shopUI.localPosition = p;
            yield return wait;
            i = Mathf.Clamp01(i + (direction * Time.fixedDeltaTime * s));
        }
        while (i != t);
    }

}
