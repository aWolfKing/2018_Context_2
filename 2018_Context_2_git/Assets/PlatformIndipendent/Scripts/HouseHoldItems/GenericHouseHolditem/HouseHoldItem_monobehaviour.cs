using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class HouseHoldItem_monobehaviour : MonoBehaviour {

    public enum VisualMaterial{
        canPlace, cannotPlace, normalMaterial
    }
    public VisualMaterial visualMaterial = VisualMaterial.normalMaterial;
    private VisualMaterial visualMaterial_was = VisualMaterial.normalMaterial;

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

    private static Material canPlaceMaterial = null;
    private static Material cannotPlaceMaterial = null;
    private Dictionary<MeshRenderer, Material[]> originalMaterials = new Dictionary<MeshRenderer, Material[]>();

    [SerializeField] private Material m_canPlaceMaterial = null;
    [SerializeField] private Material m_cannotPlaceMaterial = null;



    private void OnEnable() {

        if(canPlaceMaterial == null && m_canPlaceMaterial != null){ canPlaceMaterial = m_canPlaceMaterial; }
        if(cannotPlaceMaterial == null && m_cannotPlaceMaterial != null){ cannotPlaceMaterial = m_cannotPlaceMaterial; }

        if (houseHoldItemDataID == -9999) {
            gameObject.SetActive(false);
        }
        else {
            RegisterOriginalMaterials();
            MainGameManager.AddObject(this);
        }
    }

    private void OnDisable() {
        MainGameManager.RemoveObject(this);
    }


    private void FixedUpdate() {
        if(visualMaterial != visualMaterial_was){
            switch(visualMaterial){
                case VisualMaterial.normalMaterial:
                    SetMaterial(null);
                    break;
                case VisualMaterial.canPlace:
                    SetMaterial(canPlaceMaterial);
                    break;
                case VisualMaterial.cannotPlace:
                    SetMaterial(cannotPlaceMaterial);
                    break;
            }
        }
        visualMaterial_was = visualMaterial;
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


    private void RegisterOriginalMaterials(){
        originalMaterials.Clear();
        RegisterOriginalMaterialFor(transform);
    }

    private void RegisterOriginalMaterialFor(Transform t){
        var renderers = new List<MeshRenderer>(t.GetComponentsInChildren<MeshRenderer>());

        for(int i=0; i<renderers.Count; i++){
            originalMaterials.Add(renderers[i], renderers[i].materials);
        }
    }

    /// <summary>
    /// 
    /// </summary>
    /// <param name="m">Original when m == null</param>
    private void SetMaterial(Material m){

        foreach(var pair in originalMaterials){
            int l = pair.Key.materials.Length;
            var mats = new Material[l];
            for (int i = 0; i < l; i++) {
                mats[i] = m ?? pair.Value[i];
            }
            pair.Key.sharedMaterials = mats;
        }

    }

}
