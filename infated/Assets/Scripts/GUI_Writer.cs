using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class GUI_Writer : MonoBehaviour
{
    public GameObject mGui;
    public UnityEngine.UI.Text mMana, mCharge, mChargeType;

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
        GameObject[] guiMagicPanel;

        foreach(GameObject child in guiChildren){
            if(child.name == "MagicPanel"){
                guiMagicPanel = getAllChildren(child);
                foreach(GameObject magicChild in guiMagicPanel){
                    switch(magicChild.name){
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
    public void setManaText(string amount){
        mMana.text = amount;
    }
    public void setChargeTypeText(string type){
        mChargeType.text = type;
    }
}
