using System.Collections;
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

    private void Update() {
        zoom = Mathf.Clamp01(zoom + Input.GetAxis("Mouse ScrollWheel") * Time.smoothDeltaTime);
        yRot = (yRot + Input.GetAxis("Mouse X") * Input.GetAxis("Fire2") * Time.smoothDeltaTime * 100) % 360;
        angle = Mathf.Clamp01(angle + Input.GetAxis("Mouse Y") * Input.GetAxis("Fire2") * Time.smoothDeltaTime);

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
    }
    #endif

}
