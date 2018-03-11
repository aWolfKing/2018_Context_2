using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public enum Season{
    Summer, Fall, Winter, Spring
}

public class MainGameManager : MonoBehaviour {

    private static List<HouseHoldItem_monobehaviour> allObjects = new List<HouseHoldItem_monobehaviour>();
    private static MainGameManager _this = null;
    [SerializeField] private AllHouseHoldItemData allHouseHoldItemData = null;
    public static AllHouseHoldItemData Data{
        get{
            _this = GameObject.FindObjectOfType<MainGameManager>();
            if (_this == null) {
                GameObject obj = new GameObject("GameManager");
                obj.transform.position = Vector3.zero;
                _this = obj.AddComponent<MainGameManager>();
            }
            if(_this.allHouseHoldItemData == null){
                Debug.LogError(@"Please create a ""MainGameManager"" and assign ""allHouseHoldItemData"".");
            }
            return _this.allHouseHoldItemData;
        }
    }
    [SerializeField]private Season currentSeason = Season.Summer;
    public static Season CurrentSeason{
        get{
            return _this.currentSeason;
        }
    }

    
    [RuntimeInitializeOnLoadMethod]
    private static void Init(){
        _this = GameObject.FindObjectOfType<MainGameManager>();
        if (_this == null) {
            GameObject obj = new GameObject("GameManager");
            obj.transform.position = Vector3.zero;
            _this = obj.AddComponent<MainGameManager>();
        }
    }
    
    
    public static void AddObject(HouseHoldItem_monobehaviour obj){
        if(!allObjects.Contains(obj)){
            allObjects.Add(obj);
        }
    }
    public static void RemoveObject(HouseHoldItem_monobehaviour obj){
        if(allObjects.Contains(obj)){
            allObjects.Remove(obj);
        }
    }


    public static float CalculateEnergyCosts(){
        float ret = 0;
        foreach(var o in allObjects){
            o.HouseHoldItemData.Alert(Data);
            o.ResetInfluence();
        }

        HouseHoldItem_monobehaviour currObject = allObjects[0];
        for(int i=0; i<allObjects.Count; i++, currObject = allObjects[i]){
            var affectedObjects = currObject.HouseHoldItemData.affectedObjects;

            List<int> affectedIDs = new List<int>();
            foreach(var aO in affectedObjects){ affectedIDs.Add(aO.affectedObjectId); }

            HouseHoldItem_monobehaviour otherObject = allObjects[0];
            for(int o=0; o<allObjects.Count; o++, otherObject = allObjects[o]){
                if(currObject != otherObject){
                    if(affectedIDs.Contains(otherObject.HouseHoldItemData.ID)){

                        var effect = affectedObjects[affectedIDs.IndexOf(otherObject.HouseHoldItemData.ID)];
                        if(Vector3.Distance(currObject.transform.position, otherObject.transform.position) <= effect.effectRadius){

                            switch(CurrentSeason){
                                case Season.Summer:
                                    otherObject.AddInfluence(effect.summerElectricityPercentage);
                                    break;
                                case Season.Fall:
                                    otherObject.AddInfluence(effect.fallElectricityPercentage);
                                    break;
                                case Season.Winter:
                                    otherObject.AddInfluence(effect.winterElectricityPercentage);
                                    break;
                                case Season.Spring:
                                    otherObject.AddInfluence(effect.springElectricityPercentage);
                                    break;
                            }

                        }

                    }
                }
            }
        }

        foreach(var o in allObjects){
            ret += o.CalculateTotalEnergyCost();
        }

        MonoBehaviour.print("This seasons energy cost: " + ret);

        return ret;
    }

}
