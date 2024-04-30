using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class StardustParticle : MonoBehaviour
{
    ParticleSystem ps;
    private void Start()
    {
        ps = GetComponent<ParticleSystem>();
        Collider playerCollider = GameObject.FindGameObjectWithTag("Player").GetComponent<Collider>();

        ps.trigger.AddCollider(playerCollider);
    }
    void OnParticleTrigger()
    {
        List<ParticleSystem.Particle> enter = new List<ParticleSystem.Particle>();

        int numEnter = ps.GetTriggerParticles(ParticleSystemTriggerEventType.Enter, enter);

        if (Level1Controller.stardustMeter < Level1Controller.maxStardustMeter) //if the stardust meter isn't full
        {
            for (int i = 0; i < numEnter; i++)
            {
                ParticleSystem.Particle p = enter[i];

                p.remainingLifetime = 0;

                if (CompareTag("Big Stardust")) { Level1Controller.stardustMeter += 10; }
                else { Level1Controller.stardustMeter++; }

                Level1Controller.stardustRatio = Level1Controller.stardustMeter / Level1Controller.maxStardustMeter;

                float changeValue = Mathf.Clamp(Level1Controller.stardustRatio, 0.1f, Level1Controller.stardustRatio);
                StartCoroutine(Level1Controller.IncreaseLightRange(20 * Level1Controller.stardustRatio, changeValue / 20));
                StartCoroutine(Level1Controller.IncreaseSkyboxLightness(Level1Controller.stardustRatio, changeValue / 200));

                Debug.Log(Level1Controller.stardustMeter);

                var main = ps.main;

                if (ps.main.maxParticles <= 1) //destroy gameobject when no particles remain
                {
                    ps.Stop();
                    Destroy(ps.gameObject);
                }

                else //reduce amount of max particles otherwise
                {
                    main.maxParticles -= 1;
                }

                enter[i] = p;
            }
        }

        if (ps.isEmitting)
        {
            ps.SetTriggerParticles(ParticleSystemTriggerEventType.Enter, enter);
        }
    }

}
