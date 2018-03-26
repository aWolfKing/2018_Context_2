using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public enum Season{
    Summer, Fall, Winter, Spring
}

public class MainGameManager : MonoBehaviour {

    private static List<HouseHoldItem_monobehaviour> allObjects = new List<HouseHoldItem_monobehaviour>();
    public static List<HouseHoldItem_monobehaviour> AllObjects{
        get{
            return allObjects;
        }
    }

    private static MainGameManager _this = null;
    [SerializeField] private AllHouseHoldItemData allHouseHoldItemData = null;
    public static AllHouseHoldItemData Data{
        get{
            _this = GameObject.FindObjectOfType<MainGameManager>();
            if (_this == null) {
                GameObject obj = new GameObject("GameManager");
                obj.transform.position = Vector3.zero;
                _this = obj.AddComponent<MainGameManager>();
            }
            if(_this.allHouseHoldItemData == null){
                Debug.LogError(@"Please create a ""MainGameManager"" and assign ""allHouseHoldItemData"".");
            }
            return _this.allHouseHoldItemData;
        }
    }
    [SerializeField]private Season currentSeason = Season.Summer;
    public static Season CurrentSeason{
        get{
            return _this.currentSeason;
        }
    }

    [SerializeField] private float incomePerSeason = 1000;
    public static float IncomePerSeason{
        get{ return _this.incomePerSeason; }
    }
    [SerializeField] private float rentCostPerSeason = 300;
    public static float RentCostPerSeason{
        get{ return _this.rentCostPerSeason; }
    }

    [SerializeField] private float m_cash = 0;
    public static float Cash{
        get{
            return _this.m_cash;
        }
        set{
            _this.m_cash = value;
            onCashChanged();
        }
    }

    [SerializeField] private int seasonsSurvived = 0;
    public static int SeasonsSurvived{
        get{
            return _this.seasonsSurvived;
        }
    }

    private List<OnNextSeason_monobehaviour> onSeasonChanges = new List<OnNextSeason_monobehaviour>();

    private bool thanksMom = false;

    public static System.Action onCashChanged = null;


    [SerializeField] private UnityEngine.Events.UnityEvent onGameOver = null;



    [RuntimeInitializeOnLoadMethod]
    private static void Init(){
        _this = GameObject.FindObjectOfType<MainGameManager>();
        if (_this == null) {
            GameObject obj = new GameObject("GameManager");
            obj.transform.position = Vector3.zero;
            _this = obj.AddComponent<MainGameManager>();
        }

    }
    
    
    public static void AddObject(HouseHoldItem_monobehaviour obj){
        if(!allObjects.Contains(obj)){
            allObjects.Add(obj);
        }
    }
    public static void RemoveObject(HouseHoldItem_monobehaviour obj){
        if(allObjects.Contains(obj)){
            allObjects.Remove(obj);
        }
    }


    public static void AddOnSeasonChange(OnNextSeason_monobehaviour m) {
        if (!_this.onSeasonChanges.Contains(m)) {
            _this.onSeasonChanges.Add(m);
        }
    }
    public static void RemoveOnSeasonChange(OnNextSeason_monobehaviour m) {
        if (_this.onSeasonChanges.Contains(m)) {
            _this.onSeasonChanges.Remove(m);
        }
    }


