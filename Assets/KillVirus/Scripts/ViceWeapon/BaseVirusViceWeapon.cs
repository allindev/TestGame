using UnityEngine;

namespace ViceWeapon
{
    public abstract class BaseVirusViceWeapon : MonoBehaviour
    {

        public bool IsUpdate { set; get; }

        public abstract void Initi();

        public abstract void ReIniti();

        public virtual void OnUpdate()
        {
            if (!IsUpdate)
                return;
        }
    }
}
