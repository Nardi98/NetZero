using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;
using TMPro;

public class UIManager : MonoBehaviour
{
    public enum State{
        Idle,
        Building,
        Settings
    }

    //Building UI references
    
    
    [SerializeField] private PowerPlant[] powerPlantsInfo;
    
    [Header("Building UI components")]
    [SerializeField] private Button[] powerPlantButton;
    [SerializeField] private TMP_Text fundings;
    [SerializeField] private GameObject buildingInfoUI;
    [SerializeField] private TMP_Text powerPlantNameText;
    [SerializeField] private TMP_Text minPowerText;
    [SerializeField] private TMP_Text maxPowerText;
    [SerializeField] private TMP_Text costText;

    [Space(10)]
    //PowerPlantSettings UI refereces
    [Header("Power plant settings UI components")]
    [SerializeField] private TMP_Text typeText;
    [SerializeField] private TMP_Text maxProductionText;
    [SerializeField] private TMP_Text workingCostText;
    [SerializeField] private TMP_Text pausedCostText;
    [SerializeField] private TMP_Text destructionCostText;
    [SerializeField] private Button destroyButton;
    [SerializeField] private Button pauseButton;
    [SerializeField] private GameObject powerPlantInfoUI;
    private TMP_Text pauseButtonText;


    private ResourcesManager resourcesManager;
    private float powerPlantPowerPercentage = 0.0f;

    //power plant to be built info 
    private PowerPlant powerPlantToBeBuild;
    private int powerPlantPower = 0;
    private int maxPowerPlantPower = 0;

    //state manager
    private State state = State.Idle;
    private PowerPlant powerPlantBeingSet = null;
    // Start is called before the first frame update
    void Start()
    {
        for(int i = 0; i < powerPlantButton.Length; i++)
        {
            InitializeButtons(powerPlantsInfo[i], powerPlantButton[i]);
        }

        resourcesManager = GameManager.Instance.resourcesManager;
        pauseButtonText = pauseButton.GetComponentInChildren<TMP_Text>();

        
    }

    // Update is called once per frame
    void Update()
    {
        // setting the state of the UI
        if(powerPlantToBeBuild != null)
        {
            powerPlantBeingSet = null;
            state = State.Building;

        }else if(powerPlantBeingSet != null)
        {
            powerPlantToBeBuild = null;
            state = State.Settings;
            if (!powerPlantInfoUI.activeInHierarchy)
            {
                PowerPlantSettingsUISetup(powerPlantBeingSet);
            }
        }
        else
        {
            state= State.Idle;
        }


        for(int i = 0;i < powerPlantsInfo.Length; i++)
        {
            if (powerPlantsInfo[i].powerPlantInfo.BuildingCost > resourcesManager.Fundings || state == State.Settings)
            {
                powerPlantButton[i].interactable = false;
            }
            else
            {
                powerPlantButton[i].interactable = true;
            }
        }


        fundings.text = "Fundings: " + resourcesManager.Fundings.ToString()+ "M";
        
    }

    private void InitializeButtons(PowerPlant powerPlantType, Button button)
    {
        button.onClick.AddListener(delegate { ButtonClicked(powerPlantType); });
        button.image.sprite = powerPlantType.powerPlantInfo.Icon;
     
    } 

    public void ButtonClicked(PowerPlant powerPlant)
    {
        
        Debug.Log(powerPlant);
        buildingInfoUI.gameObject.SetActive(true);
        powerPlantToBeBuild = powerPlant;
        ComputePowerPlantPower(powerPlant);

        powerPlantNameText.text = "Building: " + powerPlant.powerPlantInfo.Name;
        maxPowerText.text = maxPowerPlantPower.ToString() + " MWatt";
        
        GameManager.Instance.builder.build(powerPlant);

        // This function is called in order to set the power plant percentage coherently with the one of the slider 
        SetPowerPlantPower(powerPlantPowerPercentage);

    }

    private void ComputePowerPlantPower(PowerPlant powerPlant)
    {
        maxPowerPlantPower = resourcesManager.Fundings / powerPlant.powerPlantInfo.BuildingCost;
    }

    public void SetPowerPlantPower(float powerPercentage)
    {
        powerPlantPowerPercentage = powerPercentage;
        powerPlantPower = (int)(powerPlantPowerPercentage * maxPowerPlantPower);
        costText.text = "Cost: " + (powerPlantPower * powerPlantToBeBuild.powerPlantInfo.BuildingCost).ToString()+ "M €";
        GameManager.Instance.builder.PowerPlantPower = powerPlantPower;

    }

    public void Built()
    {
        buildingInfoUI.gameObject.SetActive(false);
        powerPlantToBeBuild = null;
    }

    public void StopConstruction()
    {

        buildingInfoUI.gameObject.SetActive(false);
        powerPlantToBeBuild = null;
        GameManager.Instance.builder.build(null);
    }

    public void ChangePowerPlantSettings(PowerPlant powerPlant)
    {
        if (state != State.Building)
        {
            powerPlantBeingSet = powerPlant;
        }
    }

    private void PowerPlantSettingsUISetup(PowerPlant powerPlant)
    {
        powerPlantInfoUI.SetActive(true);
        typeText.text = "Type: "+ powerPlant.powerPlantInfo.Name;
        maxProductionText.text = "Max production: " + powerPlant.MaxProduction.ToString() + " MWatt";
        workingCostText.text = "Operational cost: " + powerPlant.powerPlantInfo.OperationalCost.ToString() + " €";
        pausedCostText.text = "Paused cost: " + powerPlant.powerPlantInfo.PausedOperationaCost.ToString() + " €";
        destructionCostText.text = "Destruction cost: " + powerPlant.powerPlantInfo.DestructionCost.ToString() + " €";
        if (powerPlant.Paused)
        {
            pauseButtonText.text = "Restart";
        }
        else
        {
            pauseButtonText.text = "Pause";
        }
        pauseButton.interactable = powerPlant.powerPlantInfo.Pausable;
    }

    public void ClosePowerPlantSettings()
    {
        powerPlantInfoUI.SetActive(false);
        powerPlantBeingSet = null;
    }

    public void ChangePaused()
    {
        powerPlantBeingSet.Paused = !powerPlantBeingSet.Paused;
        PowerPlantSettingsUISetup(powerPlantBeingSet);
    }
}
