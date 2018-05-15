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
            Cursor.visible = true;
            Cursor.lockState = CursorLockMode.None;
            scriptSceneFade = sceneFade.GetComponent<SceneChangeFade>();
        }

        public void RetryGame()
        {
            Cursor.visible = false;
            Cursor.lockState = CursorLockMode.Locked;
            scriptSceneFade.changeScene(SceneManagement.lastScene);
        }

        public void ExitGame()
        {
            Application.Quit();
        }

    }
}
