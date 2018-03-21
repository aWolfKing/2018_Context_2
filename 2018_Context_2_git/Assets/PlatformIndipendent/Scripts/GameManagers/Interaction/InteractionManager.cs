using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class InteractionManager : MonoBehaviour {

    private float lastMouse0 = 0;

    private void Update() {
        float mouse0 = Input.GetAxis("Fire1");
        if(lastMouse0 < 0.5f && mouse0 >= 0.5f && !CanvasUI_Main_cs.EventSystem.IsPointerOverGameObject()){
            CheckForInteractableObject();
        }
        lastMouse0 = mouse0;
    }


    private void CheckForInteractableObject(){
        RaycastHit hit;
        if(Physics.Raycast(CameraControl.Camera.ScreenPointToRay(Input.mousePosition), out hit, 1000f)){
            var m = hit.transform.GetComponent<HouseHoldItem_monobehaviour>();
            if (m != null) {
                CanvasUI_Main_cs.SetInteracting(m);
            }
            else{
                CanvasUI_Main_cs.SetInteracting(null);
            }
        }
        else {
            CanvasUI_Main_cs.SetInteracting(null);
        }
    }

}
