using System.Collections;
using System.Collections.Generic;
using UnityEngine;

/*
 * Scriptable object that defines the characteristics of a building common to all the types. 
 * Parent scripts for the other cathegories
 */


public class BuildingInfo : ScriptableObject
{
    [Header("Apperance")]
    [SerializeField] private Sprite _icon;
    [SerializeField] private string _name;

    [Header("Costs")]
    [SerializeField] private int _buildingCost;
    [SerializeField] private int _operationalCost;
    [SerializeField] private int _destructionCost;
    [SerializeField] private int _pausedOperationaCost;

    [Space(20)]
    [SerializeField] private int _emittedCO2;
    [SerializeField] private int _buildingTime;
    [SerializeField] private bool _pausable;



    public Sprite Icon { get => _icon; }
    public int BuildingCost { get => _buildingCost;  }
    public int OperationalCost { get => _operationalCost; }
    public int DestructionCost { get => _destructionCost;  }
    public int EmittedCO2 { get => _emittedCO2;  }
    public int BuildingTime { get => _buildingTime;  }
    public bool Pausable { get => _pausable;  }
    public string Name { get => _name; set => _name = value; }
    public int PausedOperationaCost { get => _pausedOperationaCost; }
}
