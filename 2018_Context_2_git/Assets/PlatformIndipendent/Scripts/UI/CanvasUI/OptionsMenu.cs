using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class OptionsMenu : MonoBehaviour {

    private bool doOpen = false;
    private bool doBlock = false;

    private float lastInp = 0;
    
    private static OptionsMenu _this = null;

    [SerializeField] private GameObject optionsMenuObj = null;


    private void Awake() {
        _this = this;
    }

    private void Update() {
        float inp = Input.GetAxis("Cancel");
        if(lastInp < 0.5f && inp >= 0.5f){
            doOpen = true;
        }
        lastInp = inp;
    }

    private void FixedUpdate() {
        if(doOpen && !doBlock){
            Open();
        }
        doOpen = false;
        doBlock = false;
    }


    public static void PreventOpening(){
        _this.doBlock = true;
    }


    public void Open(){
        optionsMenuObj.SetActive(true);
    }

    public void Close(){
        optionsMenuObj.SetActive(false);
    }

}
