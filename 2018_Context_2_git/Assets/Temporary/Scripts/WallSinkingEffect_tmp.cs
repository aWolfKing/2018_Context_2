using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class WallSinkingEffect_tmp : MonoBehaviour {

    [SerializeField] private Vector3    lowestPos,
                                        highestPos;

    private void FixedUpdate() {
        transform.localPosition = Vector3.Lerp(lowestPos, highestPos, CameraControl.Angle);
    }

}
