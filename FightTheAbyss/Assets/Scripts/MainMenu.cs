using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.SceneManagement;

namespace FightTheAbyss
{
    public class MainMenu : MonoBehaviour
    {

        public GameObject sceneFade;
        private SceneChangeFade scriptSceneFade;

        public void Start()
        {
            scriptSceneFade = sceneFade.GetComponent<SceneChangeFade>();
        }

        public void PlayGame()
        {
            scriptSceneFade.changeScene("Phase 1");
        }

        public void ExitGame()
        {
            Application.Quit();
        }

    }
}
