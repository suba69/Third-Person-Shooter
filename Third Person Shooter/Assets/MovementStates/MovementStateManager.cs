using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class MovementStateManager : MonoBehaviour
{
    MovementBaseState currentState;

    public IdleState idle = new IdleState();
    public WalkState walk = new WalkState();
    public CrouchState crouch = new CrouchState();
    public RunState run = new RunState();

    public float moveSpeed = 3;
    [HideInInspector] public Vector3 dir;
    private float hzInput, vInput;
    CharacterController controller;

    [SerializeField] private float groundYOffset;
    [SerializeField] private LayerMask groundMask;
    Vector3 spherePos;

    [SerializeField] float gravity = -9.81f;
    Vector3 velocity;
    private void Start()
    {
        controller = GetComponent<CharacterController>();
        SwitchState(idle);
    }

    private void Update()
    {
        GetDirectionAndMove();
        Gravity();

        currentState.UpdateState(this);
    }

    public void SwitchState(MovementBaseState state)
    {
        currentState = state;
        currentState.EnterState(this);
    }
    private void GetDirectionAndMove()
    {
        hzInput = Input.GetAxis("Horizontal");
        vInput = Input.GetAxis("Vertical");

        dir = transform.forward * vInput + transform.right * hzInput;

        controller.Move(dir * moveSpeed * Time.deltaTime);
    }

    private bool Ground()
    {
        spherePos = new Vector3(transform.position.x, transform.position.y, transform.position.z);
        if (Physics.CheckSphere(spherePos, controller.radius, groundMask))
        {
            return true;
        }
        else
        {
            return false;
        }
    }

    private void Gravity()
    {
        if (!Ground()) velocity.y += gravity * Time.deltaTime;
        else if (velocity.y < 0)
        {
            velocity.y = 0;
        }

        controller.Move(velocity * Time.deltaTime);
    }
}
