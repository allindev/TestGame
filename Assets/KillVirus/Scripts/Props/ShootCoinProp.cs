using Events;
using Tool;
using UnityEngine;

public class ShootCoinProp : VirusBaseProp
{

    [SerializeField] private float _duration;
    public override void Excute(Transform target)
    {
        EventManager.TriggerEvent(new VirusPropAddEvent(_duration, VirusPropEnum.ShootCoin));
        PropPools.Instance.DeSpawn(gameObject);
    }

}