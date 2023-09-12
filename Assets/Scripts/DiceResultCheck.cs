using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class DiceResultCheck : MonoBehaviour
{
    public static DiceResultCheck instance { get; private set; }

    [SerializeField]
    private Rigidbody rb;

    private bool firstShoot = true;

    private float totalTrys;

    private void Awake()
    {
        instance = this;
    }

    void OnTriggerStay(Collider other) 
    {
        //Comprobamos si el cubo está parado o no
        if(other.gameObject.tag == "Side" && rb.velocity.x <= 0.001f && rb.velocity.y <= 0.001f && rb.velocity.z <= 0.001f)
        {
            //Si está disparando por primera vez, y todavía no ha tirado, entonces cuando tire se le actualizan las tiradas
            if(firstShoot)
            {
                print("Estamos disparando por primera vez");

                print(other.gameObject.name);

                //Comprueba números no poderes
                switch (other.gameObject.name)
                {
                    case "Side3":
                        totalTrys = 3;
                        DiceManager.instance.SetTrys(totalTrys);
                        break;

                    case "Side4":
                        totalTrys = 4;
                        DiceManager.instance.SetTrys(totalTrys);
                        break;

                    case "Side5":
                        totalTrys = 5;
                        DiceManager.instance.SetTrys(totalTrys);
                        break;

                    case "Side6":
                        totalTrys = 6;
                        DiceManager.instance.SetTrys(totalTrys);
                        break;
                }

                firstShoot = false;

            }
        }
    }

    public void FirstShoot()
    {
        firstShoot = true;
    }

}
