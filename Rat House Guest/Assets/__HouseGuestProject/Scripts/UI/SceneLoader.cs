using System.Collections;
using UnityEngine;
using UnityEngine.SceneManagement;
using UnityEngine.UI;
using DG.Tweening;

namespace DanielUI {
    public class SceneLoader : MonoBehaviour {
        public CanvasGroup transitionCanvasGroup;
        public Image progressBar;
        [SerializeField] float progress;
        public float transitionDuration = 1f;

        private AsyncOperation sceneLoadOperation;
        static SceneLoader instance;
        [SerializeField] string StartScene;
        [SerializeField] string NextScene;
        
        public void LoadScene () {
            // Start the transition process
            StartCoroutine (Transition ());
        }

        IEnumerator Transition () {
            DontDestroyOnLoad (gameObject);
            // Fade in the canvas group
            transitionCanvasGroup.alpha = 0f;
            transitionCanvasGroup.interactable = true;
            transitionCanvasGroup.blocksRaycasts = true;
            transitionCanvasGroup.DOFade (1f, transitionDuration);
            Scene activeScene = SceneManager.GetActiveScene ();
            Debug.Log ($"<color=green>Fade in load screen {activeScene.name}</color>");
            yield return new WaitForSeconds (transitionDuration);

            // Load the designated scene in the background
            sceneLoadOperation = SceneManager.LoadSceneAsync ($"{NextScene}", LoadSceneMode.Additive);
            sceneLoadOperation.allowSceneActivation = false;
            Debug.Log ($"<color=green>Load in the next scene</color>");

            // Update the progress bar as the scene loads
            while (sceneLoadOperation.progress < 0.9f) {
                progress = Mathf.Clamp01 (sceneLoadOperation.progress / 0.9f);
                progressBar.fillAmount = progress;
                //Debug.Log ("Update progress bar");
                yield return null;
            }

            // Activate the new scene
            Scene newScene = SceneManager.GetSceneByName ($"{NextScene}");
            sceneLoadOperation.allowSceneActivation = true;
            while (!sceneLoadOperation.isDone) {
                yield return null;
            }

            SceneManager.SetActiveScene (newScene);
            Debug.Log ($"<color=green>Activate new scene</color>");

            // Unload the current scene
            var _unloadScene = SceneManager.UnloadSceneAsync (activeScene);

            // Fade out the canvas group
            transitionCanvasGroup.DOFade (0f, transitionDuration);
            yield return new WaitForSeconds (transitionDuration);
            Debug.Log ($"<color=green>Unload current scene</color>");
            while (_unloadScene is { isDone: false }) {
                yield return null;
            }

            // Destroy the transition object once the process is complete
            if (gameObject != null) {
                Destroy (gameObject);
            }

            Debug.Log ($"<color=green>Get rid of load scene</color>");
        }

        public void ReturnToMainScene () {
            SceneManager.LoadScene (0);
            Time.timeScale = 1;
        }

        public void GoToGameScene () {
            SceneManager.LoadScene (1);
            Time.timeScale = 1;
        }
    }
}