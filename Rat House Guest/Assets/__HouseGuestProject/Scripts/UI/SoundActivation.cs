using UnityEngine;

public class SoundActivation : MonoBehaviour {
    [SerializeField] AudioSource click;

    public void ClickSound () {
        click.Play ();
    }
}