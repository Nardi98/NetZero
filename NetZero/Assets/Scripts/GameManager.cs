using System.Collections;
using System.Collections.Generic;
using UnityEngine;
    /*Game manager, Singleton that has to contain in the same game object as children also 
 * Resources Manager
 * Building Placer
 * Audio Manager
 * 
 * It manages the change of seens and allows to reference the other managers 
 */
public class GameManager : MonoBehaviour
{
    public static GameManager Instance { get; private set; }
    
    //public AudioManager AudioManager { get; private set; }
    public ResourcesManager resourcesManager { get; private set; }
    public Builder builder { get; private set; }
    
    private UIManager uiManager;
    public UIManager UiManager { get => uiManager; set => uiManager = value; }

    
    private void Awake()
    {
        if (Instance != null && Instance != this)
        {
            Destroy(this);
            return;
        }
        Instance = this;
        //AudioManager = GetComponentInChildren<AudioManager>();
        resourcesManager = GetComponentInChildren<ResourcesManager>();
        builder = GetComponentInChildren<Builder>();
        uiManager = GetComponentInChildren<UIManager>();
    }



    // Start is called before the first frame update
    void Start()
    {
        
    }

    // Update is called once per frame
    void Update()
    {
        
    }
}
