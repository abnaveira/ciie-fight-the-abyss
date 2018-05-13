using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.SceneManagement;
namespace FightTheAbyss
{
    public class SceneChangeFade : MonoBehaviour
    {

        public Texture2D fadeOutTexture;        // Texture overlaying scene changes
        public float fadeSpeed = 0.8f;          // Fading speed

        private int drawDepth = -1000;          // Texture draw order, so it renders on top of others
        private float alpha = 1.0f;             // Texture alpha value between 0 and 1
        private int fadeDir = -1;               // Direction to fade (-1 = in, 1 = out)

        private void OnGUI()
        {
            // Fade out/in the alpha value using a direction, a speed and Time.deltaTime to convert the operation to seconds
            alpha += fadeDir * fadeSpeed * Time.deltaTime;
            // Force number between 0 and 1 (GUI.color uses value in this range)
            alpha = Mathf.Clamp01(alpha);

            // Set the color of the GUI (the texture). Color values remain the same, alpha changes
            GUI.color = new Color(GUI.color.r, GUI.color.g, GUI.color.b, alpha);
            // Make sure the texture renders on top
            GUI.depth = drawDepth;
            // Draw the texture
            GUI.DrawTexture(new Rect(0, 0, Screen.width, Screen.height), fadeOutTexture);
        }

        // Sets fadeDir to the direction parameter, making the scene fade in if -1 and out if 1
        public float BeginFade(int direction)
        {
            fadeDir = direction;
            // Return fadeSpeed as variable to time easily Application.LoadLevel()
            return (fadeSpeed);
        }

        // OnLevelWasLoaded is called when a level is loaded. It takes loaded level index (int) as a parameter
        // so you can limit the fadeIn to certain scenes
        private void OnLevelWasLoaded(int level)
        {
            // Use this is the alpha is not set to 1 by default
            // alpha = 1;
            // Call the fade function
            BeginFade(-1);
        }

        [SerializeField] private string loadLevel;

        void OnTriggerEnter(Collider other)
        {
            if (other.CompareTag("Player"))
            {
                float fadeTime = BeginFade(1);
                /*yield return */
                new WaitForSeconds(fadeTime);
                SceneManager.LoadScene(loadLevel);
            }
        }
    }
}
