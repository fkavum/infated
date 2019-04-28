using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using System.IO;

public class userProfiler : MonoBehaviour
{
    public int doubleJumpCount = 0;
    public int oneJumpCount = 0;
    public int tripleJumpCount = 0;
    public float iceChargeAmount = 0;
    public int iceMagicUsed = 0;
    public int shapeShiftCount = 0;

    private List<string> checkPoints = new List<string>();
    public float totalCombatTime = 0f;


    private void Start()
    {
    }
    // Update is called once per frame
    void Update()
    {
        if (Input.GetKeyDown(KeyCode.K))
        {
            Debug.Log("I ll write file");
            writeDataToFile();
        }
    }

    public void profileJump(int jumpCount)
    {
        switch (jumpCount)
        {
            case 1:
                oneJumpCount++;
                break;
            case 2:
                doubleJumpCount++;
                break;
            case 3:
                tripleJumpCount++;
                break;
            default:
                Debug.Log("Unexpected Jump to profile");
                break;
        }
    }
    public void profileIceMagic (float chargeAmount)
    {
        iceMagicUsed++;
        iceChargeAmount += chargeAmount;
    }

    public void writeDataToFile()
    {
        var jsonData = new
        {
            OneJumpCount = oneJumpCount,
            DoubleJumpCount = doubleJumpCount,
            IceMagicUsed = iceMagicUsed,
            ShapeShiftCount = shapeShiftCount,
            Magic = new
            {
                TripleJumpCount = tripleJumpCount,
                IceChargeAmount = iceChargeAmount,
            }
        };

        string jsonString = JsonUtility.ToJson(this);

        File.WriteAllText(Application.dataPath + "/Resources/Items.json", jsonString);
        Debug.Log("Writed");
    }


    public void checkPointReached(string checkPointName)
    {
        checkPoints.Add(checkPointName);

    }

}
