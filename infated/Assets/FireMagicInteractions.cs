using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class FireMagicInteractions : MonoBehaviour
{
    // Start is called before the first frame update
    void Start()
    {
        Vector3 position = gameObject.transform.position;
        RaycastHit2D hit = Physics2D.Raycast(position, Vector2.right);
        if(hit.collider != null){
                if(hit.collider.tag == "BurnableIvy"){
                    hit.transform.gameObject.GetComponent<Animator>().Play("IvyBurnDown");
                }
            }

    }

    // Update is called once per frame
    void Update()
    {
        
    }
}
