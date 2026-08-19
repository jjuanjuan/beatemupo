public abstract class CharacterState
{
    protected CharacterContext context;
    protected CharacterStateMachine stateMachine;

    protected CharacterState(
        CharacterContext context,
        CharacterStateMachine stateMachine)
    {
        this.context = context;
        this.stateMachine = stateMachine;
    }

    public virtual void Enter() { }

    public virtual void Exit() { }

    public virtual void Update() { }

    public virtual void FixedUpdate() { }
}