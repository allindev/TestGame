using Events;
using Tool;
using UnityEngine;

public class LimitMoveProp : VirusBaseProp
{

    [SerializeField] private float _duration;
    public override void Excute(Transform target)
    {
        EventManager.TriggerEvent(new VirusPropAddEvent(_duration, VirusPropEnum.LimitMove));
        PropPools.Instance.DeSpawn(gameObject);
    }

}