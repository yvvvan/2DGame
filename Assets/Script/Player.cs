using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.InputSystem;


public class Player : MonoBehaviour
{
    public Animator animator;
    public InputMaster controls;
    public GameObject crossHair;

    Vector2 move = Vector2.zero;
    Vector2 moveCrossHair = Vector2.zero;

    // Awake is called before Start()
    void Awake(){
        controls = new InputMaster();
        controls.Player.Fire.performed += _ => Fire();
        controls.Player.Movement.performed += ctx => move = ctx.ReadValue<Vector2>();
        controls.Player.Movement.canceled += ctx => move = Vector2.zero;
        controls.Player.Aim.performed += ctx => moveCrossHair = ctx.ReadValue<Vector2>();
        controls.Player.Aim.canceled += ctx => moveCrossHair = Vector2.zero;
    }

    // must do
    private void OnEnable(){
        controls.Enable();
    }

    private void OnDisable(){
        controls.Disable();
    }

    // Start is called before the first frame update
    void Start()
    {

    }

    // Update is called once per frame
    void Update()
    {   
        // move
        Vector3 movement = new Vector3(move.x, move.y,0.0f);
        
        animator.SetFloat("Horizontal", movement.x);
        animator.SetFloat("Vertical", movement.y);
        animator.SetFloat("Magnitude", movement.magnitude);

        transform.position = transform.position + movement*Time.deltaTime;

        // aim
        if (moveCrossHair.magnitude > 0.0f){
            crossHair.SetActive(true);
            Vector3 movementCrossHair = new Vector3(moveCrossHair.x, moveCrossHair.y,0.0f);
            movementCrossHair.Normalize();
            crossHair.transform.localPosition = movementCrossHair;
        } else {
            crossHair.SetActive(false);
        }
        
    }


    void Fire(){
        Debug.Log("FIRE!"); 
    }

    void MoveCrossHair(){

    }
}
