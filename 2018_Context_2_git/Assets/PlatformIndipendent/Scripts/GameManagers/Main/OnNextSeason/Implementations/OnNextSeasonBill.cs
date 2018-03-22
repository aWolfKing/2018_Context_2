using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class OnNextSeasonBill : OnNextSeason_monobehaviour {

    public override void OnAfterChange() {
        MonoBehaviour.print("Show Bill");
        base.OnAfterChange();
    }

}
