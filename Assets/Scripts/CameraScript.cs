using UnityEngine;
using System.Collections;

public class CameraScript : MonoBehaviour 
{
    [SerializeField] GameObject _playerRef;

	void Start () 
    {
	    
	}
    
	void Update () 
    {
        Vector2 playerPosition = _playerRef.GetComponent<Transform>().position;
        Vector3 cameraPosition = new Vector3(playerPosition.x, this.transform.position.y, -10);
        if(playerPosition.y >= 1)
        {
            Debug.Log("playerPosition.y: " + playerPosition.y);
            cameraPosition.y = playerPosition.y - .65f;
        }

        this.transform.position = cameraPosition;
	}
}
