using System.Collections;
using System.Collections.Generic;
using UnityEngine;

/*
 * class that inerith from Building, it allows to create production plants. 
 */
[CreateAssetMenu]
public class PowerPlantInfo : BuildingInfo
{
    [Header("Energy production")]
    [SerializeField] private int maxProduction;

    [Range(0,1)]
    [SerializeField] private float[] hourlyEfficiency = new float[24]; //the production of power plants that are always on can vary based on the time of the day

    public float[] HourlyEfficiency { get => hourlyEfficiency; }
    public int MaxProduction { get => maxProduction;  }

    //[Header("Emissions")]


}
