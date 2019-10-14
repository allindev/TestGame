using Events;
using Tool;
using UnityEngine;

public class WeakenProp : VirusBaseProp
{

    [SerializeField] private float _duration;

    public override void Excute(Transform target)
    {
        EventManager.TriggerEvent(new VirusPropAddEvent(_duration, VirusPropEnum.Weaken));
        PropPools.Instance.DeSpawn(gameObject);
    }

}