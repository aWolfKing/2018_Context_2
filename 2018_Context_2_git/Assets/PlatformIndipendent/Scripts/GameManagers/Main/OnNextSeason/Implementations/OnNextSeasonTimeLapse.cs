using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class OnNextSeasonTimeLapse : OnNextSeason_monobehaviour {

    public override void OnBeforeChange() {
        MonoBehaviour.print("Timelapse");
        base.OnBeforeChange();
    }

    public override void OnAfterChange() {
        base.OnAfterChange();
    }

}
