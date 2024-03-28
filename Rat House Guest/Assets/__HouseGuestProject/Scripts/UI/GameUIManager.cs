using System.Collections;
using UnityEngine;
using UnityEngine.SceneManagement;
using DG.Tweening;

public class GameUIManager : MonoBehaviour
{
    public static GameUIManager Instance;
    [SerializeField] GameObject pauseMenu;
    [SerializeField] bool isMenuActive;
    bool isPauseRoutineRunning;
    [SerializeField] int menuScene;
    [SerializeField] CanvasGroup pauseGroup;
    [SerializeField] CanvasGroup loadingGroup;
    private void Awake()
    {
        Instance = this;
    }

    public void PauseMenu(bool isPaused)
    {
        isMenuActive = isPaused;
        if (isPaused)
        {
            pauseMenu.GetComponent<Canvas>().enabled = true;
            Cursor.lockState = CursorLockMode.None;
            Cursor.visible = true;
            isMenuActive = true;
            Time.timeScale = 0f;
        }
    }
    public void ResumeGame()
    {
        if (isPauseRoutineRunning)
        {
            StopCoroutine(WaitForPause(0f));
        }

        Time.timeScale = 1f;
        pauseMenu.GetComponent<Canvas>().enabled = false;
        Cursor.lockState = CursorLockMode.Locked;
        Cursor.visible = false;
        isMenuActive = false;

        CharacterController.Instance.pause = false;
    }
    IEnumerator WaitForPause(float delay)
    {
        isPauseRoutineRunning = true;
        yield return new WaitForSeconds(0.1f);
        pauseMenu.SetActive(true);
        Cursor.lockState = CursorLockMode.None;
        Cursor.visible = true;
        isMenuActive = true;
        yield return new WaitForSeconds(delay);
        Time.timeScale = 0f;
        yield return new WaitForSeconds(0.01f);
        isPauseRoutineRunning = false;
    }
    public void ReturnToMenu()
    {
        Debug.Log("Return To Menu");
        pauseGroup.DOFade(0, 0.3f);
        loadingGroup.DOFade(1, 0.3f);
        SceneLoaderManager.Instance.LoadScene(menuScene);
    }
}
