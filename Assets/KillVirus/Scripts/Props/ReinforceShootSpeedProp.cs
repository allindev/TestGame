using Events;
using Tool;
using UnityEngine;

public class ReinforceShootSpeedProp : VirusBaseProp
{

    [SerializeField] private float _duration;

    public override void Excute(Transform target)
    {
        EventManager.TriggerEvent(new VirusPropAddEvent(_duration, VirusPropEnum.ReinforceShootSpeed));
        PropPools.Instance.DeSpawn(gameObject);
    }
}