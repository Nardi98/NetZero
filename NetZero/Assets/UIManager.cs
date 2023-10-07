using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;
using TMPro;

public class UIManager : MonoBehaviour
{
    [SerializeField] private Button[] powerPlantButton;
    [SerializeField] private PowerPlant[] powerPlantsInfo;
    [SerializeField] private TMP_Text fundings;
    [SerializeField] private GameObject buildingInfoUI;
    [SerializeField] private TMP_Text powerPlantNameText;
    [SerializeField] private TMP_Text minPowerText;
    [SerializeField] private TMP_Text maxPowerText;
    [SerializeField] private TMP_Text costText;

    private ResourcesManager resourcesManager;
    private float powerPlantPowerPercentage = 0.0f;

    //power plant to be built info 
    private PowerPlant powerPlantToBeBuild;
    private int powerPlantPower = 0;
    private int maxPowerPlantPower = 0;

    // Start is called before the first frame update
    void Start()
    {
        for(int i = 0; i < powerPlantButton.Length; i++)
        {
            InitializeButtons(powerPlantsInfo[i], powerPlantButton[i]);
        }

        resourcesManager = GameManager.Instance.resourcesManager;
        //button.onClick.AddListener(delegate { ButtonClicked(powerPlantInfo); });
    }

    // Update is called once per frame
    void Update()
    {

        for(int i = 0;i < powerPlantsInfo.Length; i++)
        {
            if (powerPlantsInfo[i].powerPlantInfo.BuildingCost > resourcesManager.Fundings)
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
        powerPlantToBeBuild = powerPlant;
        ComputePowerPlantPower(powerPlant);

        powerPlantNameText.text = "Building: " + powerPlant.powerPlantInfo.Name;
        maxPowerText.text = maxPowerPlantPower.ToString() + " MWatt";
        
        

        GameManager.Instance.builder.build(powerPlant);

    }

    private void ComputePowerPlantPower(PowerPlant powerPlant)
    {
        maxPowerPlantPower = resourcesManager.Fundings / powerPlant.powerPlantInfo.BuildingCost;
    }

    public void SetPowerPlantPower(float powerPercentage)
    {
        powerPlantPowerPercentage = powerPercentage;
        powerPlantPower = (int)powerPlantPowerPercentage * maxPowerPlantPower;
        costText.text = (powerPlantPower * powerPlantToBeBuild.powerPlantInfo.BuildingCost).ToString();
        GameManager.Instance.builder.PowerPlantPower = powerPlantPower;
        Debug.Log(powerPlantPowerPercentage);
    }
}
