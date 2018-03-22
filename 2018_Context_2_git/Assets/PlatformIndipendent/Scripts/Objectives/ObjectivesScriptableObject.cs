using System.Collections;
using System.Collections.Generic;
using UnityEngine;

[CreateAssetMenu]
public class ObjectivesScriptableObject : ScriptableObject {

    [System.Serializable]
    public class Objective{
        public string objectiveDescription = "do this thing...";
        public List<Season> seasons = new List<Season>();
        public List<string> objectIdsForBronze = new List<string>();
        public List<string> objectIdsForSilver = new List<string>();
        public List<string> objectIdsForGold = new List<string>();
        public string feedbackOnBronze = "bronze feedback";
        public string feedbackOnSilver = "silver feedback";
        public string feedbackOnGold = "gold feedback";
    }

    public List<Objective> objectives = new List<Objective>();

}
