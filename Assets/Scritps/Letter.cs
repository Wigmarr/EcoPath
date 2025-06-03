using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Letter : MonoBehaviour
{
    [SerializeField] private ParticleSystem partSys;
    private void OnTriggerEnter2D(Collider2D collision)
    {
         if (collision.tag == "Player")
        {
            var instance = Instantiate<ParticleSystem>(partSys);
            instance.transform.position = gameObject.transform.position;
            instance.Play();
            Destroy(instance, instance.duration);
            Destroy(gameObject);
        }
    }
}
