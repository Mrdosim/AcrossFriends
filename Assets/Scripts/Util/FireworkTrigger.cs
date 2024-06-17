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
            // ���� ��ƼŬ ȿ�� ���
            fireworkParticle.Play();

            // ���� ȿ���� ���� �� �ı�
            Destroy(gameObject, fireworkParticle.main.duration);
        }
    }
}
