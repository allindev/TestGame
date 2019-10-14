using System;
using Events;
using Tool;
using UnityEngine;

public class BigProp:VirusBaseProp
{

    [SerializeField] private float _duration;

    public override void Excute(Transform target)
    {
        EventManager.TriggerEvent(new VirusPropAddEvent(_duration, VirusPropEnum.Big));
        PropPools.Instance.DeSpawn(gameObject);
    }
  

}