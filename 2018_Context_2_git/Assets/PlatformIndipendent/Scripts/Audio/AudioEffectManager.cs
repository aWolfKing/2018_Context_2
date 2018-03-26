using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class AudioEffectManager : MonoBehaviour {

    private static AudioEffectManager _this = null;
    public static AudioEffectManager Instance{ get{ return _this; } }



    public AudioClip
            objectPlaced = null;



    private void Awake() {
        _this = this;
    }

    public static void Play(AudioClip clip){
        if(clip != null){
            AudioSource.PlayClipAtPoint(clip, CameraControl.Camera.transform.position);
        }
    }	

}
