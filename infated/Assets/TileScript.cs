using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using Infated.Tools;

namespace Infated.CoreEngine
{	
    public class TileScript : MonoBehaviour
    {
        [Header("Object References from Tile")]
        public GameObject NPCs;
        public GameObject Walls;
        public GameObject Interactable_NPC;

        [Header("Object Count in the Tile")]
        public int NPC_Groups = 3;
        public int NPC_Count_in_a_Group = 2;
        public int Wall_Count = 2;
        public int Interactable_NPC_Count = 1;
        public int enabledWalls = 2;
        bool eInteractionNPC = true;
        private int totalNPCs = 0;
        private GameObject[] NPCList;

        // Start is called before the first frame update
        void Start()
        {
            enableWalls(enabledWalls);
            if(eInteractionNPC) enableInteractionNPC();

        }

        public void AdjustTileSettings(float warrior, float explorer, float storyreader, float speedrunner){
            float sum =  warrior + explorer + storyreader + speedrunner;
            float warriorPerc = warrior / sum;
            float explorerPerc = explorer / sum;
            float storyreaderPerc = storyreader / sum;
            float speedrunnerPerc = speedrunner / sum;

        }

        public void setNPCGroupCount(int amount){
            if(amount > NPC_Groups) amount = NPC_Groups;

            totalNPCs = amount * NPC_Count_in_a_Group;
            NPCList = new GameObject[totalNPCs];
            int i = 0;
            foreach(Transform child in NPCs.transform){            
                if(amount == 0){
                    Object.Destroy(child);
                    continue;
                }
                    
                
                child.gameObject.SetActive(true);
                foreach(Transform c in child.transform){  
                    NPCList[i++] = c.gameObject;
                }

                amount--;
            }
        }

        public void setNPCStrength(int health, int armor, int damage){
            foreach(GameObject npc in NPCList){
                Health h = npc.GetComponent<Health>();
                h.Armor = armor;
                h.CurrentHealth = health;
                h.MaximumHealth = health;
                npc.GetComponent<CharacterAttack>().Damage = damage;
            }
        }

        public void enableWalls(int amount){
            if(amount > Wall_Count) amount = Wall_Count;

            foreach(Transform wall in Walls.transform){
                if(amount == 0){
                    Object.Destroy(wall);
                    continue;
                }

                wall.gameObject.SetActive(true);
                RaycastHit2D hit = Physics2D.Raycast(wall.transform.position, Vector2.down);
                if(hit.collider != null){
                    Vector2 target = hit.point;
                    Vector3 pos = wall.transform.position;
                    pos.y = target.y;
                    wall.transform.SetPositionAndRotation(pos, Quaternion.identity);
                }
                amount--;
            }
        }

        public void enableInteractionNPC(){
            Interactable_NPC.SetActive(true);
        }

        // Update is called once per frame
        void Update()
        {
            
        }
    }
}