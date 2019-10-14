using Events;
using Tool;
using UnityEngine;

public class CallFriendProp : VirusBaseProp
{

    [SerializeField] private float _duration;

    public override void Excute(Transform target)
    {
        EventManager.TriggerEvent(new VirusPropAddEvent(_duration, VirusPropEnum.CallFriend));
        PropPools.Instance.DeSpawn(gameObject);
    }

}