using System.Collections;
using System.Collections.Generic;
using UnityEngine;


/* Script that determines the behaviour of building 
 * it refrences a scriptable object containing the info related to the building
 */
public class Building : MonoBehaviour
{
    //[Header("Building cathegory info")]
    //public Building BuildingInfo;

    [Header("Energy production")]
    [SerializeField] private int _maxProduction;

    public int MaxProduction { get => _maxProduction; set => _maxProduction = value; }

    // Start is called before the first frame update
    void Start()
    {
        
    }

    // Update is called once per frame
    void Update()
    {
        
    }
}
