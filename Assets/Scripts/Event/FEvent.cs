
public class FEvent : FEventRegister
{
    public void BroadCastEvent()
    {
        _BroadCastEvent();
    }
}

public class FEvent<T0> : FEventRegister<T0>
{
    public void BroadCastEvent(T0 arg)
    {
        _BroadCastEvent(arg);
    }
}

public class FEvent<T0, T1> : FEventRegister<T0, T1>
{
    public void BroadCastEvent(T0 arg, T1 arg1)
    {
        _BroadCastEvent(arg, arg1);
    }
}

//....
