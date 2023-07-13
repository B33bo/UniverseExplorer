using UnityEngine;

namespace Universe
{
    public class Player : MonoBehaviour
    {
        [SerializeField]
        private float speed;

        [SerializeField]
        private float jumpPower;

        [SerializeField]
        private Rigidbody2D rb;

        [SerializeField]
        private Transform[] feet;

        private float lastJump = float.NegativeInfinity;

        private void FixedUpdate()
        {
            transform.position += speed * Time.fixedDeltaTime * new Vector3(Input.GetAxisRaw("Horizontal"), 0);
        }

        private bool IsGrounded()
        {
            for (int i = 0; i < feet.Length; i++)
            {
                if (Physics2D.Raycast(feet[i].position, Vector2.down, .2f))
                    return true;
            }
            return false;
        }

        private void Jump()
        {
            if (Time.time - lastJump < .2f)
                return;

            if (!IsGrounded())
                return;

            lastJump = Time.time;
            rb.velocity = Vector3.zero;
            rb.AddForce(Vector2.up * jumpPower);
        }

        private void Update()
        {
            if (Input.GetButton("Jump"))
                Jump();
        }
    }
}
