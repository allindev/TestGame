using System;

namespace KLWStateMachine
{
    [AttributeUsage(AttributeTargets.Method)]
    public class FSMAttribute : Attribute
    {
        public readonly FSMActionName ActionName;
        public readonly string EnumValue;

        /// <summary>
        /// 
        /// </summary>
        /// <param name="enumValue">EnumValue toString</param>
        /// <param name="actionName">FSMActionType Enter Exit Update Finally</param>
        public FSMAttribute(string enumValue, FSMActionName actionName)
        {
            EnumValue = enumValue;
            ActionName = actionName;
        }
    }



    public enum FSMActionName
    {
        Enter,
        Exit,
        Finally,
        Update
    }
}