using UnityEngine;

public class SpiritParticlesEmitter : MonoBehaviour
{
    [SerializeField, Tooltip("Жизненный цикл этого духа.")]
    private CharacterLifecycle lifecycle;

    [SerializeField, Tooltip("Системы частиц постоянного излучения духа.")]
    private ParticleSystem particles;

    private void OnEnable()
    {
        lifecycle.AliveStateChanged += SetAlive;

    }

    private void OnDisable()
    {
        lifecycle.AliveStateChanged -= SetAlive;
    }

    private void SetAlive(bool isAlive)
    {
        if (isAlive)
        {
            particles.Play();
            return;
        }

        particles.Stop();
    }

 
}