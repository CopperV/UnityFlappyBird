using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.InputSystem;

[RequireComponent(typeof(CharacterController))]
public class PlayerController : MonoBehaviour
{
    [SerializeField]
    private CharacterController characterController;
    [SerializeField]
    private GameObject model;

    [SerializeField]
    private Animator animator;
    [SerializeField]
    private string flapTrigger;

    [SerializeField]
    private float jumpForce = 5f;
    [SerializeField]
    private float gravity = 1f;

    [SerializeField]
    private InputActionReference jumpInput;
    [SerializeField, Min(0)]
    private float jumpCooldown = 0.1f;

    private float jumpCooldownTimer = 0f;

    [SerializeField]
    private float jumpAngleRotation = 60;
    [SerializeField]
    private float jumpAngleRotationSpeed = 5f;
    [SerializeField]
    private float fallAngleRotation = -90;
    [SerializeField]
    private float fallAngleRotationSpeed = 1f;

    private Vector3 velocity = Vector3.zero;

    private void Awake()
    {
        jumpInput.action.performed += DoJump;
    }

    private void Update()
    {
        if(jumpCooldownTimer > 0)
            jumpCooldownTimer -= Time.deltaTime;

        velocity.y -= gravity;
        characterController.Move(velocity * Time.deltaTime);

        if (velocity.y > 0)
            RotateCharacter(jumpAngleRotation, jumpAngleRotationSpeed * Time.deltaTime);
        else
            RotateCharacter(fallAngleRotation, fallAngleRotationSpeed * Time.deltaTime);
    }


    private void OnEnable()
    {
        jumpInput.action.Enable();
    }

    private void OnDisable()
    {
        jumpInput.action.Disable();
    }

    private void RotateCharacter(float targetAngle, float rotationSpeed)
    {
        float currentAngle = model.transform.eulerAngles.z;
        float angle = Mathf.LerpAngle(currentAngle, targetAngle, rotationSpeed);

        model.transform.rotation = Quaternion.Euler(0, 0, angle);
    }

    private void DoJump(InputAction.CallbackContext callbackContext)
    {
        if (jumpCooldownTimer > 0)
            return;

        jumpCooldownTimer = jumpCooldown;
        velocity.y = jumpForce;

        animator.SetTrigger(flapTrigger);
    }

}
