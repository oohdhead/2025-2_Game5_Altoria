using GameInteract;
public class InputBinder
{
    private PlayerInputHandler handler;
    private IMoveInput currentReceiver;

    public InputBinder(PlayerInputHandler input) { handler = input; }
    public void Initialize(IMoveInput  receiver)
    {
        Bind(receiver);
        if (receiver is IInteractInput interact) handler.OnInteract += interact.TryInteract;
        if (receiver is IRidingInput riding) handler.OnRiding += riding.TryRiding;
    }
    public void Bind(IMoveInput receiver)
    {
        Unbind();
        currentReceiver = receiver;
        if (receiver == null)
        {
            UnityEngine.Debug.Log("RECEIVER IS NONE");
        }

        BindInput(receiver);
    }
    void BindInput(IMoveInput receiver)
    {
        handler.OnMove += receiver.OnMoveInput;
        handler.OnMoveCanceled += receiver.OnMoveCancel;

        if(receiver is IJumper jumper) handler.OnJump += jumper.OnJumpInput;
        if (currentReceiver is IRunner runner)
        {
            handler.OnRun += runner.OnRunInput;
            handler.OnRunCancel += runner.OnRunCancel;
        }
    }
    public void Unbind()
    {
        if (currentReceiver == null) return;

        handler.OnMove -= currentReceiver.OnMoveInput;
        handler.OnMoveCanceled -= currentReceiver.OnMoveCancel;
        if (currentReceiver is IJumper jumper) handler.OnJump -= jumper.OnJumpInput;

        if (currentReceiver is IRunner runner)
        {
            handler.OnRun -= runner.OnRunInput;
            handler.OnRunCancel -= runner.OnRunCancel;
        }
        currentReceiver = null;
    }
}
