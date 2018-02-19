using System.Collections;
using System.Collections.Generic;
using UnityEngine;

#if UNITY_ANDROID

#endif
public static partial class InputManager {

    private static Touch        touch0t     = new Touch()   ,
                                touch1t     = new Touch()   ;
    private static Vector2?     touch0      = Vector2.zero  , 
                                touch1      = Vector2.zero  ;
    private static Vector2?     lastTouch0  = Vector2.zero  ,
                                lastTouch1  = Vector2.zero  ;
    private static int          touch0Id    = 0             ,
                                touch1Id    = 1             ;

    private const float         ZOOM_ROTATE_ANGLE = 10      ;   //from what angle will 'zoom' be considered rotating.

    private static float        zoomInput   = 0             ,
                                rotateInput = 0             ;
    private static Vector2      moveInput   = Vector2.zero  ;

    public static float         zoomSpeed   = 2             ;
    public static float         touchMoveDist 
                                            = 100           ;



    [RuntimeInitializeOnLoadMethod]
    private static void WakeUpMobileInput(){
        Updater.OnUpdate += UpdateMobileInput;
    }

    private static void UpdateMobileInput(){
        lastTouch0 = touch0;
        lastTouch1 = touch1;

        var touches = Input.touches;
        int touchCount = 0;
        for(int i=0; i<touches.Length; i++){
            touchCount += (touches[i].phase != TouchPhase.Ended && touches[i].phase != TouchPhase.Canceled) ? 1 : 0;
            if(touches[i].fingerId == touch0Id){
                touch0t = touches[i];
            }
            else if(touches[i].fingerId == touch1Id){
                touch1t = touches[i];
            }
        }
        for(int i=0; i<touches.Length; i++){
            if(touches[i].phase == TouchPhase.Ended || touches[i].phase == TouchPhase.Canceled){
                if(touches[i].fingerId == touch0Id){
                    touch0 = null;
                }
                else if(touches[i].fingerId == touch1Id){
                    touch1 = null;
                }
            }
            else{
                if(touches[i].fingerId == touch0Id){
                    touch0 = touches[i].position;
                }
                else if(touches[i].fingerId == touch1Id){
                    touch1 = touches[i].position;
                }
            }
        }

        zoomInput = 0;
        rotateInput = 0;
        moveInput = Vector2.zero;
        if (UpdateMoveInput()) {    //If not moving, check zooming and rotating
            UpdateZoomAndRotateInput();
        }
    }

    /// <summary>
    /// Returns true if no move input was detected.
    /// </summary>
    /// <returns></returns>
    private static bool UpdateMoveInput(){
        if(Vector2.Distance(touch0.Value, touch1.Value) <= touchMoveDist){
            if(Vector2.Angle(Vector2.right, (touch1.Value-touch0.Value).normalized) <= 45){
                moveInput = ((touch0.Value + touch1.Value) * 0.5f) - ((lastTouch0.Value + lastTouch1.Value) * 0.5f);
            }
            return false;
        }
        return true;
    }

    private static void UpdateZoomAndRotateInput(){
        if (touch0t.phase == TouchPhase.Moved && touch1t.phase == TouchPhase.Moved) {
            if (lastTouch0.HasValue && lastTouch1.HasValue) {
                Vector2 oldVec = (lastTouch1.Value - lastTouch0.Value);
                Vector2 newVec = (touch1.Value - touch0.Value);
                if(Vector2.Angle(oldVec.normalized, newVec.normalized) <= ZOOM_ROTATE_ANGLE){
                    zoomInput = (oldVec.magnitude - newVec.magnitude) * -1;
                }
                else{
                    rotateInput = Vector2.SignedAngle(oldVec.normalized, newVec.normalized);
                }
            }
        }
    }


    public static float ZoomInput{
        get{
            return zoomInput;
        }
    }

    public static float RotateInput{
        get{
            return rotateInput;
        }
    }

    public static Vector2 MoveInput{
        get{
            return moveInput;
        }
    }

}
