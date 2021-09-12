using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.InputSystem;

public class Player : MonoBehaviour
{
    public Animator moveAnimator;
    public Animator attackAnimator;
    public InputMaster controls;
    public GameObject crossHair;
    public GameObject slashPrefab;
    public Rigidbody2D rb;
    // movement input
    Vector2 move = Vector2.zero;
    // aiming input
    Vector2 moveCrossHair = Vector2.zero;
    Vector2 moveCrossHair_mouse = Vector2.zero;
    // attack input
    bool isFired = false;
    int holdFire= 0;
    // character status
    int attackDelay = 24; // in frames
    
    

    // Awake is called before Start()
    void Awake(){

        Screen.SetResolution(1920, 1080, true);
        controls = new InputMaster();
        controls.Player.Fire.performed += _ => isFired = true;
        controls.Player.Fire.canceled += _ =>  isFired = false;
        controls.Player.Fire.canceled += _ => holdFire = 0;
        controls.Player.Movement.performed += ctx => move = ctx.ReadValue<Vector2>();
        controls.Player.Movement.canceled += _ => move = Vector2.zero;
        controls.Player.Aim.performed += ctx => moveCrossHair = ctx.ReadValue<Vector2>();
        controls.Player.Aim.canceled += _ => moveCrossHair = Vector2.zero;
        controls.Player.MousePosition.performed += ctx => MousePosition(ctx.ReadValue<Vector2>());
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
        moveAnimator.SetFloat("MoveHorizontal", movement.x);
        moveAnimator.SetFloat("MoveVertical", movement.y);
        moveAnimator.SetFloat("MoveMagnitude", movement.magnitude);
        //transform.position = transform.position + movement*Time.deltaTime;
        rb.velocity=new Vector2(movement.x, movement.y);

        // aim 
        Vector3 movementCrossHair =  Vector3.zero;
        if (moveCrossHair.magnitude > 0.0f){
            // if is aming
            crossHair.SetActive(true);
            controls.Player.Fire.Enable();
            movementCrossHair = new Vector3(moveCrossHair.x, moveCrossHair.y,0.0f);
            movementCrossHair.Normalize();
            crossHair.transform.localPosition = movementCrossHair;

        } else {
            crossHair.SetActive(false);
            controls.Player.Fire.Disable();  
        }
        
        // attack
        attackAnimator.SetFloat("AttackHorizontal", movementCrossHair.x);
        attackAnimator.SetFloat("AttackVertical", movementCrossHair.y);
        attackAnimator.SetFloat("AttackMagnitude", movementCrossHair.magnitude);  
        attackAnimator.SetBool("isAttacked", isFired);  
        if (isFired) {
            if (holdFire % attackDelay == 0){
                Fire();
            }
            holdFire += 1;
        }
        
        
    }


    void Fire(){
        Vector2 slashDirection = new Vector2(moveCrossHair.x, moveCrossHair.y);    
        slashDirection.Normalize();
        GameObject slash = Instantiate(slashPrefab, transform.position, Quaternion.identity);
        slash.GetComponent<Rigidbody2D>().velocity = slashDirection*2;
        slash.transform.Rotate(0.0f, 0.0f, Mathf.Atan2(slashDirection.y, slashDirection.x)*Mathf.Rad2Deg);
        Destroy(slash, 2.0f);
    }

    void MousePosition(Vector2 position){
        Vector3 mousePosition = position;
        mousePosition.z = 20;
        mousePosition =  Camera.main.ScreenToWorldPoint(mousePosition) - transform.position;
        moveCrossHair.x = mousePosition.x;
        moveCrossHair.y = mousePosition.y;
    }
}
