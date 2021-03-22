using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Boss : MonoBehaviour
{
    public enum BossState
    {
        Draw,
        Teleport,
        Beam,
        Thrust,
        Spin,
        Hit,
        Death
    }

    public static class States
    {
        public class State
        {
            protected Boss boss;
            virtual public State Update()
            {
                return null;
            }
            virtual public void OnStart(Boss boss)
            {
                this.boss = boss;
            }
            virtual public void OnEnd()
            {

            }
        }

        public class Draw : State
        {
        }

        public class Teleport : State
        {
            float particleTime = 1f;
            public override void OnStart(Boss boss)
            {
                boss.SmokeOut();
            }
            public override State Update()
            {
                particleTime -= Time.deltaTime;
                if (particleTime <= 0) return new States.Beam();
                return null;
            }
        }

        public class Beam : State
        {
            public override void OnStart(Boss boss)
            {
                boss.SmokeIn();
            }
            public override State Update()
            {
                return new States.Teleport();
            }
        }

        public class Thrust : State
        {
        }

        public class Spin : State
        {
        }

        public class Hit : State
        {
        }

        public class Death : State
        {
        }
    }

    private States.State state;
    public ParticleSystem smoke;

    // Start is called before the first frame update
    void Start()
    {
    }

    // Update is called once per frame
    void Update()
    {
        if (state == null) SwitchState(new States.Teleport());

        if (state != null)
        {
            if (state != null) SwitchState(state.Update());
        };

        //if (timerSpawnBullt <= 0)
    }

    void SwitchState(States.State newState)
    {
        if (newState == null) return;

        if (state != null) state.OnEnd();

        state = newState;

        state.OnStart(this);
    }

    void SmokeOut()
    {
        Instantiate(smoke, transform.position, transform.rotation);
        transform.position = transform.forward * (-20);
    }

    void SmokeIn()
    {
        Instantiate(smoke, transform.position, transform.rotation);
        transform.position = transform.right * Random.Range(-10f, 10f) + transform.up * Random.Range(-5f, 5f);
    }
}
