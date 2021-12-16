using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Head : MonoBehaviour
{
    public GameController gameController;

    private void OnTriggerEnter(Collider col)
    {
        switch(col.gameObject.tag)
        {
            case "food":
                gameController.Eat();
                break;

            case "tail":
                gameController.GameOver();

                break;
            


        }
    }
}
