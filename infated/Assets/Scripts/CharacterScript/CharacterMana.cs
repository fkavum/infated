using System.Collections;
using System.Collections.Generic;
using UnityEngine;

namespace Infated.CoreEngine
{
    public class CharacterMana : MonoBehaviour
    {
        
        public float RegenCooldown = 4.0f;
        public float RegenAmount = 2.5f;

        private bool Charging = false;
        private float CurrentMana = 100.0f;
        private float MaxMana = 100.0f;
        private float currentRegenCooldown = 0.0f;
        // Start is called before the first frame update
        void Start()
        {
            Charging = false;
        }

        // Update is called once per frame
        void Update()
        {
            if(currentRegenCooldown <= 0)
            {
                currentRegenCooldown = 0;
                if(CurrentMana < MaxMana){
                    CurrentMana += RegenAmount;
                }
                else{
                    CurrentMana = MaxMana;
                }
            }
            else
            {
                if(!Charging)
                    currentRegenCooldown -= Time.deltaTime;
            }
        }


        public bool spendMana(float amount)
        {
            if(CurrentMana >= amount)
            {
                CurrentMana -= amount;
                currentRegenCooldown = RegenCooldown;
                return true;
            }
            return false;
        }
                 
        public string getManaGuiString(){
            return Mathf.Floor(CurrentMana).ToString() + "/" + Mathf.Floor(MaxMana).ToString();
        }

        public void setCharging(bool charging){
            Charging = charging;
        }
    }
}