using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class checkPoint : MonoBehaviour
{
    
    public string CheckpointName;
    public bool Debugging = false;
    private void OnTriggerEnter2D(Collider2D other)
    {
        if (other.gameObject.tag == "Player")
        {
            Character _char = other.gameObject.GetComponent<Character>();
            _char._userProfiler.checkPointReached(CheckpointName);
            gameObject.SetActive(false);
            if(Debugging) Debug.Log(CheckpointName + " Reached!");
        }
    }


}
