using UnityEngine;
using System.Collections;
using System.Collections.Generic;

public class GameController : MonoBehaviour 
{
    [SerializeField] List<GameObject> _enemyList;
    public List<GameObject> GetEnemyList { get { return _enemyList; } }
    static GameController _instance;

	// Use this for initialization
	void Awake() 
    {
        _instance = this;
	}

    public static GameController Instance()
    {
        return _instance;
    }    
	
	// Update is called once per frame
	void Update () 
    {
	
	}
}
