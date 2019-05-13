using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class hizirConvEvent : MonoBehaviour
{
    // Start is called before the first frame update
    private int counter = 0;
    public GameObject checkpoint1, checkpoint2, checkpoint3;
    void Start()
    {
        
    }

    // Update is called once per frame
    void Update()
    {
        
    }


    public void conversationEnded(){
        counter++;

        if(counter == 1){
            transform.position = checkpoint1.transform.position;
        }
        if(counter == 2){
            transform.position = checkpoint2.transform.position;
        }
        if(counter == 3){
            transform.position = checkpoint3.transform.position;
        }
    }
}
