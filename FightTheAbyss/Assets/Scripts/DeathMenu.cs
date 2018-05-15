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
            Debug.LogWarning(SceneManagement.lastScene);
            scriptSceneFade.changeScene(SceneManagement.lastScene);
        }

        public void ExitGame()
        {
            Application.Quit();
        }

    }
}
