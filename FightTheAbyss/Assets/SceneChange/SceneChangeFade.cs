using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.SceneManagement;
using UnityEngine.UI;
namespace FightTheAbyss
{
    public class SceneChangeFade : MonoBehaviour
    {
        // public Image fadeOutImage;
        public Texture2D loadingTexture;
        public Texture2D controlsTexture;
        public Texture2D fadeOutTexture;        // Texture overlaying scene changes
        public float fadeSpeed =0.8f;          // Fading speed

        private int drawDepth = -1000;          // Texture draw order, so it renders on top of others
        private float alpha = 1.0f;             // Texture alpha value between 0 and 1
        private int fadeDir = -1;               // Direction to fade (-1 = in, 1 = out)


        private bool loadingScene=false;
        private float widthControls;
        private float widthLoading;
        private float heightControls;
        private float heightLoading;
        private void OnGUI()
        {
           /* // if (fading)
              // {
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
              // }*/
              

                alpha += fadeDir * fadeSpeed * Time.deltaTime;
                alpha = Mathf.Clamp01(alpha);
                GUI.color = new Color(GUI.color.r, GUI.color.g, GUI.color.b, alpha);
                GUI.depth = drawDepth+2;
                GUI.DrawTexture(new Rect(0, 0, Screen.width, Screen.height), fadeOutTexture);
                if (loadingScene)
                {
                    widthControls = Screen.width / 2;
                    heightControls = Screen.height / 2;
                    widthLoading = Screen.width / 3;
                    heightLoading = Screen.width / 10;
                    GUI.color = new Color(GUI.color.r, GUI.color.g, GUI.color.b, alpha);
                    GUI.depth = drawDepth+1;
                    GUI.DrawTexture(new Rect((Screen.width / 2) - (widthControls / 2), (Screen.height / 2) - (heightControls / 2),widthControls,heightControls), controlsTexture);

                    GUI.color = new Color(GUI.color.r, GUI.color.g, GUI.color.b, Mathf.PingPong(Time.time, alpha));
                    GUI.depth = drawDepth;
                    GUI.DrawTexture(new Rect((Screen.width / 2) - (widthLoading / 2), (Screen.height *(2/ 3)), widthLoading,heightLoading), loadingTexture);

                }

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
        void Start()
        {

            // Use this is the alpha is not set to 1 by default
            alpha = 1;
            // Call the fade function
            loadingScene = false;
            BeginFade(-1);
        }

        public void changeScene(string level)
        {
            loadingScene = true;
            float fadeTime = BeginFade(1);
            // Fading takes time
            //new WaitForSeconds(fadeTime);
            //SceneManager.LoadScene(level);
            StartCoroutine(loadSceneCoroutine(level));
        }

        IEnumerator loadSceneCoroutine(string level)
        {
            yield return new WaitForSeconds(4f);
            AsyncOperation async = SceneManager.LoadSceneAsync(level);
            while (!async.isDone)
            {
                yield return null;
            }
        }

        [SerializeField] private string loadLevel;

        void OnTriggerEnter(Collider other)
        {
            if (other.CompareTag("Player"))
            {
                changeScene(loadLevel);
            }
        }
    }
}
