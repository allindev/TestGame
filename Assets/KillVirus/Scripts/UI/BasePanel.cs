using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using UnityEngine;

namespace UI
{
    public abstract class BasePanel : MonoBehaviour
    {
        public abstract void Active();

        public abstract void UnActive();
    }

}
