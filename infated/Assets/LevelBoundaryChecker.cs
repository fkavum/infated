using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class LevelBoundaryChecker : MonoBehaviour
{
    public Spawner spawner;
    // Start is called before the first frame update
    void Start()
    {
        spawner = GetComponent<Spawner>();
    }

    // Update is called once per frame
    void Update()
    {
        
    }

    private void OnTriggerExit2D(Collider2D other){
       other.transform.position = spawner._Position;
    }
}
