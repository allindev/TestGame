using System;
using System.Collections.Generic;

public class RectiveProperty<T>
{

    public List<Action<T>> ValueChangeAction { set; get; }

    private T _value;
    public T Value
    {
        get { return _value; }
        set
        {
            _value = value;
            if (ValueChangeAction != null)
            {
                for (int i = 0; i < ValueChangeAction.Count; i++)
                {
                    ValueChangeAction[i].Invoke(_value);
                }
            }
        }
    }


    public RectiveProperty(T initValue)
    {
        _value = initValue;
        ValueChangeAction = new List<Action<T>>();
    }

    public RectiveProperty()
    {
        _value = default(T);
        ValueChangeAction = new List<Action<T>>();
    }

    public void Subscibe(Action<T> obsever)
    {
        if (!ValueChangeAction.Contains(obsever))
            ValueChangeAction.Add(obsever);
    }


}
