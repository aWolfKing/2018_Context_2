using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public static partial class InputManager {

    public class mUpdater : MonoBehaviour{
        private System.Action onUpdate = null;
        public System.Action OnUpdate{
            get{
                return onUpdate;
            }
            set{
                onUpdate = value;
            }
        }
        private void Update() {
            if(onUpdate != null){ onUpdate(); }
        }
    }

    private static InputManager.mUpdater m_updater = null;
    public static InputManager.mUpdater Updater{
        get{
            if(m_updater == null){
                m_updater = GameObject.FindObjectOfType<InputManager.mUpdater>();
                if(m_updater == null){
                    GameObject obj = new GameObject("Input updater");
                    m_updater = obj.AddComponent<mUpdater>();
                }
            }
            return m_updater;
        }
    }

}
