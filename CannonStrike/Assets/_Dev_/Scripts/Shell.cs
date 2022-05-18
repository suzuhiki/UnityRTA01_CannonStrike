using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

namespace CannonStrike
{
    public class Shell : MonoBehaviour
    {
        [SerializeField] private Color _targetDestroyedColor;
    
        private void OnCollisionEnter(Collision collision)
        {
            if (collision.gameObject.CompareTag("Target"))
            {
                collision.gameObject.GetComponent<MeshRenderer>().material.color = _targetDestroyedColor;
                collision.gameObject.tag = "Untagged";
                
                GameManager.instance.OnDestroyTarget();
            }
        
            Destroy(this.gameObject);
        }
    }
}

