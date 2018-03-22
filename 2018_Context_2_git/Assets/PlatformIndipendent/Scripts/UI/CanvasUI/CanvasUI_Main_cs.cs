using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class CanvasUI_Main_cs : MonoBehaviour {

    [System.Serializable]
    public class ShopOptions{
        public Transform shopUI;
        /*From open to closed*/
        public AnimationCurve xPositionAnimation;
        public float animationDuration = 0.6f;
        public Dropdown categoryDropDown;
        public void FixedUpdate(){ }
    }

    [System.Serializable]
    public class InterfaceOptions{
        public Text seasonText;
        public Text cashText;
        public Text seasonCountText;
        public void FixedUpdate(){
            seasonText.text = MainGameManager.CurrentSeason.ToString();
            cashText.text = "Cash : " + MainGameManager.Cash;
            seasonCountText.text = "Seasons survived : " + MainGameManager.SeasonsSurvived;
        }
    }


    [System.Serializable]
    public class InteractionAndUpgrade{
        public Transform interactionTransform = null;
        public Text interactionTitle = null;
        public Text descriptionText = null;
        public Text usageText = null;
        public Button interaction_upgradeButton = null;
        public Button interaction_removeButton = null;
        public Button interaction_moveButton = null;
        public Transform upgradeTransform = null;
        public Text usageWasText = null;
        public Text usageBecomesText = null;
        public Text upgradeCostText = null;
        public HouseHoldItem_monobehaviour interacting = null;
        public Button upgradeButton = null;

        public AnimationCurve openAndClosCurve = null;

        public void Awake(){
            interactionTransform.localPosition = new Vector3(openAndClosCurve.Evaluate(1f), 0, 0);
            upgradeTransform.localPosition = new Vector3(openAndClosCurve.Evaluate(1f), 0, 0);
        }
        public void FixedUpdate(){
            if(interacting != null){
                interactionTitle.text = interacting.HouseHoldItemData.name;
                descriptionText.text = interacting.HouseHoldItemData.description;
                usageText.text = "Usage: " + interacting.HouseHoldItemData.electricityUsage.ToString();

                usageWasText.text = interacting.HouseHoldItemData.electricityUsage.ToString();

                interaction_moveButton.interactable = interacting.CanMove;
                interaction_removeButton.interactable = interacting.CanDelete;

                var u = interacting.HouseHoldItemData.Upgrade;
                if (u != null){
                    usageBecomesText.text = u.electricityUsage.ToString();
                    upgradeCostText.text = "Upgrade cost: " + interacting.HouseHoldItemData.upgradeCost.ToString();
                    upgradeButton.interactable = MainGameManager.Cash >= interacting.HouseHoldItemData.upgradeCost;
                    interaction_upgradeButton.interactable = true;
                }
                else{
                    usageBecomesText.text = "-";
                    upgradeCostText.text = "-";
                    upgradeButton.interactable = false;
                    interaction_upgradeButton.interactable = false;
                }
            }
            else{
                interactionTitle.text = "-";
                descriptionText.text = "-";
                usageText.text = "-";
                usageWasText.text = "-";
                usageBecomesText.text = "-";
                upgradeCostText.text = "-";
                upgradeButton.interactable = false;
                interaction_upgradeButton.interactable = false;
                interaction_moveButton.interactable = false;
                interaction_removeButton.interactable = false;
            }
        }
    }



    [SerializeField] private ShopOptions shopOptions = new ShopOptions();
    [SerializeField] private InterfaceOptions interfaceOptions = new InterfaceOptions();
    [SerializeField] private InteractionAndUpgrade interactionAndUpgradeOptions = new InteractionAndUpgrade();
    private static CanvasUI_Main_cs _this = null;

    [SerializeField] private UnityEngine.EventSystems.EventSystem eventSystem = null;
    public static UnityEngine.EventSystems.EventSystem EventSystem{
        get{
            return _this.eventSystem;
        }
    }


    private void OnEnable() {

        _this = this;

        /*Set category dropdown*/
        {
            List<Dropdown.OptionData> categories = new List<Dropdown.OptionData>();
            for (int i = 0; i < MainGameManager.Data.categories.Count; i++){
                categories.Add(new Dropdown.OptionData(MainGameManager.Data.categories[i]));
            }
            shopOptions.categoryDropDown.options = categories;

            shopOptions.categoryDropDown.onValueChanged.AddListener(OnCategoryChanged_UICallback);
        }

        interactionAndUpgradeOptions.Awake();

    }

    private void FixedUpdate() {
        shopOptions.FixedUpdate();
        interfaceOptions.FixedUpdate();
        interactionAndUpgradeOptions.FixedUpdate();
    }


    public void OpenShop(){
        ShopManager.CancelBuy();
        ShopManager.OnOpenShop();

        CloseInteraction();
        CloseUpgrade();

        StartCoroutine(OpenOrCloseShop(-1));
    }
    public void CloseShop(){
        ShopManager.OnOpenShop();
        StartCoroutine(OpenOrCloseShop(1));
    }

    public static void RequestOpenShop() {
        _this.OpenShop();
    }

    public static void RequestCloseShop(){
        _this.CloseShop();
    }



    public void OpenInteraction() {
        CloseUpgrade();
        StartCoroutine(OpenOrCloseInteractionOrUpgrade(interactionAndUpgradeOptions.interactionTransform, -1));
    }
    public void CloseInteraction() {
        StartCoroutine(OpenOrCloseInteractionOrUpgrade(interactionAndUpgradeOptions.interactionTransform, 1));
    }

    public void OpenUpgrade() {
        CloseInteraction();
        StartCoroutine(OpenOrCloseInteractionOrUpgrade(interactionAndUpgradeOptions.upgradeTransform, -1));
    }
    public void CloseUpgrade() {
        StartCoroutine(OpenOrCloseInteractionOrUpgrade(interactionAndUpgradeOptions.upgradeTransform, 1));
    }

    public static void RequestOpenInteraction(){
        _this.OpenInteraction();
    }
    public static void RequestCloseInteraction(){
        _this.CloseInteraction();
    }

    public static void RequestOpenUpgrade(){
        _this.OpenUpgrade();
    }
    public static void RequestCloseUpgrade(){
        _this.CloseUpgrade();
    }


    public void DeleteInteractingObject(){
        if(interactionAndUpgradeOptions.interacting != null){
            //GameObject.Destroy(interactionAndUpgradeOptions.interacting.gameObject);
            CloseInteraction();

            var a = interactionAndUpgradeOptions.interacting.gameObject;
            AreYouSure.RequestConfirm(() => { GameObject.Destroy(a); }, OpenInteraction);
        }
    }

    public void MoveInteracting(){
        if(interactionAndUpgradeOptions.interacting != null && interactionAndUpgradeOptions.interacting.CanMove){
            ShopManager.Move(interactionAndUpgradeOptions.interacting, interactionAndUpgradeOptions.interacting.GetComponent<PlacementObject>());
        }
    }




    public void OnCategoryChanged(){
        ShopManager.ChangeCategory(shopOptions.categoryDropDown.options[shopOptions.categoryDropDown.value].text);
    }

    private void OnCategoryChanged_UICallback(int category){
        ShopManager.ChangeCategory(shopOptions.categoryDropDown.options[category].text);
    }

    public static string GetDefaultCategory(){
        return _this.shopOptions.categoryDropDown.options[_this.shopOptions.categoryDropDown.value].text;
    }


    /// <summary>
    /// 
    /// </summary>
    /// <param name="direction">Positive : close, Negative : open</param>
    /// <returns></returns>
    private IEnumerator OpenOrCloseShop(int direction){
        float s = (1f/shopOptions.animationDuration);
        float t = direction * 0.5f + 0.5f;
        float i = 1 - t;
        var wait = new WaitForFixedUpdate();
        do {
            Vector3 p = shopOptions.shopUI.localPosition;
            p.x = shopOptions.xPositionAnimation.Evaluate(Mathf.Clamp01(i));
            shopOptions.shopUI.localPosition = p;
            yield return wait;
            i = Mathf.Clamp01(i + (direction * Time.fixedDeltaTime * s));
        }
        while (i != t);
    }


    /// <summary>
    /// 
    /// </summary>
    /// <param name="t"></param>
    /// <param name="direction">Positive : close, Negative : open</param>
    /// <returns></returns>
    private IEnumerator OpenOrCloseInteractionOrUpgrade(Transform transfrm, int direction){
        float s = (1f / shopOptions.animationDuration);
        float t = direction * 0.5f + 0.5f;
        float i = 1 - t;
        var wait = new WaitForFixedUpdate();
        if (transfrm.localPosition.x != interactionAndUpgradeOptions.openAndClosCurve.Evaluate(Mathf.Clamp01(t))) {


            do {
                Vector3 p = transfrm.localPosition;
                p.x = interactionAndUpgradeOptions.openAndClosCurve.Evaluate(Mathf.Clamp01(i));
                transfrm.localPosition = p;
                yield return wait;
                i = Mathf.Clamp01(i + (direction * Time.fixedDeltaTime * s));
            }
            while (i != t);

        }
    }


    public static void SetInteracting(HouseHoldItem_monobehaviour m){
        if (m != _this.interactionAndUpgradeOptions.interacting) {
            _this.interactionAndUpgradeOptions.interacting = m;
            if(m != null){
                _this.OpenInteraction();
            }
            else{
                _this.CloseInteraction();
                _this.CloseUpgrade();
            }
        }
    }

    public static HouseHoldItem_monobehaviour GetInteracting(){
        return _this.interactionAndUpgradeOptions.interacting;
    }


}
