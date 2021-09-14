using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Slash : MonoBehaviour
{
    public Vector2 velocity = new Vector2(0.0f, 0.0f);
    public float degree = 0.0f;
    public GameObject knight;
    public Vector2 OFFSET = new Vector2(0.0f, -0.22f);
    public Vector2 SIZE = new Vector2(0.1f, 0.2f);

    void  Start() {
        //gameLogic = GameObject.Find("GameLogic").GetComponent<GameLogic>();
    }

    void Update()
    {
        Vector2 currentPosition = new Vector2(transform.position.x, transform.position.y);
        Vector2 newPosition = currentPosition + velocity * Time.deltaTime;
        
        Debug.DrawLine(currentPosition+OFFSET,newPosition+OFFSET,Color.red);

        //RaycastHit2D[] hits = Physics2D.LinecastAll(currentPosition + OFFSET, newPosition + OFFSET);
        float dis =  (velocity * Time.deltaTime).magnitude;
        RaycastHit2D[] hits = Physics2D.BoxCastAll(currentPosition+OFFSET, SIZE, degree, velocity, dis);
        

        foreach (RaycastHit2D hit in hits) {   
            //Debug.Log(hit.collider.gameObject.name);
            GameObject other = hit.collider.gameObject;
            if (other != knight){
                if (other.CompareTag("character")){
                    Destroy(gameObject);
                    //gameLogic.SpawnKnight(other);
                    GameEvents.knightHit.Invoke( new HitEventData(knight, other, gameObject));
                    //Debug.Log(other.name);
                    break;
                }
                
                if (other.CompareTag("wall")){
                    Destroy(gameObject);
                    break;
                }
            }
        }

        transform.position =  newPosition;
    }
}