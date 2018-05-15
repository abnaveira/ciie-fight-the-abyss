using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.SceneManagement;

namespace FightTheAbyss
{
    public class DeathMenu : MonoBehaviour
    {

        public GameObject sceneFade;
        private SceneChangeFade scriptSceneFade;

        public void Start()
        {
            scriptSceneFade = sceneFade.GetComponent<SceneChangeFade>();
        }

        public void RetryGame(string scene)
        {
            scriptSceneFade.changeScene(scene);
        }

        public void ExitGame()
        {
            Application.Quit();
        }

    }
}
