using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Trunk : MonoBehaviour
{
   
    private void OnTriggerEnter(Collider other)
    {
        if(other.TryGetComponent(out MainCharacterController controller))
        {
            // interact
        }
    }
}
