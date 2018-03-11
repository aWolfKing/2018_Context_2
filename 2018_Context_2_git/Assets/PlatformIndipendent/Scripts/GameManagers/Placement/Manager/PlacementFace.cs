using System.Collections;
using System.Collections.Generic;
using UnityEngine;
#if UNITY_EDITOR
using UnityEditor;
#endif

public class PlacementFace : MonoBehaviour {


    [System.Serializable] 
    public class PlacementDot{
        public Vector3 point = Vector3.zero;
        public bool used = false;
    }

    private List<PlacementDot> dots = new List<PlacementDot>();
    private float distanceBetweenDots = 0.15f;
    [SerializeField] private bool blocksPlacement = false;

    private void OnEnable() {
        dots.Clear();
    
        Gizmos.color = Color.Lerp(Color.red, Color.yellow, 0.4f);
        Vector3 p0, p1, p2, p3;
        Vector3 scale = transform.lossyScale;
        //p0 = transform.position + new Vector3(0.5f * scale.x, 0, 0.5f * scale.z);
        //p1 = transform.position + new Vector3(0.5f * scale.x, 0, -0.5f * scale.z);
        //p2 = transform.position + new Vector3(-0.5f * scale.x, 0, -0.5f * scale.z);
        //p3 = transform.position + new Vector3(-0.5f * scale.x, 0, 0.5f * scale.z);

        p0 = transform.TransformPoint(new Vector3(0.5f, 0, 0.5f));
        p1 = transform.TransformPoint(new Vector3(0.5f, 0, -0.5f));
        p2 = transform.TransformPoint(new Vector3(-0.5f, 0, -0.5f));
        p3 = transform.TransformPoint(new Vector3(-0.5f, 0, 0.5f));

        float   w = (p3 - p0).magnitude,
                h = (p2 - p3).magnitude;
        float x = 0;
        do {
            float y = 0;
            do {
                Vector3 pos = p3 + (p0 - p3).normalized * x + (p2 - p3).normalized * y;
                var dot = new PlacementDot();
                dot.point = pos;
                dot.used = blocksPlacement; //false
                dots.Add(dot);
                y += distanceBetweenDots;
            }
            while (y <= h);
            x += distanceBetweenDots;
        }
        while (x <= w);

        PlacementManager.AddDots(dots);

    }

    private void OnDisable() {
        if (PlacementManager.Exists) {
            PlacementManager.RemoveDots(dots);
        }
    }


#if UNITY_EDITOR

    [SerializeField] private Transform testTransform = null;

    private void FixedUpdate() {
        if(testTransform != null){
            foreach(var dot in dots){
                dot.used = false;
            }
            Vector3 c0, c1, c2, c3;
            c0 = testTransform.TransformPoint(new Vector3(0.5f, 0, 0.5f));
            c1 = testTransform.TransformPoint(new Vector3(0.5f, 0, -0.5f));
            c2 = testTransform.TransformPoint(new Vector3(-0.5f, 0, -0.5f));
            c3 = testTransform.TransformPoint(new Vector3(-0.5f, 0, 0.5f));
            Vector3 middle = (c0 + c1 + c2 + c3) * 0.25f;
            foreach(var dot in dots){
                Vector3 d0 = (c0 - c3);
                Vector3 d1 = (c2 - c3);
                Vector3 l0 = Vector3.Project(dot.point - c3, d0.normalized);
                Vector3 l1 = Vector3.Project(dot.point - c3, d1.normalized);
                if(Vector3.Angle(d0, l0) <= 90 && Vector3.Angle(d1, l1) <= 90 && l0.magnitude <= d0.magnitude && l1.magnitude <= d1.magnitude){
                    dot.used = true;
                }
            }
        }
    }


    private void OnDrawGizmos() {
        float lineDist = 0.2f;

        Gizmos.color = Color.Lerp(Color.red, blocksPlacement ? Color.black : Color.yellow, 0.4f);
        Vector3 p0, p1, p2, p3;
        Vector3 scale = transform.lossyScale;
        //p0 = transform.position + new Vector3(0.5f * scale.x, 0, 0.5f * scale.z);
        //p1 = transform.position + new Vector3(0.5f * scale.x, 0, -0.5f * scale.z);
        //p2 = transform.position + new Vector3(-0.5f * scale.x, 0, -0.5f * scale.z);
        //p3 = transform.position + new Vector3(-0.5f * scale.x, 0, 0.5f * scale.z);

        p0 = transform.TransformPoint(new Vector3(0.5f, 0, 0.5f));
        p1 = transform.TransformPoint(new Vector3(0.5f, 0, -0.5f));
        p2 = transform.TransformPoint(new Vector3(-0.5f, 0, -0.5f));
        p3 = transform.TransformPoint(new Vector3(-0.5f, 0, 0.5f));

        Gizmos.DrawLine(p0, p1);
        Gizmos.DrawLine(p1, p2);
        Gizmos.DrawLine(p2, p3);
        Gizmos.DrawLine(p3, p0);

        Vector3 axis0 = p0 - p3;
        Vector3 axis1 = p2 - p3;
        if(axis1.magnitude > axis0.magnitude){
            Vector3 w = axis0;
            axis0 = axis1;
            axis1 = w;
        }

        float i = 0;
        do {
            Vector3 start = p3 + axis0.normalized * i;
            Vector3 end = start;
            float h = axis1.magnitude;
            if(i <= h){
                end = p3 + axis1.normalized * i;
            }
            else{
                end = start - (start - p3).normalized * h + axis1.normalized * h;
            }
            Gizmos.DrawLine(start, end);
            i += lineDist;
        }
        while (i <= axis0.magnitude);

        if(dots != null && dots.Count > 0){
            foreach(var dot in dots){
                Color col = dot.used ? Color.red : Color.blue; 
                col.a = 0.3f;
                Gizmos.color = col;
                Gizmos.DrawSphere(dot.point, distanceBetweenDots * 0.2f);
            }
        }

    }
    #endif

}
