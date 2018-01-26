using System.Collections;
using System.Collections.Generic;
using UnityEngine;

namespace CargoStrategy.UserInput
{
    public class UserInputDispatcher : MonoBehaviour
    {

        
        private void Update()
        {

            Debug.Log(Input.GetAxis("P1Horizontal"));

            /*
            if (Input.GetAxis("P1Horizontal") > 0.5f)
            {
                Debug.Log("P1Horizontal: " + Input.GetAxis("P1Horizontal"));
            }
            if (Input.GetAxis("P1Vertical") > 0.5f)
            {
                Debug.Log("P1Vertical: " + Input.GetAxis("P1Vertical"));
            }
            if (Input.GetAxis("P1Fire") > 0.5f)
            {
                Input.ax
                KeyCode.jo
                Debug.Log("P1fire: " + Input.GetAxis("P1Fire"));
            }

    */
        }
    }
}