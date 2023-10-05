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
        
        if (powerPlantToBeBuilt != null)
        {
            Collider2D col = Physics2D.OverlapPoint(Camera.main.ScreenToWorldPoint(Input.mousePosition));
            if (Input.GetMouseButtonDown(0) && col == null )
            {
                Vector2 mousePosition = Camera.main.ScreenToWorldPoint(Input.mousePosition);
                Vector3Int gridPosition = map.WorldToCell(mousePosition);
                Vector3 buildingPosition = map.CellToWorld(gridPosition);

                


                /* Checks the the type of map that it will build on
                if (map.GetTile(gridPosition).name.Contains("costal"))
                {
                    Debug.Log("At position" + gridPosition + "there is a costal map");
                }
                */

                GameObject.Instantiate(powerPlantToBeBuilt, buildingPosition, Quaternion.identity);
                Debug.Log("build");                powerPlantToBeBuilt = null;
            }
        }
        
    }

    public void build(PowerPlant powerPlant)
    {
        powerPlantToBeBuilt = powerPlant;
    }
}
