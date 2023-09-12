using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;
using TMPro;

public class DiceManager : MonoBehaviour
{
    //Manager Singleton
    public static DiceManager instance { get; private set; }

    [Header("UI")]
    [Space(15)]

    [SerializeField]
    private TextMeshProUGUI triesText;

    [SerializeField]
    private List<Transform> checkPoint = new List<Transform>();

    private float currentTries;
    private float totalTries;
    private int cpIndex;

    private void Awake()
    {
        //Instancing the singleton
        if (instance != null && instance != this)
        {
            Destroy(this);
        }
        else
        {
            instance = this;
        }

        cpIndex = 0;
    }

    public void SetTrys(float diceTrys)
    {
        currentTries = diceTrys;
        totalTries = diceTrys;

        triesText.text = currentTries.ToString(); 
    }

    public bool UpdateTrys()
    {
        currentTries--;
        triesText.text = currentTries.ToString();

        if(currentTries < 0)
        {
            print("Respawneando...");
            currentTries = totalTries;
            triesText.text = currentTries.ToString();
            return true;
        }

        return false;
    }

    public void ResetTries()
    {
        currentTries = 0;
        triesText.text = currentTries.ToString();
        DiceResultCheck.instance.FirstShoot();
    }

    public Transform ActualCheckPoint()
    {
        return checkPoint[cpIndex];
    }

    public void UpdateCheckPoint()
    {
        cpIndex++;
    }
    
}
