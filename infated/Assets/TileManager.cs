using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using Infated.Tools;

namespace Infated.CoreEngine
{	
    public class TileManager : MonoBehaviour
    {
        
        private float[][] EventTrait;
        // Start is called before the first frame update
        void Start()
        {
            //Read json data and get the results
            //Calculate event-trait matrix from the data
        }

        public void AdjustTileSettings(float warrior, float explorer, float storyreader, float speedrunner){
            float sum =  warrior + explorer + storyreader + speedrunner;
            float warriorPerc = warrior / sum;
            float explorerPerc = explorer / sum;
            float storyreaderPerc = storyreader / sum;
            float speedrunnerPerc = speedrunner / sum;
            
            

            // Logic goes here
            foreach(Transform tile in this.transform){
                TileScript tileScript = tile.GetComponent<TileScript>();
                
                int NPCgroups = 0;
                if(warriorPerc > 0.15){
                    NPCgroups++;
                }
                else if(warriorPerc > 0.25){
                    NPCgroups++;
                }
                else if(warriorPerc > 0.35){
                    NPCgroups++;
                }
                else if(warriorPerc > 0.45){
                    NPCgroups++;
                }

                int wallCount = 0;
                if(speedrunner > 0.3){
                    wallCount++;
                }
                else if(speedrunner > 0.5){
                    wallCount++;
                }

                if(storyreader > 0.4){
                    tileScript.enableInteractionNPC();
                }
                tileScript.enableWalls(wallCount);
                tileScript.setNPCGroupCount(NPCgroups);
                //Some logic:
                //tileScript.setNPCStrength();
                //etc...
            }

        }

        // Update is called once per frame
        void Update()
        {
            
        }
    }
}