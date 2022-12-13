using System;
using System.Collections.Generic;
using UnityEngine;

public class GameFlowHolder
{
    private int current = -1;

    protected List<IFlow> Flows = null;

    protected IFlow holding;

    public event Action StartPlayEvent;
    public event Action StopPlayEvent;

    public GameFlowHolder()
    {
        Flows = new List<IFlow>();
    }

    public virtual void Update()
    {
        if (holding != null) holding.Update(this);
    }

    public virtual List<IFlow> Add(IFlow flow)
    {
        Flows.Add(flow);
        return Flows;
    }

    public virtual void ChangeState(IFlow enterState)
    {
        if (holding != null) holding.Exit(this);
        enterState.Enter(this);
        holding = enterState;
    }

    public virtual void PlayNext()
    {
        if (Flows == null) return;

        //­Y©|¥¼¶}©l
        if (holding == null)
        {
            current = 0;
            StartPlayEvent?.Invoke();
            Log("Play Start.");
            ChangeState(Flows[current]);
        }
        else if (current != Flows.Count - 1)
        {
            current++;
            ChangeState(Flows[current]);
        }
        else
        {
            current = -1;
            holding.Exit(this);
            holding = null;
            Log("Play End.");
        }
    }

    public virtual void StopPlay()
    {
        if (holding != null)
        {
            StopPlayEvent?.Invoke();
            holding.Exit(this);
            holding = null;
        }
    }

    private void Log(string str)
    {
#if UNITY_EDITOR
        Debug.Log(str);
#endif
    }
}
