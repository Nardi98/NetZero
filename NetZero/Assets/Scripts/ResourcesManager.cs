using System.Collections;
using System.Collections.Generic;
using UnityEngine;

/*Resources Manager
 * Manages the need and production of energy
 *  -Calculates the base energy production
 *  -Switches on energy plants depending on the necessity
 *  
 * Manages keeps track of the total emission of CO2 and keeps track of the new emissions
 * Manages the fundings and the new fundings 
 * 
 */

public class ResourcesManager : MonoBehaviour
{
    [Header("Energy (in MWatt)")]
    [Range(5000, 50000)]
    [SerializeField] private int[] baseHourlyEnergyNeed = new int[24];  //array that defines the hourly energy need

    [Space(10)]
    [Header("Settings")]
    [Range(0.0f, 1.0f)]
    public float requiredEnergyVariability = 0.4f;  //variability in the energy requirement each hour
    public float producedEnergyVariability = 0.7f;  //variability in the energy production due to weather

    private List<PowerPlant> alwaysOnPlants = new List<PowerPlant>(); //list of power plants that are always on (like solar) 
    private List<PowerPlant> controllablePlants = new List<PowerPlant>(); //list of power plants that the manager actually have control on 
    private int[] baseHourlyEnergyAvaliable = new int[24];  //array that defines the hourly energy produced by production plants that can't be switched off

    private int totalCO2Emitted = 0; //Variable that keeps the current value of total CO2 emitted
    private int oldTotalCO2Emitted = 0; //variable that keeps track of the vaue of CO2 the previous day
    private int yearCO2Emitted = 0; //variable that keeps track of the CO2 emitted during the year

    public int TotalCO2Emitted { get => totalCO2Emitted; }

    private void Start()
    {
        // Finds all the power plant avaliable and add them to a list
        PowerPlant[] allPowerPlants;
        allPowerPlants = FindObjectsByType<PowerPlant>(FindObjectsSortMode.None);



        //run through all the power plants found and sorts the in the two lists keeping the list in order based on the emission of CO2
        foreach (PowerPlant powerPlant in allPowerPlants)
        {
            AddPowerPlant(powerPlant);
        }

        
        InvokeRepeating("DailyElectricityCycle", 2.0f, 10f);

    }



    public void Update()
    {
        
    }
    
    private void DailyElectricityCycle()
    {
        /* Determines the electricity need for each hour of the day and then determines if it can be reached
         * if it can this determines a black out. Starts by using the energy that can't be controlled (power plants that are always on) 
         * and once that that is finished start using the ones that can be controlled */


        int[] hourlyEnergyRequired = DailyEnergyVariability(requiredEnergyVariability, requiredEnergyVariability, baseHourlyEnergyNeed);
        float dailyVariability = Utilities.RandomGaussian(0.0f, producedEnergyVariability);
        int[] hourlyEnergyProduced = DailyEnergyVariability(dailyVariability, 0f, baseHourlyEnergyAvaliable);

        for(int i = 0; i < hourlyEnergyRequired.Length; i++)
        {
            //energy still required. 
            if (hourlyEnergyRequired[i] > hourlyEnergyProduced[i])
            {
                int energyStillRequired = hourlyEnergyRequired[i] - hourlyEnergyProduced[i];

                //runs throwgh all the controllable power plants maxing out the production of all the needed ones
                foreach (PowerPlant powerPlant in controllablePlants) { 
                    
                    if(energyStillRequired >= powerPlant.MaxProduction && !powerPlant.Paused )
                    {
                        energyStillRequired -= powerPlant.MaxProduction;
                        hourlyEnergyProduced[i] += powerPlant.MaxProduction;
                        AddCO2(ComputeEmittedCO2(powerPlant.MaxProduction, powerPlant.powerPlantInfo));
                        //TODO calculation Cost 
                        //TODO create event for production
                    }
                    else if(energyStillRequired < powerPlant.MaxProduction && !powerPlant.Paused)
                    {
                        //TODO calculate cost 
                        //TODO create event for production
                        AddCO2(ComputeEmittedCO2(energyStillRequired, powerPlant.powerPlantInfo));
                        hourlyEnergyProduced[i] += energyStillRequired;
                        energyStillRequired = 0;
                        break;
                    }

                }
                if(energyStillRequired > 0)
                {
                    //Debug.Log("BLACKOUT");
                }
                else
                {
                    //Debug.Log("energy required" + hourlyEnergyRequired[i] + "energy produced" + hourlyEnergyProduced[i]);
                    //Debug.Log("Energy requirement met");
                }
            }
            else
            {
                
                //Debug.Log("energy required" + hourlyEnergyRequired[i] +"energy produced" + hourlyEnergyProduced[i] + "energy sufficient");
            }
        }

    }
    
