using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class ShopManager : MonoBehaviour {

    private static ShopManager _this = null;

    [SerializeField] private Vector3 defaultItemSpawnPosition = Vector3.zero;


    public delegate void ShopItemCreate(HouseHoldItemData data);

    [System.Serializable]
    public class ShopItemPreset_UI{
        public Transform holder_Parent = null;
        public GameObject preset = null;
        public Text name_Text = null;
        public Button buyButton = null;
        public Text button_Text = null;
        public Transform previewTransform = null;
        public float shopItemHeight = 0.32f;
        public float distBetweenItems = 0.05f;
        public float containerHeight = 1.0827f;
        public Scrollbar containerSlider = null;
        public float maxPreviewUnscaledSize = 1f;
        public float previewScale = 0.3f;
    }
    [SerializeField] private ShopItemPreset_UI shopItemPreset_UI = new ShopItemPreset_UI();


    private Dictionary<HouseHoldItemData, Button> canBuy = new Dictionary<HouseHoldItemData, Button>();
    private List<Transform> previews = new List<Transform>();

    [SerializeField] private Collider placementHelp = null;

    private HouseHoldItem_monobehaviour currently_buying = null;
    public static bool IsPlacing{
        get{
            return _this.currently_buying != null;
        }
    }


    private void OnEnable() {
        _this = this;
        shopItemPreset_UI.preset.SetActive(false);
    }

    private void FixedUpdate() {
        foreach(var pair in canBuy){
            if(CanBuy(pair.Key)){
                pair.Value.interactable = true;
            }
            else{
                pair.Value.interactable = false;
            }
        }    
        foreach(var preview in previews){
            preview.Rotate(Vector3.up * Time.fixedDeltaTime * 40);
        }
    }


    public static void OnOpenShop(){
        ChangeCategory(CanvasUI_Main_cs.GetDefaultCategory());
    }

    public static void OnCloseShop(){

    }

    public static void ChangeCategory(string category){

        for(int i=0; i<_this.shopItemPreset_UI.holder_Parent.childCount; i++){
            Transform child = _this.shopItemPreset_UI.holder_Parent.GetChild(i);
            if(child.gameObject != _this.shopItemPreset_UI.preset){
                GameObject.Destroy(child.gameObject);
            }
        }

        _this.canBuy.Clear();

        for(int i=_this.previews.Count-1; i>=0; i--){
            GameObject.Destroy(_this.previews[i].gameObject);
        }
        _this.previews.Clear();

        List<HouseHoldItemData> itemsInThisCategory = new List<HouseHoldItemData>();
        for(int i=0; i<MainGameManager.Data.data.Count; i++){
            if(MainGameManager.Data.categories[MainGameManager.Data.data[i].category] == category){
                itemsInThisCategory.Add(MainGameManager.Data.data[i]);
            }
        }

        float totalHeight = (itemsInThisCategory.Count * _this.shopItemPreset_UI.shopItemHeight) + (((itemsInThisCategory.Count -1) > 0 ? (itemsInThisCategory.Count - 1) : 0) * _this.shopItemPreset_UI.distBetweenItems);

        _this.shopItemPreset_UI.containerSlider.size = 1f / totalHeight * _this.shopItemPreset_UI.containerHeight;
        _this.shopItemPreset_UI.containerSlider.value = 0;

        for(int i=0; i<itemsInThisCategory.Count; i++){

            GameObject holder = GameObject.Instantiate(_this.shopItemPreset_UI.preset, _this.shopItemPreset_UI.holder_Parent);
            holder.name = @"ItemHolder for """ + itemsInThisCategory[i].name + @"""";
            holder.transform.localPosition = _this.shopItemPreset_UI.preset.transform.localPosition + new Vector3(0, -1 * ((_this.shopItemPreset_UI.shopItemHeight*i) + (_this.shopItemPreset_UI.distBetweenItems * i)), 0);

            Transform previewHolder = null;

            for(int o=0; o<holder.transform.childCount; o++){
                Transform child = holder.transform.GetChild(o);
                if(child.name == _this.shopItemPreset_UI.name_Text.name){
                    child.GetComponent<Text>().text = itemsInThisCategory[i].name;
                }
                else if(child.name == _this.shopItemPreset_UI.buyButton.name){

                    Button b = child.GetComponent<Button>();

                    _this.canBuy.Add(itemsInThisCategory[i], b);
                    b.interactable = false;

                    var h = itemsInThisCategory[i];
                    b.onClick.AddListener(
                        () => { Buy(h); }
                    );

                    child.GetChild(0).GetComponent<Text>().text = "Buy (" + itemsInThisCategory[i].purchaseCost + ")";
                }
                else if(child.name == _this.shopItemPreset_UI.previewTransform.name){
                    previewHolder = child;
                }
            }

            if (itemsInThisCategory[i].prefab != null) {
                GameObject preview = GameObject.Instantiate(itemsInThisCategory[i].prefab, previewHolder);
                preview.transform.localPosition = Vector3.zero;


                /*
                Bounds b = new Bounds();
                Vector3[] verts = itemsInThisCategory[i].prefab.GetComponent<MeshFilter>().sharedMesh.vertices;
                for(int o=0; o<verts.Length; o++){
                    b.Encapsulate(verts[o]);
                }                
                */

                //float scale = 1f / (1f / _this.shopItemPreset_UI.maxPreviewUnscaledSize * Mathf.Max(b.extents.x, b.extents.y, b.extents.z)) * _this.shopItemPreset_UI.previewScale;

                Bounds b = _this.GetBounds(preview);

                float scale = _this.shopItemPreset_UI.maxPreviewUnscaledSize/(Mathf.Max(b.extents.x, b.extents.y, b.extents.z)*2)*_this.shopItemPreset_UI.previewScale;

                preview.transform.localScale = new Vector3(scale, scale, scale);

                //preview.layer = 5;

                _this.SetToLayer(5, preview);

                _this.previews.Add(preview.transform);
            }

            holder.SetActive(true);

        }

    }


    public static void GenerateShop(ShopItemCreate c){
        for(int i=0;i<MainGameManager.Data.data.Count; i++){
            c(MainGameManager.Data.data[i]);
        }
    }



    public static bool CanBuy(HouseHoldItemData data){
        return data.purchaseCost <= MainGameManager.Cash;
    }

    public static void Buy(HouseHoldItemData data){

        CanvasUI_Main_cs.RequestCloseShop();    

        GameObject obj = GameObject.Instantiate(data.prefab);
        obj.name = data.name;
        obj.transform.position = _this.defaultItemSpawnPosition;
        var placementObj = obj.AddComponent<PlacementObject>();
        Vector2 wd = CalculateWidthAndDepth(data);
        placementObj.width = wd.x;
        placementObj.depth = wd.y;
        var houseHoldItem = obj.AddComponent<HouseHoldItem_monobehaviour>();
        houseHoldItem.SetHouseHoldItemDataID(data.ID);

        _this.currently_buying = houseHoldItem;

        _this.StartCoroutine(_this.BuyCoroutine());

    }


    private IEnumerator BuyCoroutine(){
        var wait = new WaitForEndOfFrame();
        System.Diagnostics.Stopwatch stopwatch = new System.Diagnostics.Stopwatch();
        stopwatch.Start();

        string axis = "Fire1";
        string rotateAxis = "Horizontal";
        float rotateSpeed = 90f;

        float placementHelpOffset = -100;
        placementHelp.transform.position = CameraControl.RootPosition + new Vector3(0, placementHelpOffset, 0);

        float inpWas = Input.GetAxis(axis);
        do {

            currently_buying.transform.Rotate(Vector3.up * Input.GetAxis(rotateAxis) * rotateSpeed * Time.smoothDeltaTime);

            if(stopwatch.ElapsedMilliseconds >= 5){

                var cameraRay = CameraControl.Camera.ScreenPointToRay(Input.mousePosition);
                RaycastHit hit;
                Vector3 placementPos = currently_buying.transform.position;
                if(placementHelp.Raycast(new Ray(cameraRay.origin + new Vector3(0, placementHelpOffset, 0), cameraRay.direction), out hit, Mathf.Abs(1000 * placementHelpOffset))){
                    placementPos = hit.point + new Vector3(0, -placementHelpOffset, 0);
                }

                var placementDots = PlacementManager.GetPlacementDots(placementPos, currently_buying.transform.rotation, 1f, 1f);

                if(placementDots.Count > 0){
                    currently_buying.transform.position = placementPos;
                }

                var canPlace = PlacementManager.CanPlace(placementDots);
                currently_buying.visualMaterial = canPlace ? HouseHoldItem_monobehaviour.VisualMaterial.canPlace : HouseHoldItem_monobehaviour.VisualMaterial.cannotPlace;


                if(inpWas >= 0.5f && Input.GetAxis(axis) < 0.5f){

                    if(canPlace){
                        MainGameManager.Cash -= currently_buying.HouseHoldItemData.purchaseCost;
                        currently_buying.visualMaterial = HouseHoldItem_monobehaviour.VisualMaterial.normalMaterial;
                        currently_buying = null;
                        break;
                    }
                }
                stopwatch.Reset();
                stopwatch.Start();
            }
            inpWas = Input.GetAxis(axis);
            yield return wait;
        }
        while (currently_buying != null);
    }


    private static Vector2 CalculateWidthAndDepth(HouseHoldItemData data){
        Bounds b = _this.GetBounds(data.prefab);
        return new Vector2(b.extents.x, b.extents.z) * 2;
    }


    private void SetToLayer(int layer, GameObject t){

        t.layer = layer;
        for(int i=0; i<t.transform.childCount; i++){
            SetToLayer(layer, t.transform.GetChild(i).gameObject);
        }

    }


    private Bounds GetBounds(GameObject o){
        Bounds b = new Bounds();

        List<Vector3> verts = GetVertices(o);

        for(int i=0; i<verts.Count; i++){
            b.Encapsulate(verts[i]);
        }

        return b;
    }

    private List<Vector3> GetVertices(GameObject o){
        List<Vector3> ret = new List<Vector3>();
        for(int i=0; i<o.transform.childCount; i++){
            ret.AddRange(GetVertices(o.transform.GetChild(i).gameObject));
        }
        var c = o.GetComponent<MeshFilter>();
        if (c != null) {
            Vector3[] vrts = c.sharedMesh.vertices;
            for (int j = 0; j < vrts.Length; j++) {
                ret.Add(o.transform.TransformPoint(vrts[j]));
            }
        }
        return ret;
    }


}
