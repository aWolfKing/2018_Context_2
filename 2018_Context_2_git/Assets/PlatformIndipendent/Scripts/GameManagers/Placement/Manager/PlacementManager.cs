using System.Collections;
using System.Collections.Generic;
using UnityEngine;
#if UNITY_EDITOR
using UnityEditor;
#endif

public class PlacementManager : MonoBehaviour {

    public static bool Exists{
        get{
            return m_this != null;
        }
    }

    private List<PlacementFace.PlacementDot> dots = new List<PlacementFace.PlacementDot>();
    private static PlacementManager m_this = null;
    public static PlacementManager _this {
        get{
            if(m_this == null){
                m_this = GameObject.FindObjectOfType<PlacementManager>();
                if(m_this == null){
                    GameObject obj = new GameObject("PlacementManager");
                    obj.transform.position = Vector3.zero;
                    m_this = obj.AddComponent<PlacementManager>();
                }
            }
            return m_this;
        }
    }

    public static void AddDots(List<PlacementFace.PlacementDot> d){
        foreach(var _d in d){
            if(!_this.dots.Contains(_d)){
                _this.dots.Add(_d);
            }
        }
    }
    public static void RemoveDots(List<PlacementFace.PlacementDot> d){
        foreach (var _d in d) {
            if (_this.dots.Contains(_d)) {
                _this.dots.Remove(_d);
            }
        }
    }

    public static bool CanPlace(Vector3 point, Quaternion rotation, float width, float depth){
        Vector3 c0, c1, c2, c3;
        c0 = point + rotation * Vector3.right * width * 0.5f + rotation * Vector3.forward * depth * 0.5f;
        c1 = point + rotation * Vector3.right * width * 0.5f + rotation * Vector3.forward * depth * -0.5f;
        c2 = point + rotation * Vector3.right * width * -0.5f + rotation * Vector3.forward * depth * -0.5f;
        c3 = point + rotation * Vector3.right * width * -0.5f + rotation * Vector3.forward * depth * 0.5f;
        Vector3 middle = (c0 + c1 + c2 + c3) * 0.25f;
        foreach (var dot in _this.dots) {
            Vector3 d0 = (c0 - c3);
            Vector3 d1 = (c2 - c3);
            Vector3 l0 = Vector3.Project(dot.point - c3, d0.normalized);
            Vector3 l1 = Vector3.Project(dot.point - c3, d1.normalized);
            if (Vector3.Angle(d0, l0) <= 90 && Vector3.Angle(d1, l1) <= 90 && l0.magnitude <= d0.magnitude && l1.magnitude <= d1.magnitude) {
                return false;
            }
        }
        return true;
    }
    public static bool CanPlace(List<PlacementFace.PlacementDot> d){
        foreach(var _d in d){ 
            if(_d.used){
                return false;
            }
        }
        return true;
    }

    public static List<PlacementFace.PlacementDot> GetPlacementDots(Vector3 point, Quaternion rotation, float width, float depth){
        var ret = new List<PlacementFace.PlacementDot>();
        Vector3 c0, c1, c2, c3;
        c0 = point + rotation * Vector3.right * width * 0.5f + rotation * Vector3.forward * depth * 0.5f;
        c1 = point + rotation * Vector3.right * width * 0.5f + rotation * Vector3.forward * depth * -0.5f;
        c2 = point + rotation * Vector3.right * width * -0.5f + rotation * Vector3.forward * depth * -0.5f;
        c3 = point + rotation * Vector3.right * width * -0.5f + rotation * Vector3.forward * depth * 0.5f;
        Vector3 middle = (c0 + c1 + c2 + c3) * 0.25f;
        foreach (var dot in _this.dots) {
            Vector3 d0 = (c0 - c3);
            Vector3 d1 = (c2 - c3);
            Vector3 l0 = Vector3.Project(dot.point - c3, d0.normalized);
            Vector3 l1 = Vector3.Project(dot.point - c3, d1.normalized);
            if (Vector3.Angle(d0, l0) <= 90 && Vector3.Angle(d1, l1) <= 90 && l0.magnitude <= d0.magnitude && l1.magnitude <= d1.magnitude) {
                ret.Add(dot);
            }
        }
        return ret;
    }

}
