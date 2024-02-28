using UnityEngine;
using UnityEngine.UI;
using UnityEngine.EventSystems;
using DG.Tweening;

namespace DanielUI {
    public class ButtonScale : MonoBehaviour, IPointerEnterHandler, IPointerExitHandler {
        public Button button;
        public Vector3 highlightedScale = new Vector3 (1f, 1f, 1f);
        public float scaleDuration = 0.3f;

        private Vector3 originalScale;

        private void Start () {
            // Store the original scale of the button
            originalScale = button.transform.localScale;
        }

        public void OnPointerEnter (PointerEventData eventData) {
            // Scale the button to the highlighted scale using DOTween
            button?.transform.DOScale (highlightedScale, scaleDuration);
        }

        public void OnPointerExit (PointerEventData eventData) {
            // Scale the button back to the original scale using DOTween
            button?.transform.DOScale (originalScale, scaleDuration);
        }

        public void SetOriginalScale () {
            button?.transform.DOScale (originalScale, scaleDuration);
        }
    }
}