    private int[] DailyEnergyVariability(float variabilityBottom, float variabilityTop, int[] baseHourlyEnergy)
    {
        int[] hourlyEnergyRequired = new int[baseHourlyEnergy.Length];
        for(int i = 0; i < baseHourlyEnergy.Length; i++)
        {
            hourlyEnergyRequired[i] = (int)(baseHourlyEnergy[i] + baseHourlyEnergy[i] * Utilities.RandomGaussian(-variabilityBottom, variabilityTop));
        }
        return hourlyEnergyRequired;
    }


    public void AddPowerPlant(PowerPlant powerPlant)
    {
        //checks if the power plant is always on or not and positions it in the right list
        if (powerPlant.powerPlantInfo.Pausable != true)
        {
            alwaysOnPlants.Add(powerPlant);
            //For each hour of the day the ammount of energy produced by the new plant is added to the one already produced by the other plants
            //The ammount of energy total avaliable each hout is computed has
            //energyAlreadyAvaliable + (energyProducedNew * efficiencyAtThatHour)
            for (int i = 0; i < baseHourlyEnergyAvaliable.Length; i++) {

                baseHourlyEnergyAvaliable[i] = baseHourlyEnergyAvaliable[i] + (int)(powerPlant.powerPlantInfo.HourlyEfficiency[i] * powerPlant.MaxProduction);
                    }
        }
        else
        {
            //if the power plant can be controlled it insert it in the list basig the position on the ammount of CO2 produced
            if (controllablePlants.Count <= 0)
            {
                controllablePlants.Add(powerPlant);
            }
            else
            {

                for (int i = 0; i < controllablePlants.Count + 1; i++)
                {
                    if (controllablePlants[i].powerPlantInfo.EmittedCO2 >= powerPlant.powerPlantInfo.EmittedCO2 && i < controllablePlants.Count)
                    {
                        controllablePlants.Insert(i, powerPlant);
                        break;
                    }
                    else
                    {
                        controllablePlants.Add(powerPlant);
                    }
                }
            }
        }
    }

    public void DestroyPowerPlant(PowerPlant powerPlant)
    {
        if (powerPlant.powerPlantInfo.Pausable != true)
        {
            alwaysOnPlants.Remove(powerPlant);
            //For each hour of the day the ammount of energy produced by the new plant is removed from the produced total 
            //The ammount of energy total avaliable each hour is computed has
            //energyAlreadyAvaliable - (energyProducedNew * efficiencyAtThatHour)
            for (int i = 0; i < baseHourlyEnergyAvaliable.Length; i++)
            {

                baseHourlyEnergyAvaliable[i] = baseHourlyEnergyAvaliable[i] - (int)(powerPlant.powerPlantInfo.HourlyEfficiency[i] * powerPlant.MaxProduction);
            }

        }
        else
        {
            controllablePlants.Remove(powerPlant);
        }
    }


    public int ComputeEmittedCO2(int power, PowerPlantInfo powerPlantType)
    {
        return power * powerPlantType.EmittedCO2;
    }

    public void AddCO2(int emittedCO2)
    {
        totalCO2Emitted += emittedCO2;
    }


    

}
