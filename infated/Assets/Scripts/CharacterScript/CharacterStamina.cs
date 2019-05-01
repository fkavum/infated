using System.Collections;
using System.Collections.Generic;
using UnityEngine;

namespace Infated.CoreEngine
{
    public class CharacterStamina : MonoBehaviour
    {
        
        public float RegenCooldown = 2.0f;
        public float RegenAmount = 1.0f;

        private float CurrentStamina = 100.0f;
        private float MaxStamina = 100.0f;
        private float currentRegenCooldown = 0.0f;


        // Start is called before the first frame update
        void Start()
        {
        }

        // Update is called once per frame
        void Update()
        {
            if(currentRegenCooldown <= 0)
            {
                currentRegenCooldown = 0;
                if(CurrentStamina < MaxStamina){
                    CurrentStamina += RegenAmount;
                }
                else{
                    CurrentStamina = MaxStamina;
                }
            }
            else
            {
                currentRegenCooldown -= Time.deltaTime;
            }
        }


        public float spendStamina(float amount)
        {
            if(CurrentStamina >= amount)
            {
                CurrentStamina -= amount;
                currentRegenCooldown = RegenCooldown;
                return amount;
            }
            return 0;
        }
                 
        public string getStaminaGuiString(){
            return Mathf.Floor(CurrentStamina).ToString() + "/" + Mathf.Floor(MaxStamina).ToString();
        }


    }
}