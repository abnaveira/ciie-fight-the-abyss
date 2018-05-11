using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.SceneManagement;

public class DeathMenu : MonoBehaviour {

	public void RetryGame()
    {
        SceneManager.LoadScene("Phase 1");
    }

    public void ExitGame()
    {
        Application.Quit();
    }

}
