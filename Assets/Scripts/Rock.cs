public class Rock: MovingEntity
{
    public void Initialize(bool forward)
    {
        SetMovement(forward, false, false);
    }
    
    public override void Reset()
    {
    }
}