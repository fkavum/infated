using UnityEngine;
using System.Collections;
using Infated.Tools;
 using System.Collections.Generic;

namespace Infated.CoreEngine
{	

    public class SplashCheck : MonoBehaviour
    {
        public bool splashEnabled = false;
        public List<Collider2D> splashList = new List<Collider2D>();

        public int SplashDamage = 5;
        // Start is called before the first frame update
        void Start()
        {
            
        }

        // Update is called once per frame
        void Update()
        {
        }

        public void DealDamage(){
            foreach(Collider2D col in splashList){
                if((col.tag == "Enemy")) {
                    Health hp = col.transform.GetComponent<Health>();
                    if(hp != null){
                        hp.Damage(SplashDamage, this.gameObject, 5, 0.5f);
                    }
            }
            }
            
        }
        


        //called when something enters the trigger
        void OnTriggerEnter2D(Collider2D Collider)
        {
            //if the object is not already in the list
            if(!splashList.Contains(Collider))
            {
                //add the object to the list
                splashList.Add(Collider);
            }
        }
        
        //called when something exits the trigger
        void OnTriggerExit2D(Collider2D Collider)
        {
            //if the object is in the list
            if(splashList.Contains(Collider))
            {
                //remove it from the list
                splashList.Remove(Collider);
            }
        }
    }
}
