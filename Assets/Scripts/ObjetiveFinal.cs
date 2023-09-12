using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class ObjetiveFinal : MonoBehaviour
{
    [SerializeField]
    private GameObject finalPanel;

    private void Start()
    {
        finalPanel.SetActive(false);    
    }

    private void OnTriggerStay(Collider other)
    {
        if(other.gameObject.tag == "Player")
        {
            //Terminó el juego
            finalPanel.SetActive(true);

        }
    }
}
