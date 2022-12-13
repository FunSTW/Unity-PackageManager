
public abstract class IFlow
{
    public override string ToString()
    {
        return $"{GetType().Name}";
    }

    public delegate void FlowEvent();

    public FlowEvent EnterEvent = null;
    public FlowEvent ExitEvent = null;
    public FlowEvent UpdateEvent = null;

    public virtual void Enter(GameFlowHolder holder) { EnterEvent?.Invoke(); }
    public virtual void Exit(GameFlowHolder holder) { ExitEvent?.Invoke(); }
    public virtual void Update(GameFlowHolder holder) { UpdateEvent?.Invoke(); } 
}