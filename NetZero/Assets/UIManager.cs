using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;
using TMPro;

public class UIManager : MonoBehaviour
{
    [SerializeField] private Button[] powerPlantButton;
    [SerializeField] private PowerPlant[] powerPlantsInfo;


    // Start is called before the first frame update
    void Start()
    {
        for(int i = 0; i < powerPlantButton.Length; i++)
        {
            InitializeButtons(powerPlantsInfo[i], powerPlantButton[i]);
        }
        //button.onClick.AddListener(delegate { ButtonClicked(powerPlantInfo); });
    }

    // Update is called once per frame
    void Update()
    {
        
    }

    private void InitializeButtons(PowerPlant powerPlantType, Button button)
    {
        button.onClick.AddListener(delegate { ButtonClicked(powerPlantType); });
        button.image.sprite = powerPlantType.powerPlantInfo.Icon;
     
    } 

    public void ButtonClicked(PowerPlant powerPlant)
    {
        Debug.Log(powerPlant);
        GameManager.Instance.builder.build(powerPlant);
    }
}
