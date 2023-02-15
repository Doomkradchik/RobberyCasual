public class TrashCan : Interactor<MainCharacterController>
{
    protected override void OnTriggered()
    {
        _entity.OnDrop();
    }
}
