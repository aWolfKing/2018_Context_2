using System.Collections;
using System.Collections.Generic;
using UnityEngine;

[System.Serializable]
public class HouseHoldItemData {

    [System.NonSerialized] AllHouseHoldItemData container = null;

    [SerializeField][HideInInspector] private int m_id = -1;
    public int ID{ get{ return m_id; } }

    public GameObject prefab = null;

    public string name = "name";
    public int category = 0;
    public string description = "description";

    public float purchaseCost = 0;
    public float electricityUsage = 0;
    public float breakPercentage = 0;
    public float repairCost = 0;

    public int upgradesToId = -1;               //id of the object this object upgrades to
    public float upgradeCost = 0;

    [System.NonSerialized] private HouseHoldItemData    downGrade   = null,
                                                        upgrade     = null;
    public HouseHoldItemData DownGrade{
        get{
            if(downGrade == null){
                foreach(var d in container.data){
                    if(d.upgradesToId == ID){
                        downGrade = d;
                    }
                }
            }
            return downGrade;
        }
    }
    public HouseHoldItemData Upgrade{
        get{
            if(upgrade == null && upgradesToId >= 0){
                upgrade = container.GetDataFromID(upgradesToId);
            }
            return upgrade;
        }
    } 


    public float standardEffectRadius = 100;
    public List<AffectsObject> affectedObjects = new List<AffectsObject>();

    [System.Serializable]
    public class AffectsObject{
        public int affectedObjectId = -1;
        public float summerElectricityPercentage = 0;
        public float fallElectricityPercentage = 0;
        public float winterElectricityPercentage = 0;
        public float springElectricityPercentage = 0;
        public float effectRadius = 0;
    }



    public HouseHoldItemData(AllHouseHoldItemData allHouseHoldItemData){
        this.m_id = GetValidId(allHouseHoldItemData);
    }


    private int GetValidId(AllHouseHoldItemData allHouseHoldItemData) {
        int t_id = Random.Range(0, 1000);
        bool validId = false;
        do {
            bool broke = false;
            for (int i = 0; i < allHouseHoldItemData.data.Count; i++) {
                if (allHouseHoldItemData.data[i].ID == t_id && allHouseHoldItemData.data[i] != this) {
                    broke = true;
                    break;
                }
            }
            if (!broke) {
                validId = true;
                break;
            }
            else {
                t_id++;
            }
        } while (!validId);
        return t_id;
    }


    public void Alert(AllHouseHoldItemData h){
        container = h;
    }

}
