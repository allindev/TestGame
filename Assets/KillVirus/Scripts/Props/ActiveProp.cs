using Events;
using Tool;
using UnityEngine;

public class ActiveProp : VirusBaseProp
{

    [SerializeField] private float _duration;

    public override void Excute(Transform target)
    {
        EventManager.TriggerEvent(new VirusPropAddEvent(_duration, VirusPropEnum.Active));
        PropPools.Instance.DeSpawn(gameObject);
    }
}