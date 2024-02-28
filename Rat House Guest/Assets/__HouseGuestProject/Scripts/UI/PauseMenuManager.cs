using UnityEngine;
using UnityEngine.InputSystem;

public class PauseMenuManager : MonoBehaviour {
    // Start is called before the first frame update
    bool isPaused = false;
    [SerializeField] Canvas pauseCanvas;

    void Start () {
        pauseCanvas.enabled = isPaused;
        Time.timeScale = 1;
    }

    public void OnPauseInput (InputAction.CallbackContext incomingValue) {
        if (incomingValue.performed) {
            isPaused = !isPaused;
            pauseCanvas.enabled = isPaused;
        }

        /*if (isPaused) {
            Time.timeScale = 0;
        }

        if (!isPaused) {
            Time.timeScale = 1;
        }*/
    }

    public void OnResume () {
        isPaused = !isPaused;
        pauseCanvas.enabled = isPaused;
    }
}