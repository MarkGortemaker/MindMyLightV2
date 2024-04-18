using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class StardustParticle : MonoBehaviour
{
    void OnParticleTrigger()
    {
        ParticleSystem ps = GetComponent<ParticleSystem>();

        List<ParticleSystem.Particle> enter = new List<ParticleSystem.Particle>();

        int numEnter = ps.GetTriggerParticles(ParticleSystemTriggerEventType.Enter, enter);

        for (int i = 0; i < numEnter; i++)
        {
            ParticleSystem.Particle p = enter[i];

            p.remainingLifetime = 0;
            Level1Controller.stardustMeter++;
            Debug.Log(Level1Controller.stardustMeter);

            var main = ps.main;

            if (ps.main.maxParticles <= 1)
            {
                ps.Stop();
                Destroy(ps.gameObject);
            }

            else
            {
                main.maxParticles -= 1;
            }

            enter[i] = p;
        }

        if (ps.isEmitting)
        {
            ps.SetTriggerParticles(ParticleSystemTriggerEventType.Enter, enter);
        }
    }

}
