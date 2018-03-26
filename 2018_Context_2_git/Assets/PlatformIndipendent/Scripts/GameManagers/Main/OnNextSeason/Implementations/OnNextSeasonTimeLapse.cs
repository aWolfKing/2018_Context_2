using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class OnNextSeasonTimeLapse : OnNextSeason_monobehaviour {

    [SerializeField] private GameObject winterGameObject    = null,
                                        springGameObject    = null,
                                        summerGameObject    = null,
                                        fallGameObject      = null;

    public override void OnBeforeChange() {
        MonoBehaviour.print("Timelapse");
        base.OnBeforeChange();
    }

    public override void OnAfterChange() {
        base.OnAfterChange();
        winterGameObject.SetActive(false);
        springGameObject.SetActive(false);
        summerGameObject.SetActive(false);
        fallGameObject.SetActive(false);
        switch(MainGameManager.CurrentSeason){
            case Season.Winter:
                winterGameObject.SetActive(true);
                break;
            case Season.Spring:
                springGameObject.SetActive(true);
                break;
            case Season.Summer:
                springGameObject.SetActive(true);
                break;
            case Season.Fall:
                fallGameObject.SetActive(true);
                break;
        }
    }

}
