using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.Tilemaps;

public class Builder : MonoBehaviour
{
    // Start is called before the first frame update
    [SerializeField] private Tilemap map;
    private PowerPlant powerPlantToBeBuilt = null;
    public int PowerPlantPower;


    void Start()
    {
        
    }

    // Update is called once per frame
    void Update()
    {

        //Todo fix check if clicked on objects to avoid to build a power plant on another one 
        
        if (powerPlantToBeBuilt != null)
        {
            Collider2D col = Physics2D.OverlapPoint(Camera.main.ScreenToWorldPoint(Input.mousePosition));
            Vector2 mousePosition = Camera.main.ScreenToWorldPoint(Input.mousePosition);
            Vector3Int gridPosition = map.WorldToCell(mousePosition);
            Vector3 buildingPosition = map.CellToWorld(gridPosition);

            bool buildable = CheckBuildable(gridPosition);
            
            if (Input.GetMouseButtonDown(0) && col == null && PowerPlantPower != 0 && buildable  )
            {

                
                
                /* Checks the the type of map that it will build on
                if (map.GetTile(gridPosition).name.Contains("costal"))
                {
                    Debug.Log("At position" + gridPosition + "there is a costal map");
                }
                */

                PowerPlant builtPowerPlant = GameObject.Instantiate(powerPlantToBeBuilt, buildingPosition, Quaternion.identity);
                builtPowerPlant.MaxProduction = PowerPlantPower;
                GameManager.Instance.resourcesManager.AddFundings(-PowerPlantPower * powerPlantToBeBuilt.powerPlantInfo.BuildingCost);
                Debug.Log("build");                
                powerPlantToBeBuilt = null;
                GameManager.Instance.UiManager.Built();
            }

        }
        
    }

    //function that checks if the tile type allows to build on 
    private bool CheckBuildable(Vector3Int gridPosition)
    {
        if(map.GetTile(gridPosition).name.Contains("costal") || map.GetTile(gridPosition).name.Contains("Forest"))
        {
            return true;
        }
        else
        {
            return false;
        }
    }

    public void build(PowerPlant powerPlant)
    {
        powerPlantToBeBuilt = powerPlant;
        PowerPlantPower = 0;
    }

}
