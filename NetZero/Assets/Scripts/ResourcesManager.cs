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
    [Header("Energy")]
    [SerializeField] private int[] hourlyEnergyNeed = new int[24];  //array that defines the hourly energy need

    private List<PowerPlant> alwaysOnPlants = new List<PowerPlant>(); //list of power plants that are always on (like solar) 
    private List<PowerPlant> controllablePlants = new List<PowerPlant>(); //list of power plants that the manager actually have control on 

    private void Start()
    {
        // Finds all the power plant avaliable and add them to a list
        PowerPlant[] allPowerPlants;
        allPowerPlants = FindObjectsByType<PowerPlant>(FindObjectsSortMode.None);



        //run through all the power plants found and sorts the in the two lists keeping the list in order based on the emission of CO2
        foreach (PowerPlant powerPlant in allPowerPlants)
        {
            //checks if the power plant is always on or not and positions it in the right list
            if (powerPlant.powerPlantInfo.Pausable != true)
            {
                alwaysOnPlants.Add(powerPlant);
            }
            else
            {
                //if the power plant can be controlled it insert it in the list basig the position on the ammount of CO2 produced
                if (controllablePlants.Count <= 0)
                {
                    controllablePlants.Add(powerPlant);
                }
                else {
                    
                    for(int i = 0; i<controllablePlants.Count+1; i++)
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

    }



}
