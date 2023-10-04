using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.Tilemaps;

public class Builder : MonoBehaviour
{
    // Start is called before the first frame update
    [SerializeField] private Tilemap map;
    private PowerPlant powerPlantToBeBuilt = null; 
    Ray ray;


    void Start()
    {
        
    }

    // Update is called once per frame
    void Update()
    {

        //Todo fix check if clicked on objects to avoid to build a power plant on another one 
        Debug.Log(Physics.Raycast(ray));
        if (powerPlantToBeBuilt != null)
        {
            ray = Camera.main.ScreenPointToRay(Input.mousePosition);
            

            if (Input.GetMouseButtonDown(0) && !Physics.Raycast(ray))
            {
                Vector2 mousePosition = Camera.main.ScreenToWorldPoint(Input.mousePosition);
                Vector3Int gridPosition = map.WorldToCell(mousePosition);
                Vector3 buildingPosition = map.CellToWorld(gridPosition);



                GameObject.Instantiate(powerPlantToBeBuilt, buildingPosition, Quaternion.identity);
                Debug.Log("build");
                powerPlantToBeBuilt = null;
            }
        }
        
    }

    public void build(PowerPlant powerPlant)
    {
        powerPlantToBeBuilt = powerPlant;
    }
}
