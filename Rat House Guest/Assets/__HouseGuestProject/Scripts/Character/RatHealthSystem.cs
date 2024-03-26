using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;
using Sirenix.OdinInspector;
using UnityEngine.Rendering;
using UnityEngine.Rendering.Universal;

public class RatHealthSystem : MonoBehaviour
{
    public static RatHealthSystem Instance;
    public float maxHealth;
    public float currentHealth;

    [SerializeField] Image healthBar;
    [SerializeField] Volume globalVolume;

    void Start()
    {
        Instance = this;
        currentHealth = maxHealth;
    }

    private void Update()
    {
        healthBar.fillAmount = currentHealth / maxHealth;
    }

    [Button]
    public void TakeDamage(float damage)
    {
        currentHealth -= damage;
        if (globalVolume.profile.TryGet(out Vignette damageVignette))
        {
            damageVignette.intensity.value = 0.5f;
            StartCoroutine(VignetteReset());
            StartCoroutine(HurtReset());
        }
        if (currentHealth <= 0)
        {
            Debug.Log("Health is zero. Game Over");
            GameManager.Instance.GameOver();
        }
    }

    private IEnumerator VignetteReset()
    {
        yield return new WaitForSeconds(0.5f);
        if (globalVolume.profile.TryGet(out Vignette damageVignette))
        {
            damageVignette.intensity.value = 0.0f;
        }
    }
    private IEnumerator HurtReset()
    {
        CharacterController.Instance.isHurt = true;
        yield return new WaitForSeconds(0.5f);
        CharacterController.Instance.isHurt = false;
    }
}
