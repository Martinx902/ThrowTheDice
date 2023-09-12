using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Objetive : MonoBehaviour
{
    [SerializeField]
    private float maxTime = 1f;

    [SerializeField]
    private float current;

    [SerializeField]
    private BoxCollider triggerCollider;

    private bool objetiveCompleted = false;

    private void OnTriggerStay(Collider other) {

        if(other.name == "Player")
        {
            //El player ha llegado al objetivo deseado
            current += Time.deltaTime;

            if(current >= maxTime)
            {
                print("Hola");

                if(!objetiveCompleted)
                {
                    current = maxTime;

                    //Ha estado x tiempo en el objetivo por lo que puede activar el evento de objetivo completado
                    print("Objetivo Completado");

                    DiceManager.instance.ResetTries();
                    DiceManager.instance.UpdateCheckPoint();

                    //Desactivamos el collider de objetivo
                    triggerCollider.gameObject.SetActive(false);

                    objetiveCompleted = true;
                }

            }
        }

    }

    private void OnTriggerExit(Collider other) {

        if(other.name == "Player")
        {
            current = 0;
            objetiveCompleted = false;
        }

    }
}
