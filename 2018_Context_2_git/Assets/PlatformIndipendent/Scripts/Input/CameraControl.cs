﻿using System.Collections;
using System.Collections.Generic;
using UnityEngine;
#if UNITY_EDITOR
using UnityEditor;
#endif

#if UNITY_EDITOR
[ExecuteInEditMode]
#endif
public class CameraControl : MonoBehaviour {

    [SerializeField] private new Camera camera          = null;
    [SerializeField] private float  minDistanceFromRoot = 0,
                                    maxDistanceFromRoot = 20;
    [SerializeField] private float  minCameraAngle      = 10,
                                    maxCameraAngle      = 60;
    [SerializeField][Range(0,1f)]   private float  zoom = 0,
                                                  angle = 0;
    [SerializeField] private float  yRot                = 0;

    [SerializeField] private float  maxXPosOffset = 3,
                                    maxZPosOffset = 3;
    private Vector3 originalPosition = Vector3.zero;

    private static CameraControl _this = null;

    public static Camera Camera{
        get{
            return _this.camera;            
        }
    }

    public static Vector3 RootPosition{
        get{
            return _this.transform.position;
        }
    }

    public static float Angle{
        get{
            return _this.angle;
        }
    }



    private void OnEnable() {
        _this = this;
        originalPosition = transform.position;
    }


    private void Update() {
        zoom = Mathf.Clamp01(zoom + Input.GetAxis("Mouse ScrollWheel") * -8 * Time.smoothDeltaTime);
        yRot = (yRot + Input.GetAxis("Mouse X") * Input.GetAxis("Fire2") * Time.smoothDeltaTime * 100) % 360;
        angle = Mathf.Clamp01(angle + -1 * Input.GetAxis("Mouse Y") * Input.GetAxis("Fire2") * Time.smoothDeltaTime);

        Vector3 pos = transform.position;
        pos += Vector3.ProjectOnPlane(camera.transform.forward, Vector3.up).normalized * Time.smoothDeltaTime * Input.GetAxis("Fire3") * Input.GetAxis("Mouse Y") * -2;
        pos += Vector3.ProjectOnPlane(camera.transform.right, Vector3.up).normalized * Time.smoothDeltaTime * Input.GetAxis("Fire3") * Input.GetAxis("Mouse X") * -2;
        pos.y = originalPosition.y;
        pos.x = Mathf.Clamp(pos.x, originalPosition.x - maxXPosOffset, originalPosition.x + maxXPosOffset);
        pos.z = Mathf.Clamp(pos.z, originalPosition.z - maxZPosOffset, originalPosition.z + maxZPosOffset);
        transform.position = pos;

        Vector3 camAxis = transform.forward * -1;
        camera.transform.position = transform.position + Quaternion.Euler(Vector3.up * yRot) * (Quaternion.Euler(Vector3.right * Mathf.Lerp(minCameraAngle, maxCameraAngle, angle)) * camAxis) * Mathf.Lerp(minDistanceFromRoot, maxDistanceFromRoot, zoom);
        camera.transform.LookAt(transform.position);
    }

    #if UNITY_EDITOR
    private void OnDrawGizmos() {
        Vector3 camAxis = transform.forward * -1;
        Gizmos.color = Color.green;
        Gizmos.DrawWireSphere(transform.position, 0.3f);
        Vector3 dir;
        dir = transform.position + Quaternion.Euler(Vector3.up * yRot) * (Quaternion.Euler(Vector3.right * minCameraAngle) * camAxis);
        Gizmos.DrawLine(dir * minDistanceFromRoot, dir * maxDistanceFromRoot);
        dir = transform.position + Quaternion.Euler(Vector3.up * yRot) * (Quaternion.Euler(Vector3.right * maxCameraAngle) * camAxis);
        Gizmos.DrawLine(dir * minDistanceFromRoot, dir * maxDistanceFromRoot);

        Gizmos.color = Color.green;
        Gizmos.DrawLine(transform.position + Vector3.right * maxXPosOffset * -1, transform.position + Vector3.right / maxXPosOffset);
        Gizmos.DrawLine(transform.position + Vector3.forward * maxZPosOffset * -1, transform.position + Vector3.forward / maxZPosOffset);

    }
    #endif

}
