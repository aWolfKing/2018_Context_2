using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class ShopManager : MonoBehaviour {

    private static ShopManager _this = null;

    [SerializeField] private Vector3 defaultItemSpawnPosition = Vector3.zero;


    public delegate void ShopItemCreate(HouseHoldItemData data);


    private void OnEnable() {
        _this = this;
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
        GameObject obj = GameObject.Instantiate(data.prefab);
        obj.name = data.name;
        obj.transform.position = _this.defaultItemSpawnPosition;
        var placementObj = obj.AddComponent<PlacementObject>();
        Vector2 wd = CalculateWidthAndDepth(data);
        placementObj.width = wd.x;
        placementObj.depth = wd.y;
        var houseHoldItem = obj.AddComponent<HouseHoldItem_monobehaviour>();
        houseHoldItem.SetHouseHoldItemDataID(data.ID);
    }


    private static Vector2 CalculateWidthAndDepth(HouseHoldItemData data){
        return Vector2.one;
    }


}