    public static float CalculateEnergyCosts(){
        float ret = 0;
        foreach(var o in allObjects){
            o.HouseHoldItemData.Alert(Data);
            o.ResetInfluence();
        }

        if (allObjects.Count > 0) {

            HouseHoldItem_monobehaviour currObject = allObjects[0];
            for (int i = 0; i < allObjects.Count; i++, currObject = (allObjects.Count > i ? allObjects[i] : null)) {

                if(currObject == null){ continue; }

                var affectedObjects = currObject.HouseHoldItemData.affectedObjects;

                List<int> affectedIDs = new List<int>();
                foreach (var aO in affectedObjects) { affectedIDs.Add(aO.affectedObjectId); }

                HouseHoldItem_monobehaviour otherObject = allObjects[0];
                for (int o = 0; o < allObjects.Count; o++, otherObject = (allObjects.Count > o ? allObjects[o] : null)) {
                    if (currObject != otherObject && otherObject != null) {
                        if (affectedIDs.Contains(otherObject.HouseHoldItemData.ID)) {

                            var effect = affectedObjects[affectedIDs.IndexOf(otherObject.HouseHoldItemData.ID)];
                            if (Vector3.Distance(currObject.transform.position, otherObject.transform.position) <= effect.effectRadius) {

                                switch (CurrentSeason) {
                                    case Season.Summer:
                                        otherObject.AddInfluence(effect.summerElectricityPercentage);
                                        break;
                                    case Season.Fall:
                                        otherObject.AddInfluence(effect.fallElectricityPercentage);
                                        break;
                                    case Season.Winter:
                                        otherObject.AddInfluence(effect.winterElectricityPercentage);
                                        break;
                                    case Season.Spring:
                                        otherObject.AddInfluence(effect.springElectricityPercentage);
                                        break;
                                }

                            }

                        }
                    }
                }
            }

            foreach (var o in allObjects) {
                ret += o.CalculateTotalEnergyCost();
            }

        }

        MonoBehaviour.print("This seasons energy cost: " + ret);

        return ret;
    }


    public void ContinueToNextSeason(){
        MonoBehaviour.print("To next season");

        StartCoroutine(NextSeasonCoroutine());

        /*
        foreach(var m in onSeasonChanges){
            m.OnBeforeChange();
        }

        float costs = CalculateEnergyCosts();

        costs += _this.rentCostPerSeason;

        //costs times season multiplier?

        Cash -= costs;

        if (m_cash < 0) { GameOver(); }
        else {

            seasonsSurvived++;

            switch (currentSeason) {
                case Season.Fall:
                    currentSeason = Season.Winter;
                    break;
                case Season.Winter:
                    currentSeason = Season.Spring;
                    break;
                case Season.Spring:
                    currentSeason = Season.Summer;
                    break;
                case Season.Summer:
                    currentSeason = Season.Fall;
                    break;
            }

            Cash += incomePerSeason;

            foreach (var m in onSeasonChanges) {
                m.OnAfterChange();
            }

        }

        */

    }

    public static void ToNextSeason(){
        _this.ContinueToNextSeason();
    }

    private IEnumerator NextSeasonCoroutine() {
        foreach (var m in onSeasonChanges) {
            m.OnBeforeChange();
        }

        float costs = CalculateEnergyCosts();

        OnNextSeasonBill.EnergyCost = costs;

        costs += _this.rentCostPerSeason;

        Cash -= costs;

        Cash += incomePerSeason;

        yield return WaitForThanksMom();

        //costs times season multiplier?

        //Cash -= costs;

        if (m_cash < 0) { GameOver(); }
        else {

            seasonsSurvived++;

            switch (currentSeason) {
                case Season.Fall:
                    currentSeason = Season.Winter;
                    break;
                case Season.Winter:
                    currentSeason = Season.Spring;
                    break;
                case Season.Spring:
                    currentSeason = Season.Summer;
                    break;
                case Season.Summer:
                    currentSeason = Season.Fall;
                    break;
            }

            //Cash += incomePerSeason;

            MonoBehaviour.print("onafter " + onSeasonChanges.Count);
            for(int i=0; i<onSeasonChanges.Count; i++){
                MonoBehaviour.print(onSeasonChanges[i].name);
            }

            foreach (var m in onSeasonChanges) {
                m.OnAfterChange();
            }
        }

    }


    public static void GameOver(){
        //MonoBehaviour.print("Game over... (implement this)");
        _this.onGameOver.Invoke();
    }


    private IEnumerator WaitForThanksMom(){

        var wait = new WaitForEndOfFrame();
        do {
            yield return wait;
        }
        while (!thanksMom);
        thanksMom = false;

    }

    public void ThanksMom(){
        thanksMom = true;
    }


}
