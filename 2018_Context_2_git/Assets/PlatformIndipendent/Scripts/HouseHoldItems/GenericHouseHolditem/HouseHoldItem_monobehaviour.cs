using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class HouseHoldItem_monobehaviour : MonoBehaviour {

    private float influencePercent = 0;
    [System.NonSerialized] private HouseHoldItemData houseHoldItemData = null;
    [SerializeField]
    //[HideInInspector] 
    private int houseHoldItemDataID = 0;
    public HouseHoldItemData HouseHoldItemData{
        get{
            return MainGameManager.Data.GetDataFromID(houseHoldItemDataID);
        }
    }

    private void OnEnable() {
        MainGameManager.AddObject(this);
    }

    private void OnDisable() {
        MainGameManager.RemoveObject(this);
    }

    public void SetHouseHoldItemDataID(int id){
        houseHoldItemDataID = id;
    }

    public void AddInfluence(float percentAdded){
        influencePercent = Mathf.Clamp(influencePercent+percentAdded,0,100000);
    }
    public void ResetInfluence(){
        influencePercent = 0;
    }


    public float CalculateTotalEnergyCost(){
        float ret =

        HouseHoldItemData.electricityUsage * ((influencePercent*0.01f)+1f);

        return ret;
    }


}
