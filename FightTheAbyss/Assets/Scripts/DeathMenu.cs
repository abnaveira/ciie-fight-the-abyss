using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.SceneManagement;

namespace FightTheAbyss
{
    public class DeathMenu : MonoBehaviour
    {
        private bool isCursorVisible = false;

        public GameObject sceneFade;
        private SceneChangeFade scriptSceneFade;

        public void Start()
        {
            Cursor.visible = true;
            Cursor.lockState = CursorLockMode.None;
            scriptSceneFade = sceneFade.GetComponent<SceneChangeFade>();
        }

        public void Update()
        {
            if (!isCursorVisible)
            {
                isCursorVisible = true;
                Cursor.lockState = CursorLockMode.None;
                Cursor.visible = true;
            }
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
