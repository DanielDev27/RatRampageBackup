using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;
public class Projectile : MonoBehaviour
{
    [SerializeField] float bookDamage;

    private void Start()
    {
        StartCoroutine(ProjectileDespawn());
    }

    private void OnCollisionEnter(Collision other)
    {
        if (other.gameObject.TryGetComponent<CharacterController>(out CharacterController _characterController))
        {
            this.gameObject.SetActive(false);
            RatHealthSystem.Instance.TakeDamage(bookDamage);
        }
    }
    private IEnumerator ProjectileDespawn()
    {
        yield return new WaitForSeconds(5);
        this.gameObject.SetActive(false);
        Destroy(this);
    }
}
