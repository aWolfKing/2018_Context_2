using System.Collections;
using System.Collections.Generic;
using UnityEngine;
#if UNITY_EDITOR
using UnityEditor;
#endif

public class PlacementObject : MonoBehaviour {

    public float    width = 1,
                    depth = 1;
    private List<PlacementFace.PlacementDot> usedDots = new List<PlacementFace.PlacementDot>();

    [SerializeField] private bool autoPlace = false;

    public void SetUsedDots(List<PlacementFace.PlacementDot> d){
        foreach(var dot in usedDots){
            dot.used = false;
        }
        usedDots.Clear();
        usedDots.AddRange(d);
        foreach (var dot in usedDots) {
            dot.used = true;
        }
    }
    public List<PlacementFace.PlacementDot> GetUsedDots(){
        return usedDots;
    }

    public List<PlacementFace.PlacementDot> GetPlacementDots(){
        return PlacementManager.GetPlacementDots(transform.position, transform.rotation, transform.lossyScale.x * width, transform.lossyScale.z * depth);
    }

    public bool CanPlaceHere(){
        return PlacementManager.CanPlace(GetPlacementDots());
    }


    private void OnEnable() {
        if(autoPlace){
            //SetUsedDots(GetPlacementDots());
            StartCoroutine(DelayedSetDots());
        }
        #if UNITY_EDITOR
        StartCoroutine(Update20());
        #endif
    }

    private void OnDisable() {
        if(autoPlace){
            SetUsedDots(new List<PlacementFace.PlacementDot>());
        }
    }


    private IEnumerator DelayedSetDots(){
        yield return new WaitForEndOfFrame();
        SetUsedDots(GetPlacementDots());
    }


#if UNITY_EDITOR
    private bool canPlaceHere_editorOnly = true;


    private IEnumerator Update20(){
        WaitForSeconds wait = new WaitForSeconds(1f / 20);
        do {
            canPlaceHere_editorOnly = CanPlaceHere();
            yield return wait;
        }
        while (true);
    }


    private void OnDrawGizmos() {
        Vector3 p0, p1, p2, p3;
        p0 = transform.position + (transform.right * transform.lossyScale.x * width + transform.forward * transform.lossyScale.z * depth) * 0.5f;
        p1 = transform.position + (transform.right * transform.lossyScale.x * width - transform.forward * transform.lossyScale.z * depth) * 0.5f;
        p2 = transform.position + (-transform.right * transform.lossyScale.x * width - transform.forward * transform.lossyScale.z * depth) * 0.5f;
        p3 = transform.position + (-transform.right * transform.lossyScale.x * width + transform.forward * transform.lossyScale.z * depth) * 0.5f;
        Gizmos.color = Color.Lerp(Color.blue, canPlaceHere_editorOnly ? Color.cyan : Color.black, 0.6f);
        Gizmos.DrawLine(p0, p1);
        Gizmos.DrawLine(p1, p2);
        Gizmos.DrawLine(p2, p3);
        Gizmos.DrawLine(p3, p0);
    }
    #endif

}
