using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.Events;

public static class GameEvents
{
    public static HitEvent  knightHit = new HitEvent();
}

//public class HitEvent : UnityEvent<int> {}
public class HitEvent : UnityEvent<HitEventData> {}

public class HitEventData{
    public GameObject shooter;
    public GameObject victim;
    public GameObject slash;

    //constructor
    public HitEventData(GameObject shooter, GameObject victim, GameObject slash){
        this.shooter = shooter;
        this.victim = victim;
        this.slash = slash;
    }
}