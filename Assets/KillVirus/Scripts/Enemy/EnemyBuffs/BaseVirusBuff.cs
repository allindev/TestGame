using UnityEngine;

namespace EnemyBuffs
{
    public abstract class BaseVirusBuff
    {

        protected Transform Target;

        public VirusPropEnum VirusPropEnum { set; get; }

        protected BaseVirusBuff(Transform target)
        {
            Target = target;
        }

        public abstract void OnUpdate();

        public abstract void Stop();

    }
}
