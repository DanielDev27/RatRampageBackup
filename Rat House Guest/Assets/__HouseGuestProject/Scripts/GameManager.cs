using UnityEngine;

public class GameManager : MonoBehaviour
{
    public static GameManager Instance;
    [SerializeField] Canvas gameFinished;
    [SerializeField] Canvas gameOver;
    [SerializeField] Canvas pause;
    [SerializeField] GameObject frank;

    private void Awake()
    {
        gameFinished.enabled = false;
        gameOver.enabled = false;
        frank.SetActive(true);
    }
    void Start()
    {
        Instance = this;
        CharacterController.Instance.GetComponent<Rigidbody>().isKinematic = false;
    }

    public void GameComplete()
    {
        frank.SetActive(false);
        CharacterController.Instance.GetComponent<Rigidbody>().isKinematic = true;
        gameFinished.enabled = true;
        pause.enabled = false;
        Debug.Log("Game Completed");
        AudioManager.Instance.PlayMainMenu();
        CharacterController.Instance.CursorSettings(true, CursorLockMode.None);
    }
    public void GameOver()
    {
        frank.SetActive(false);
        CharacterController.Instance.GetComponent<Rigidbody>().isKinematic = true;
        gameOver.enabled = true;
        pause.enabled = false;
        Debug.Log("Game Over");
        AudioManager.Instance.PlayGameOver();
        CharacterController.Instance.CursorSettings(true, CursorLockMode.None);
    }

}
