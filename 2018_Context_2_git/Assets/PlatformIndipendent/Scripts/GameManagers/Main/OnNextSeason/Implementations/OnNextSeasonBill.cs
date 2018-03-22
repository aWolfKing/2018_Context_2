using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class OnNextSeasonBill : OnNextSeason_monobehaviour {

    private static OnNextSeasonBill _this = null;

    private float energyCost = 0;
    public static float EnergyCost{
        get{
            return _this.energyCost;
        }
        set{
            _this.energyCost = value;
        }
    }

    [SerializeField] private GameObject billObj = null;
    [SerializeField] private Text energyCostTxt = null;


    private void Awake() {
        _this = this;
        billObj.SetActive(false);
    }

    public override void OnBeforeChange() {
        energyCostTxt.text = energyCost.ToString();
        billObj.SetActive(true);
        base.OnBeforeChange();
    }

    public override void OnAfterChange() {
        MonoBehaviour.print("Show Bill");
        billObj.SetActive(false);
        base.OnAfterChange();
    }

}
