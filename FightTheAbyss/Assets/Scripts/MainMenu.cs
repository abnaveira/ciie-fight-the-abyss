using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.SceneManagement;

namespace FightTheAbyss
{
    public class MainMenu : MonoBehaviour
    {
        private bool isCursorVisible = false;

        public GameObject sceneFade;
        private SceneChangeFade scriptSceneFade;

        public void Start()
        {
            Cursor.visible = true;
            scriptSceneFade = sceneFade.GetComponent<SceneChangeFade>();
        }

        public void Update()
        {
            if (!isCursorVisible)
            {
                isCursorVisible = true;
                Cursor.visible = true;
            }
        }

        public void PlayGame()
        {
            Cursor.visible = false;
            scriptSceneFade.changeScene("Phase 1");
        }

        public void ExitGame()
        {
            Application.Quit();
        }

    }
}
