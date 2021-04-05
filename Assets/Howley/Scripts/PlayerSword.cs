using System.Collections;
using System.Collections.Generic;
using UnityEngine;

namespace Howley
{
    public class PlayerSword : MonoBehaviour
    {
        static class States
        {
            public class State
            {
                protected PlayerSword sword;

                virtual public State Update()
                {
                    return null;
                }
                virtual public void OnStart(PlayerSword sword)
                {
                    this.sword = sword;
                }
                virtual public void OnEnd()
                {

                }
            }

            public class Regular : State
            {
                public override State Update()
                {
                    // behavior:

                    // transition:
                    if (Input.GetButton("Fire1")) sword.SwitchState(new States.Swing());
                    return null;
                }
            }
            public class Swing : State
            {
                public override State Update()
                {
                    // behavior:
                    sword.SwingSword();
                    sword.swingCooldown = 2;
                    // transition:
                    if (sword.swingSeconds >= .5f) sword.SwitchState(new States.Cooldown());
                    return null;
                }
            }
            public class Cooldown : State
            {
                public override State Update()
                {
                    // behavior:
                    sword.swingSeconds = 0;
                    sword.SwingCooldown();

                    // transition:
                    // When the cooldown is over, the state is back to normal.
                    if (sword.swingCooldown <= 0) sword.SwitchState(new States.Regular());

                    return null;
                }
            }
        }

        private States.State state;

        public Transform sword;

        private float swingCooldown = 0;

        private float swingSeconds = 0;



        private void Update()
        {
            // If state doesn't exist, set it up.
            if (state == null) SwitchState(new States.Regular());

            // Call state.update, if it returns a state, switch to the returned state.
            if (state != null) SwitchState(state.Update());

            print(state);
        }

        void SwitchState(States.State newState)
        {
            // If there is nothing to switch to
            if (newState == null) return;

            if (state != null) state.OnEnd(); // Call the current state's OnEnd

            // Switch the state
            state = newState;

            // Call the new state's OnStart function
            state.OnStart(this);
        }

        void SwingCooldown()
        {
            // Countdown the cooldown
            swingCooldown -= Time.deltaTime;
        }

        void SwingSword()
        {
            Quaternion startingRot = sword.transform.rotation;
            Quaternion targetRot = sword.transform.rotation * Quaternion.Euler(0, -60, 0);

            swingSeconds += Time.deltaTime;

            if (swingSeconds >= .25f) targetRot = sword.transform.rotation * Quaternion.Euler(0, 60, 0);

            sword.transform.rotation = AnimMath.Slide(startingRot, targetRot, .0001f);
        }

    }
}

