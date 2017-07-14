using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public abstract class HandleBase  {

    public abstract Operation OpCode { get;}
	public HandleBase()
    {
        ClientEngine.Instance.handles.Add(OpCode, this);
    }

    public abstract void Handle(Parameter Parma);

    public virtual void Dispose()
    {

    }
}
