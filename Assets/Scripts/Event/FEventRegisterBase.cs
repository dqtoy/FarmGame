using System;

public interface IRelease
{
    void Release();
}

public abstract class FEventRegisterBase
{
    protected Delegate _delegate;

    /// <summary>
    /// 添加监听
    /// </summary>
    /// <param name="d"></param>
    /// <returns></returns>
    protected void _AddEventHandler(Delegate d)
    {
        _delegate = Delegate.Combine(_delegate, d);
    }

    /// <summary>
    /// 移除监听
    /// </summary>
    /// <param name="d"></param>
    /// <returns></returns>
    protected void _RemoveEventHandler(Delegate d)
    {
        _delegate = Delegate.RemoveAll(_delegate, d);
    }

    /// <summary>
    /// 订阅监听,订阅的监听不用remove,需要执行返回接口的Release。该功能可以方便统一管理，实现自动移除监听
    /// </summary>
    /// <param name="cb"></param>
    /// <returns></returns>
    protected IRelease _Subscribe(Delegate d)
    {
        _AddEventHandler(d);
        return new HandlerRemove(this, d);
    }

    class HandlerRemove : IRelease
    {
        FEventRegisterBase _source;
        Delegate _value;

        public HandlerRemove(FEventRegisterBase source, Delegate value)
        {
            _source = source;
            _value = value;
        }
        void IRelease.Release()
        {
            _source._RemoveEventHandler(_value);
        }
    }
}

public abstract class FEventRegister : FEventRegisterBase
{
    public void AddEventHandler(Action cb)
    {
        _AddEventHandler(cb);
    }
    public void RemoveEventHandler(Action cb)
    {
        _RemoveEventHandler(cb);
    }

    //定阅事件
    public IRelease Subscribe(Action cb)
    {
        return _Subscribe(cb);
    }

    protected void _BroadCastEvent()
    {
        (_delegate as Action)?.Invoke();
    }
}

public abstract class FEventRegister<T0> : FEventRegisterBase
{
    public void AddEventHandler(Action<T0> cb)
    {
        _AddEventHandler(cb);
    }
    public void RemoveEventHandler(Action<T0> cb)
    {
        _RemoveEventHandler(cb);
    }

    //定阅事件
    public IRelease Subscribe(Action<T0> cb)
    {
        return _Subscribe(cb);
    }

    protected void _BroadCastEvent(T0 arg)
    {
        (_delegate as Action<T0>)?.Invoke(arg);
    }
}

public abstract class FEventRegister<T0, T1> : FEventRegisterBase
{
    public void AddEventHandler(Action<T0, T1> cb)
    {
        _AddEventHandler(cb);
    }
    public void RemoveEventHandler(Action<T0, T1> cb)
    {
        _RemoveEventHandler(cb);
    }

    //定阅事件
    public IRelease Subscribe(Action<T0, T1> cb)
    {
        return _Subscribe(cb);
    }

    protected void _BroadCastEvent(T0 arg, T1 arg1)
    {
        (_delegate as Action<T0, T1>)?.Invoke(arg, arg1);
    }
}

// ....