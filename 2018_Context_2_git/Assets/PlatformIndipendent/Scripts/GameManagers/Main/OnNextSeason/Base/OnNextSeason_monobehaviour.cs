using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class OnNextSeason_monobehaviour : MonoBehaviour {

    private void OnEnable() {
        //MainGameManager.AddOnSeasonChange(this);
        StartCoroutine(DelayedAdd());
    }

    private void OnDisable() {
        MainGameManager.RemoveOnSeasonChange(this);
    }

    public virtual void OnBeforeChange(){ }

    public virtual void OnAfterChange(){ }

    private IEnumerator DelayedAdd(){
        yield return new WaitForEndOfFrame();
        MainGameManager.AddOnSeasonChange(this);
    }

}
