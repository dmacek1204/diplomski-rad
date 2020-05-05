using Mirror;
using System.Collections;
using UnityEngine;

public class PlayerController : NetworkBehaviour
{
    [SerializeField]
    private float moveSpeed = 8f;
    [SerializeField]
    private float turnSpeed = 5f;
    [SerializeField]
    private float shootingTimeDisable = 1f;

    private bool isShooting = false;

    private CharacterController characterController;

    [SerializeField]
    private Transform lookAtTransform;

    void Awake()
    {
        characterController = GetComponent<CharacterController>();
    }

    void Update()
    {
        var horizontal = Input.GetAxisRaw("Horizontal");
        var vertical = Input.GetAxisRaw("Vertical");
        if(Input.GetButton("Fire2"))
        {
            PlayerMovement(horizontal, vertical, false);
            return;
        }
            if (Input.GetButtonDown("Fire1"))
        {
            StartCoroutine(DisableMovementIfShooting());

        }


        if (!isShooting)
        {

            PlayerMovement(horizontal, vertical);
        }

    }


    public void PlayerMovement(float horizontal, float vertical, bool canMove = true)
    {
        var movement = new Vector3(0, 0, 0);

        var xVector = new Vector3();
        var zVector = new Vector3();
        if (vertical == 1)
        {
            xVector = lookAtTransform.forward;
        }
        if (vertical == -1)
        {
            xVector = -lookAtTransform.forward;
        }
        if (horizontal == 1)
        {
            zVector = lookAtTransform.right;
        }
        if (horizontal == -1)
        {
            zVector = -lookAtTransform.right;
        }
        movement = xVector + zVector;

        movement.y = 0;
        movement.Normalize();

        if (canMove)
        {
            characterController.SimpleMove(movement * Time.deltaTime * moveSpeed);
        }

        if (movement.magnitude > 0)
        {
            Quaternion newDirection = Quaternion.LookRotation(movement);

            transform.rotation = Quaternion.Slerp(transform.rotation, newDirection, Time.deltaTime * turnSpeed);
        }
    }

    private IEnumerator DisableMovementIfShooting()
    {
        isShooting = true;
        yield return new WaitForSecondsRealtime(shootingTimeDisable);
        isShooting = false;
    }
}
