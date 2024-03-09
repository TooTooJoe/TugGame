using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class ColliderTrigger : MonoBehaviour
{

    [SerializeField] MainSystem mainSystem;

    private void OnTriggerEnter(Collider other)
    {
        mainSystem.CompleteGameEarly();
    }
}
