using System.Collections.Generic;
using UnityEngine;

public class PlayerMovement : MonoBehaviour
{
    // Set Scale to (0.9, 0.9, 0.9) in inspector
    // becoz default scale of 1 does not work with
    // tile size of 1 (collision problems)

    /* Linear drag will be calculated based on the
     * terminal velocity and gravity. Friction 
     * determines how fast player stops on the ground
     * when input is stopped.
     */

    [Header("Public")]
    public float mass; // 2
    public float gravityScale; // 9.5
    public float max_fall_speed; // 25
    public float max_move_speed_on_ground; // 10.57
    public float COF; // coeff of friction 0.2
    public PhysicsMaterial2D player; // attach to player
    public PhysicsMaterial2D ground; // attach to ground
    public GameObject heightMeterObj;
    public float jump_force; // 82.5
    public float minContactNormalY; // 0.6

    [Header("Private")]
    [SerializeField] private bool canJump;

    Rigidbody2D _rb;

    float gravityGlobal;
    float total_force; // based on max move speed and linear drag
    float move_force; // with or without friction (ground / air)

    bool grounded;
    bool prevGrounded; // to determine Ground State Change

    // Debug
    private Vector3 initial_position;
    Transform heightMeter; // just an indicator

    private void Start()
    {
        _rb = GetComponent<Rigidbody2D>();

        player.friction = COF;
        ground.friction = COF;

        gravityGlobal = Mathf.Abs(Physics2D.gravity.y);

        _rb.mass = mass;
        _rb.gravityScale = gravityScale;
        _rb.drag = gravityScale * gravityGlobal / max_fall_speed;

        // F' = F - f, we need F (move_force)
        total_force = max_move_speed_on_ground * mass * _rb.drag;
        move_force = total_force + COF * mass * gravityScale * gravityGlobal;

        prevGrounded = grounded = false;
        canJump = false;

        // Debug
        initial_position = transform.position;
        heightMeter = Instantiate(heightMeterObj, Vector3.zero, Quaternion.identity).transform;
    }

    private void FixedUpdate()
    {
        float move = Input.GetAxisRaw("Horizontal");
        _rb.AddForce(move * move_force * Vector3.right, ForceMode2D.Force);
    }

    private void Update()
    {
        HandleJump();
        HandleGroundState();

        // Debug
        ResetPosition();
    }

    // Debug
    private void ResetPosition()
    {
        if(Input.GetKeyDown(KeyCode.R))
        {
            transform.position = initial_position;
        }
    }

    private void HandleGroundState()
    {
        // grounded
        prevGrounded = grounded;

        List<ContactPoint2D> contacts = new List<ContactPoint2D>();
        int totalContacts = _rb.GetContacts(contacts);

        Vector2 netNormal = Vector2.zero;
        for (int i = 0; i < totalContacts; i++)
        {
            netNormal += contacts[i].normal;
        }

        netNormal /= totalContacts;
        netNormal = netNormal.normalized;

        grounded = netNormal.y > minContactNormalY ? true : false;

        if (grounded != prevGrounded)
        {
            if (grounded)
                GroundedStart();
            else
                GroundedExit();
        }
    }

    private void HandleJump()
    {
        // Jump (Height)
        if (Input.GetButtonDown("Jump") && canJump)
        {
            _rb.AddForce(Vector2.up * jump_force, ForceMode2D.Impulse);

            heightMeter.position = transform.position + 0.5f * transform.localScale.y * Vector3.down;
        }

        if (heightMeter.position.y < transform.position.y - 0.5f * transform.localScale.y)
        {
            heightMeter.position = transform.position + 0.5f * transform.localScale.y * Vector3.down;
        }
    }

    void GroundedStart()
    {
        canJump = true;

        player.friction = COF;
        ground.friction = COF;

        move_force = total_force + COF * mass * gravityScale * gravityGlobal;
    }

    void GroundedExit()
    {
        canJump = false;

        player.friction = 0;
        ground.friction = 0;

        move_force = total_force;
    }

}
