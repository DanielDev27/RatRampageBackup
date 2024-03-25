using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class AnimationEventManager : MonoBehaviour
{
    public void ThrowBook()
    {
        GameObject currentProjectile = AntagonistAI.Instance.MakeProjectile();
        Vector3 targetPosition = AntagonistAI.Instance.antagonistActions.targetPosition;
        currentProjectile.transform.position = AntagonistAI.Instance.antagonistActions.projectileSpawn.position;
        currentProjectile.transform.forward = AntagonistAI.Instance.antagonistActions.projectileSpawn.forward;
        currentProjectile.GetComponent<Rigidbody>().velocity = (AntagonistAI.Instance.antagonistActions.characterController.transform.position - currentProjectile.transform.position) * AntagonistAI.Instance.antagonistActions.projectileSpeedFactor;
    }
}
