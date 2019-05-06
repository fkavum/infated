using System.Collections;
using System.Collections.Generic;
using UnityEngine;

namespace Infated.CoreEngine
{
    public enum ChargingMagicType { NONE = 0, FIRE, ICE };
    public class CharacterMana : MonoBehaviour
    {
        
        public float RegenCooldown = 4.0f;
        public float RegenAmount = 2.5f;

        public bool Charging = false;
        private float CurrentMana = 100.0f;
        private float MaxMana = 100.0f;
        private float currentRegenCooldown = 0.0f;

        
        public ChargingMagicType CurrentChargeType = ChargingMagicType.NONE;

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


        public float spendMana(float amount)
        {
            if(CurrentMana >= amount)
            {
                CurrentMana -= amount;
                currentRegenCooldown = RegenCooldown;
                return amount;
            }
            return 0;
        }
                 
        public string getManaGuiString(){
            return Mathf.Floor(CurrentMana).ToString() + "/" + Mathf.Floor(MaxMana).ToString();
        }

        public float getManaPercentage(){
            return CurrentMana / MaxMana;
        }

        public void setCharging(bool charging, ChargingMagicType type){
            Charging = charging;
            setChargingMagicType(type);
        }

        public void setChargingMagicType(ChargingMagicType type){
            CurrentChargeType = type;
        }
        public ChargingMagicType getChargingMagicType(){
            return CurrentChargeType;
        }
    }
}