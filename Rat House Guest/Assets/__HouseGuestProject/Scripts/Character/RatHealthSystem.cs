using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;
using Sirenix.OdinInspector;

public class RatHealthSystem : MonoBehaviour
{
    public static RatHealthSystem Instance;
    public float maxHealth;
    public float currentHealth;

    [SerializeField] Image healthBar;


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
        if (currentHealth <= 0)
        {
            Debug.Log("Health is zero. Game Over");
            GameManager.Instance.GameOver();
        }
    }
}
