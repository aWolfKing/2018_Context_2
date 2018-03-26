using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class OnNextSeasonObjectives : OnNextSeason_monobehaviour {


    [SerializeField] private TMPro.TextMeshProUGUI  objective0 = null,
                                                    objective1 = null;


    [SerializeField] private ObjectivesScriptableObject objectivesScriptableObject = null;

    private List<ObjectivesScriptableObject.Objective> completed = new List<ObjectivesScriptableObject.Objective>();

    [SerializeField] private int    bronzeCompleted = 0,
                                    silverCompleted = 0,
                                    goldCompleted = 0;

    [SerializeField] private TMPro.TextMeshProUGUI  bronzeCompletedText = null,
                                                    silverCompletedText = null,
                                                    goldCompletedText = null;


    [SerializeField] private GameObject recentClearedObjectiveObj = null;
    [SerializeField] private TMPro.TextMeshProUGUI  mission0Descr = null,
                                                    mission1Descr = null,
                                                    mission2Descr = null,
                                                    mission3Descr = null,
                                                    mission4Descr = null;
    [SerializeField] private TMPro.TextMeshProUGUI  clearedObjectiveTxt = null,
                                                    clearedObjectiveFeedback = null;
    private List<ObjectivesScriptableObject.Objective> recentCleared = new List<ObjectivesScriptableObject.Objective>();
    private List<rank> recentClearedRanks = new List<rank>();
    private static OnNextSeasonObjectives _this = null;

    private void Start() {
        _this = this;
    }


    public override void OnBeforeChange() {
        base.OnBeforeChange();
    }

    public override void OnAfterChange() {

        for(int i=0; i<objectivesScriptableObject.objectives.Count; i++){
            var objective = objectivesScriptableObject.objectives[i];
            if(!completed.Contains(objective) && objective.seasons.Contains(MainGameManager.CurrentSeason)){

                List<ILogic> logic = new List<ILogic>(GetLogicsFor(objective, objective.objectIdsForGold));

                if(LogicAnd_True(logic.ToArray())){
                    Reward(objective, rank.gold);
                }
                else{
                    logic = new List<ILogic>(GetLogicsFor(objective, objective.objectIdsForSilver));
                    if(LogicAnd_True(logic.ToArray())){
                        Reward(objective, rank.silver);
                    }
                    else{
                        logic = new List<ILogic>(GetLogicsFor(objective, objective.objectIdsForBronze));
                        if(LogicAnd_True(logic.ToArray())){
                            Reward(objective, rank.bronze);
                        }
                    }
                }

            }
        }

        base.OnAfterChange();
    }


    private IEnumerable<ILogic> GetLogicsFor(ObjectivesScriptableObject.Objective o, List<string> objectIds) {
        for(int i=0; i<objectIds.Count; i++){
            string p = objectIds[i];

            List<ILogic> logics = new List<ILogic>();

            string[] splitByAnd = p.Split('&');
            foreach(var splt in splitByAnd){
                string[] or = splt.Split('|');
                List<int> ids = new List<int>();
                foreach(var id_s in or){
                    int result;
                    if(int.TryParse(id_s, out result)){
                        ids.Add(result);
                    }
                }
                logics.Add(new Or(ids.ToArray()));
            }
            if(logics.Count == 1){
                yield return logics[0];
            }
            else if(logics.Count > 1){
                ILogic logic = new And(logics[0], logics[1]);
                for(int l=2; l<logics.Count; l++){
                    logic = new And(logic, logics[i]);
                }
                yield return logic;
            }


            /*
            List<string> numbers = new List<string>();
            {
                List<string> n = new List<string>(p.Split('&'));
                foreach (var _n in n) {
                    numbers.AddRange(_n.Split('|'));
                }
            }
            string[] operators;
            {
                string ops = p;
                foreach (var n in numbers) {
                    ops = ops.Replace(n, " ");
                }
                operators = ops.Split(' ');
            }
            */

        }
    }


    private struct Or : ILogic{
        bool returns;
        public bool Value { get { return returns; } }
        public Or(params int[] ids){
            bool ret = false;

            List<int> ids_list = new List<int>(ids);

            for(int i=0; i<MainGameManager.AllObjects.Count; i++){
                if(ids_list.Contains(MainGameManager.AllObjects[i].HouseHoldItemData.ID)){
                    ret = true;
                    break;
                }
            }


            returns = ret;
        }
    }

    private struct And : ILogic{
        bool returns;
        public bool Value{ get{ return returns; } }
        public And(params int[] ids){
            bool ret = true;

            if(ids.Length == 0){ ret = false; }

            List<int> ids_list = new List<int>(ids);

            for (int i = 0; i < MainGameManager.AllObjects.Count; i++) {
                if(!ids_list.Contains(MainGameManager.AllObjects[i].HouseHoldItemData.ID)) {
                    ret = false;
                    break;
                }
            }

            returns = ret;
        }
        public And(ILogic l, int id){
            bool ret = false;
            for (int i = 0; i < MainGameManager.AllObjects.Count; i++) {
                if (id == MainGameManager.AllObjects[i].HouseHoldItemData.ID) {
                    ret = true;
                    break;
                }
            }

            returns = l.Value && ret;
        }
        public And(ILogic l0, ILogic l1){
            returns = l0.Value && l1.Value;
        }
    }

    private interface ILogic{
        bool Value{ get; }
    }


    private bool LogicAnd_True(params ILogic[] logic){
        if(logic.Length == 0){ return false; }
        for(int i=0; i<logic.Length; i++){
            if(logic[i].Value){
                return true;
            }
        }
        return false;
    }


    private void Reward(ObjectivesScriptableObject.Objective o, rank r){

        if(completed.Contains(o)){ return; }

        completed.Add(o);

        MonoBehaviour.print(@"Player achieved """ + o.objectiveDescription + @""" with a " + r.ToString() + " rank!");

        recentCleared.Add(o);
        recentClearedRanks.Add(r);

        {
            string t = "";
            switch(r){
                case rank.bronze:
                    t = o.feedbackOnBronze;
                    bronzeCompleted++;
                    break;
                case rank.silver:
                    t = o.feedbackOnSilver;
                    silverCompleted++;
                    break;
                case rank.gold:
                    t = o.feedbackOnGold;
                    goldCompleted++;
                    break;
            }

            if (objectivesScriptableObject.objectives.IndexOf(o) == 0) {
                objective0.text = t;
            }
            else if (objectivesScriptableObject.objectives.IndexOf(o) == 1) {
                objective1.text = t;
            }

            bronzeCompletedText.text = bronzeCompleted.ToString();
            silverCompletedText.text = silverCompleted.ToString();
            goldCompletedText.text = goldCompleted.ToString();

        }

        OptionsMenu.RequestOpen();

    }

    private enum rank{
        bronze, silver, gold
    }


    public static void UpdateUI(){
        if(_this.recentCleared.Count > 0){
            _this.recentClearedObjectiveObj.SetActive(true);

            string t = "";

            int index = _this.objectivesScriptableObject.objectives.IndexOf(_this.recentCleared[0]);

            switch(index){
                case 0:
                    t = _this.mission0Descr.text;
                    break;
                case 1:
                    t = _this.mission1Descr.text;
                    break;
                case 2:
                    t = _this.mission2Descr.text;
                    break;
                case 3:
                    t = _this.mission3Descr.text;
                    break;
                case 4:
                    t = _this.mission4Descr.text;
                    break;
            }

            _this.clearedObjectiveTxt.text = t;

            string f = "";
            switch(_this.recentClearedRanks[0]){
                case rank.bronze:
                    f = _this.recentCleared[0].feedbackOnBronze;
                    break;
                case rank.silver:
                    f = _this.recentCleared[0].feedbackOnSilver;
                    break;
                case rank.gold:
                    f = _this.recentCleared[0].feedbackOnGold;
                    break;
            }

            _this.clearedObjectiveFeedback.text = f;

        }
        else{
            _this.recentClearedObjectiveObj.SetActive(false);
        }

        if(_this.completed.Contains(_this.objectivesScriptableObject.objectives[0])){
            _this.mission0Descr.text = "completed";
        }
        if (_this.completed.Contains(_this.objectivesScriptableObject.objectives[1])) {
            _this.mission1Descr.text = "completed";
        }
        if (_this.completed.Contains(_this.objectivesScriptableObject.objectives[2])) {
            _this.mission2Descr.text = "completed";
        }
        if (_this.completed.Contains(_this.objectivesScriptableObject.objectives[3])) {
            _this.mission3Descr.text = "completed";
        }
        if (_this.completed.Contains(_this.objectivesScriptableObject.objectives[4])) {
            _this.mission4Descr.text = "completed";
        }

    }

    public void NextClearedObjective(){
        if(recentCleared.Count > 0){
            recentCleared.RemoveAt(0);
            recentClearedRanks.RemoveAt(0);
        }
        UpdateUI();
    }

}