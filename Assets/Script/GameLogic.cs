using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class GameLogic : MonoBehaviour
{
    public GameState gameState;
    public GameObject[] spawnPoints;
    public GameObject[] knights;

    void Awake() {
        GameEvents.knightHit.AddListener(KnightHit);
        GameEvents.knightHit.AddListener(SpawnKnight);
    }

    void  Start() {
        gameState.gameTime = 90f;
    }
    void Update() {
        gameState.gameTime -= Time.deltaTime;
    }

    void KnightHit(HitEventData data){
        //Debug.Log(data.victim);
        Knight victimScript =data.victim.GetComponent<Knight>();
        victimScript.health -= 10;
    }

    public void SpawnKnight(HitEventData data){
        GameObject spawnPoint = GetRandomSpawnPoint();
        data.victim.transform.position = spawnPoint.transform.position;
    }


    GameObject GetRandomSpawnPoint(){
        return spawnPoints[Random.Range(0, spawnPoints.Length)];
    }



     GameObject GetFurthestSpawnPoint(){
        float[] minSqrDistances = new float[spawnPoints.Length];
        for (int i=0; i<spawnPoints.Length; i++){
            float min = Mathf.Infinity;
            for (int j=0; j<knights.Length; j++){
                float sqrDistance = (knights[j].transform.position - spawnPoints[i].transform.position).sqrMagnitude;
                if (sqrDistance < min){
                    min = sqrDistance;
                }
                minSqrDistances[i] = min;
            }
            
        }
        float max = minSqrDistances[0];
        int maxIndex = 0;
        for (int i = 1; i < minSqrDistances.Length; i++){
            if (max<minSqrDistances[i]){
                max = minSqrDistances[i];
                maxIndex = i;
            }
        }
        return spawnPoints[maxIndex];
    }
}
