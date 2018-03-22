using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class AreYouSure : MonoBehaviour {

    private System.Action onYes = null;
    private System.Action onNo = null;

    [SerializeField] private GameObject confirmObj = null;
    private static AreYouSure _this = null;

    private void Awake() {
        _this = this;
    }

    private void FixedUpdate(){
        if(onYes == null && onNo == null && confirmObj.activeInHierarchy){
            confirmObj.SetActive(false);
        }
        else if((onYes != null || onNo != null) && !confirmObj.activeInHierarchy){
            confirmObj.SetActive(true);
        }
    }

    public void No(){
        if(onNo != null){
            onNo();
        }
        onNo = null;
        onYes = null;
    }
    public void Yes(){ 
        if(onYes != null){
            onYes();
        }
        onNo = null;
        onYes = null;
    }

    public static void RequestConfirm(System.Action onYes, System.Action onNo){
        _this.onYes = onYes;
        _this.onNo = onNo;
    }

}
