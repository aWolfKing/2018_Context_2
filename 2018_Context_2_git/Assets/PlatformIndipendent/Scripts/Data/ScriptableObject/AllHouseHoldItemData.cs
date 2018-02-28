using System.Collections;
using System.Collections.Generic;
using UnityEngine;

[CreateAssetMenu]
public class AllHouseHoldItemData : ScriptableObject {

    public List<HouseHoldItemData> data = new List<HouseHoldItemData>();	

    public HouseHoldItemData GetDataFromID(int id){
        foreach(var d in data){
            if(d.ID == id){
                return d;
            }
        }
        return null;
    }


    /// <summary>
    /// Lets all data know what AllHouseItemData holds them.
    /// </summary>
    public void AlertAll(){
        foreach(var d in data){
            d.Alert(this);
        }
    }

}
