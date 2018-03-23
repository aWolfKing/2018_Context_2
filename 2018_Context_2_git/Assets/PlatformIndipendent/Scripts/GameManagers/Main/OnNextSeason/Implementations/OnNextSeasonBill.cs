using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;
using TMPro;

public class OnNextSeasonBill : OnNextSeason_monobehaviour {

    private static OnNextSeasonBill _this = null;

    private float energyCost = 0;
    public static float EnergyCost{
        get{
            return _this.energyCost;
        }
        set{
            _this.energyCost = value;
            _this.energyCostTxt.text = "-" + value.ToString();
        }
    }

    [SerializeField] private GameObject billObj = null;
    //[SerializeField] private Text energyCostTxt = null;
    [SerializeField] private TextMeshProUGUI energyCostTxt = null;

    [SerializeField] private TextMeshProUGUI cashAmount = null;

    [SerializeField] private GameObject positiveCashObj = null,
                                        negativeCashObj = null,
                                        gameOverMomsTip = null,
                                        gameOverThanksMom = null;  


    private void Awake() {
        _this = this;
        billObj.SetActive(false);
        MainGameManager.onCashChanged += OnCashChanged;
    }

    public override void OnBeforeChange() {
        energyCostTxt.text = "-" + energyCost.ToString();
        cashAmount.text = MainGameManager.Cash.ToString();
        if(MainGameManager.Cash < 0){
            negativeCashObj.SetActive(true);
            positiveCashObj.SetActive(false);
        }
        else{
            negativeCashObj.SetActive(false);
            positiveCashObj.SetActive(true);
        }
        billObj.SetActive(true);
        base.OnBeforeChange();
    }

    public override void OnAfterChange() {
        billObj.SetActive(false);
        base.OnAfterChange();
    }

    private void OnCashChanged(){
        cashAmount.text = MainGameManager.Cash.ToString();
        if (MainGameManager.Cash < 0) {
            negativeCashObj.SetActive(true);
            positiveCashObj.SetActive(false);
            gameOverMomsTip.SetActive(true);
            gameOverThanksMom.SetActive(true);
        }
        else {
            negativeCashObj.SetActive(false);
            positiveCashObj.SetActive(true);
            gameOverMomsTip.SetActive(false);
            gameOverThanksMom.SetActive(false);
        }
    }

}
