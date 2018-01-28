using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

namespace CargoStrategy.GameOver
{

    public class GameOverObject : MonoBehaviour
    {

        public Text VictorText;

        private void Start()
        {
            // set the text to whomever won.
            VictorText.text = (GameOverManager.Instance.Victor == Units.TeamIds.Player1 ? "Blue" : "Red") + "player is victorious!";
        }
        
        private void Update()
        {
            // TODO recieve input to go back to main menu.

        }
    }

}