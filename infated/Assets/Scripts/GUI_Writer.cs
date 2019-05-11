using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class GUI_Writer : MonoBehaviour
{
    public GameObject mGui;
    public UnityEngine.UI.Text mMana, mCharge, mChargeType, mStamina;
    public UnityEngine.UI.Image mHpBar, mManabar, mStaminabar;

    // Start is called before the first frame update
    void Start()
    {
        
    }

    // Update is called once per frame
    void Update()
    {
        
    }

    public void assignGUI(GameObject gui){
        this.mGui = gui;
        assignElements();
    }

    void assignElements(){
        GameObject[] guiChildren =  getAllChildren(mGui);
        GameObject[] guiPanel;

        foreach(GameObject child in guiChildren){
            if (child.name == "MagicPanel") {
                guiPanel = getAllChildren(child);
                foreach (GameObject magicChild in guiPanel) {
                    switch (magicChild.name) {
                        case "Mana":
                            mMana = magicChild.GetComponent<UnityEngine.UI.Text>();
                            mMana.text = "10/100";
                            break;
                        case "Charge":
                            mCharge = magicChild.GetComponent<UnityEngine.UI.Text>();
                            mCharge.text = "999";
                            break;
                        case "Type":
                            mChargeType = magicChild.GetComponent<UnityEngine.UI.Text>();
                            mChargeType.text = "lol";
                            break;
                        case "ManaBar":
                            mManabar = magicChild.GetComponent<UnityEngine.UI.Image>();
                            mManabar.fillAmount = 1;
                            break;
                        default:
                            break;
                    }
                }
            }
            else if (child.name == "HpPanel")
            {
                guiPanel = getAllChildren(child);
                foreach (GameObject hpChild in guiPanel) {
                    switch (hpChild.name) {
                        case "HpBar":
                            mHpBar = hpChild.GetComponent<UnityEngine.UI.Image>();
                            mHpBar.fillAmount = 1;
                            break;
                        default:
                            break;
                    }
                }
            }
            else if(child.name == "StaminaPanel"){
                guiPanel = getAllChildren(child);
                foreach(GameObject staminaChild in guiPanel){
                    switch(staminaChild.name){
                        case "Stamina":
                            mStamina = staminaChild.GetComponent<UnityEngine.UI.Text>();
                            mStamina.text = "10/100";
                            break;
                        case "StaminaBar":
                            mStaminabar = staminaChild.GetComponent<UnityEngine.UI.Image>();
                            mStaminabar.fillAmount = 1;
                            break;
                        default:
                            break;
                    }
                }
            }
        }
    }

    private GameObject[] getAllChildren(GameObject parent){
        GameObject[] children = new GameObject[parent.transform.childCount];
        for(int i = 0; i < parent.transform.childCount; i++){
            children[i] = parent.transform.GetChild(i).gameObject;
        }
        return children;
    }

    public void setChargeText(string amount){
        mCharge.text = amount;
    }
    public void setManaText(string amount, float percentage){
        mMana.text = amount;
        mManabar.fillAmount = percentage;
    }

    public void setHp(float percentage)
    {
        mHpBar.fillAmount = percentage;
    }

    public void setChargeTypeText(string type){
        mChargeType.text = type;
    }

    public void setStaminaText(string amount, float percentage){
        mStamina.text = amount;
        mStaminabar.fillAmount = percentage;
    }
}
