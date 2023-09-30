using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class PowerPlant : Building
{   

    [Header("Power Plant info")]
    public PowerPlantInfo powerPlantInfo;
    [Header("Energy production")]
    [SerializeField] private int _maxProduction;

    

    [Header("Operational settings")]
    [SerializeField] private bool built = false;

    // variable to manage the production of resources
    private float timeSpentBuilding = 0.0f;  //keeps track of the production process
    private bool producing = false;  //building is producing used for animation
    private bool paused;  //building is paused. The resource manager can't set it to produce
    
    public int MaxProduction { get => _maxProduction; }
    public bool Paused
    {
        get => paused;
        set
        {
            if (powerPlantInfo.Pausable == true)
            {
                paused = value;
            }
            else
            {
                paused = false;
            }
        }
    }

    public bool Producing { get => producing; set => producing = value; }


    // Start is called before the first frame update
    void awake()
    {

    }

    // Update is called once per frame
    void Update()
    {
        //checks if it is still building if it is it increase the passed time
        if(built == false)
        {
            timeSpentBuilding += Time.deltaTime;
            if(timeSpentBuilding >= powerPlantInfo.BuildingTime)
            {
                //once that the building is completed it adds it to the list of buildings in the resource manager
                FinishedBuilding();
            }
        }

    }


    private void FinishedBuilding()
    {
        GameManager.Instance.resourcesManager.AddPowerPlant(this);
        built = true;
    }

    //on destroy it removes the building from the list of buildings in the resource manager
    public void OnDestroy()
    {
        GameManager.Instance.resourcesManager.DestroyPowerPlant(this);
    }

}
