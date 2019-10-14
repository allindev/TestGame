using UnityEngine;

public class ShootRepulseProp : VirusBaseProp
{
    public override void Excute(Transform target)
    {
        PropPools.Instance.DeSpawn(gameObject);
    }
}