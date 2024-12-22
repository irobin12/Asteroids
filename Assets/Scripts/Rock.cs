public class Rock: MovingEntity
{
    public void SetUp(bool forward)
    {
        SetMovement(forward, false, false);
    }
    
    public override void Reset()
    {
    }
}