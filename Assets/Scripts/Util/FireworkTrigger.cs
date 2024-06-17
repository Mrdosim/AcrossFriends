using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class FireworkTrigger : MonoBehaviour
{
    private ParticleSystem fireworkParticle;

    private void Awake()
    {
        fireworkParticle = GetComponent<ParticleSystem>();
    }

    private void OnTriggerEnter(Collider other)
    {
        if (other.CompareTag("Player"))
        {
            // ÆøÁ× ÆÄÆ¼Å¬ È¿°ú Àç»ý
            fireworkParticle.Play();

            // ÆøÁ× È¿°ú°¡ ³¡³­ ÈÄ ÆÄ±«
            Destroy(gameObject, fireworkParticle.main.duration);
        }
    }
}
