using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

namespace Kortge
{
    /// <summary>
    /// Makes the player face in a specified direction so that the player can fire at the desired target.
    /// </summary>
    public class PlayerAiming : MonoBehaviour
    {
        /// <summary>
        /// Determines if the player is currently using a mouse to aim.
        /// </summary>
        private bool isAimingWithMouse = true;
        /// <summary>
        /// Determines how blue the sword icon is.
        /// </summary>
        private float blueness = 0;
        /// <summary>
        /// The component that plays animations for this object.
        /// </summary>
        private Animator animator;
        /// <summary>
        /// The camera used to view the game.
        /// </summary>
        private Camera cam;
        /// <summary>
        /// Shows if the player is currently in its stabbing animation.
        /// </summary>
        public bool stabbing;
        /// <summary>
        /// Shows if the player has picked up a rose and is ready to launch a projectile.
        /// </summary>
        public bool projectionReady = false;
        /// <summary>
        /// Controls the sound effects made by this object.
        /// </summary>
        public AudioManager audioManager;
        /// <summary>
        /// An effect that plays when the player picks up a rose.
        /// </summary>
        public ParticleSystem particles;
        /// <summary>
        /// The astral projections the player launches once it is ready.
        /// </summary>
        public Projectile projectionPrefab;
        /// <summary>
        /// The icon used to represent the status of the astral projection.
        /// </summary>
        public RawImage sword;
        /// <summary>
        /// The cursor usd to show what the player is meant to be aiming at.
        /// </summary>
        public Transform debugObject;
        /// <summary>
        /// This is destroyed after the player picks up their first rose.
        /// </summary>
        public GameObject roseTag;

        /// <summary>
        /// Hides the cursor within the game and gets the animator and camera components.
        /// </summary>
        // Start is called before the first frame update
        void Start()
        {
            Cursor.lockState = CursorLockMode.Confined;
            Cursor.visible = false;
            animator = GetComponentInChildren<Animator>();
            cam = Camera.main;
        }

        /// <summary>
        /// Aims the character using either a mouse or controller, stabs when ordered to, and updates the color of the sword UI sprite.
        /// </summary>
        // Update is called once per frame
        void Update()
        {
            if (Outbreak.Game.isPaused) return;
            AutoDetectInput();
            if (isAimingWithMouse) AimAtMouse();
            else AimWithController();
            if (Input.GetButtonDown("Fire1") && !stabbing)
            {
                Stab();
            }
            SwordUIColor();
        }

        /// <summary>
        /// Decides on whether the player is using a mouse or keyboard to aim.
        /// </summary>
        private void AutoDetectInput()
        {
            if (Input.GetAxisRaw("Mouse X") != 0||Input.GetAxisRaw("Mouse Y") != 0) isAimingWithMouse = true;
            if (Input.GetAxisRaw("AimHorizontal") != 0 || Input.GetAxisRaw("AimVertical") != 0) isAimingWithMouse = false;
        }
        /// <summary>
        /// Rotates the character in the direction of the mouse cursor.
        /// </summary>
        private void AimAtMouse()
        {
            // make a ray and a plane:
            Ray ray = cam.ScreenPointToRay(Input.mousePosition);
            Plane plane = new Plane(Vector3.up, transform.position);

            // does the ray hit the plane?
            if (plane.Raycast(ray, out float dis))
            {
                // find where the ray hit the plane:
                Vector3 hitPos = ray.GetPoint(dis);

                if (debugObject) debugObject.position = hitPos;

                Vector3 vectorToHitPos = hitPos - transform.position;

                float angle = Mathf.Atan2(vectorToHitPos.x, vectorToHitPos.z);

                angle /= Mathf.PI;
                angle *= 180; // Converts from radians to half-circles to degrees.
                transform.eulerAngles = new Vector3(0, angle, 0);
            }
        }
        /// <summary>
        /// Rotates the character in the direction of the joystick.
        /// </summary>
        private void AimWithController()
        {
            float h = Input.GetAxis("AimHorizontal");
            float v = Input.GetAxis("AimVertical");
            print(h + "" + v);
            float magSq = (h * h) + (v * v);

            if (magSq < 0.25f) return;

            float angle = Mathf.Atan2(h, v);

            angle *= Mathf.Rad2Deg; // convert to degrees

            transform.eulerAngles = new Vector3(0, angle, 0);
        }
        /// <summary>
        /// Executes the player's attack and launches an astral projection if it is ready.
        /// </summary>
        private void Stab()
        {
            animator.SetTrigger("Stab");
            audioManager.Play("Whoosh");
            if (projectionReady)
            {
                Projectile projection = Instantiate(projectionPrefab, transform.position + transform.forward, transform.rotation);
                projection.InitBullet(transform.forward * 20);
                projectionReady = false;
                audioManager.Play("Player Astral Projection");
            }
        }
        /// <summary>
        /// Adjusts the color of the sword UI based on if an astral projection is ready or not.
        /// </summary>
        private void SwordUIColor()
        {
            if (projectionReady) blueness += Time.deltaTime;
            else blueness -= Time.deltaTime * 5;
            blueness = Mathf.Clamp(blueness, 0, 1);
            sword.color = Color.Lerp(Color.white, Color.blue, blueness);
        }
        /// <summary>
        /// On collision with a rose, the player destroys the rose and readies the astral projection.
        /// </summary>
        /// <param name="hit"></param>
        private void OnControllerColliderHit(ControllerColliderHit hit)
        {
            if (hit.gameObject.CompareTag("Rose") && !projectionReady)
            {
                projectionReady = true;
                Destroy(hit.gameObject);
                particles.Play();
                audioManager.Play("Power Up");
                if (roseTag != null) Destroy(roseTag);
            }
        }
    }
}