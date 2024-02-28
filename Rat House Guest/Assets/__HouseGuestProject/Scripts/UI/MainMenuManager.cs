using System.Collections;
using UnityEngine;
using DG.Tweening;

public class MainMenuManager : MonoBehaviour
{
    [Header("Main Menu Screen")]
    public CanvasGroup mainMenu;
    public GameObject mainMenuObject;
    [SerializeField] int sceneToLoad;

    [Header("Loading Screen")]
    public GameObject loadingScreen;
    public CanvasGroup loadingMenu;

    private void Awake()
    {
        Time.timeScale = 1f;
    }

    private void Start()
    {
        AudioManager.Instance.PlayMainMenu();
    }

    public void SwitchToLoadingScreen()
    {
        StartCoroutine(SwitchToLoading());
    }

    IEnumerator SwitchToLoading()
    {
        mainMenu.DOFade(0, 0.3f);
        loadingMenu.DOFade(1, 0.3f);
        yield return new WaitForSeconds(0.4f);
        SceneLoaderManager.Instance.StartGame(sceneToLoad);
    }
}
