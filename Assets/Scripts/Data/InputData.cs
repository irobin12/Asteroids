using UnityEngine;
using UnityEngine.Assertions;

[CreateAssetMenu(fileName = "InputData", menuName = "Data/InputData", order = 1)]
public class InputData : ScriptableObject
{
    [SerializeField] private KeyCode[] moveForwardKeys;
    public KeyCode[] MoveForwardKeys
    {
        get
        {
            VerifyKeyCodes(moveForwardKeys);
            return moveForwardKeys;
        }
    }
    
    [SerializeField] private KeyCode[] moveLeftKeys;
    public KeyCode[] MoveLeftKeys
    {
        get
        {
            VerifyKeyCodes(moveLeftKeys);
            return moveLeftKeys;
        }
    }
    
    [SerializeField] private KeyCode[] moveRightKeys;
    public KeyCode[] MoveRightKeys
    {
        get
        {
            VerifyKeyCodes(moveRightKeys);
            return moveRightKeys;
        }
    }
    
    [SerializeField] private KeyCode[] shootKeys;
    public KeyCode[] ShootKeys
    {
        get
        {
            VerifyKeyCodes(shootKeys);
            return shootKeys;
        }
    }
    
    [SerializeField] private KeyCode[] teleportationKeys;
    public KeyCode[] TeleportationKeys
    {
        get
        {
            VerifyKeyCodes(teleportationKeys);
            return teleportationKeys;
        }
    }
    
    [SerializeField] private KeyCode[] restartKeys;
    public KeyCode[] RestartKeys
    {
        get
        {
            VerifyKeyCodes(restartKeys);
            return restartKeys;
        }
    }

    [Header("Accessibility")] 
    [Tooltip("Can the projectile fire key be held down to shoot continuously?")]
    [SerializeField] private bool continuousFire;
    public bool ContinuousFire => continuousFire;

    [Tooltip("Can the projectile firing be locked to avoid having to hold the key down? " +
             "(This forces continuous fire on.)")]
    [SerializeField] private bool lockFire;
    public bool LockFire => lockFire;
    
    private static void VerifyKeyCodes(KeyCode[] keyCodes)
    {
        Assert.IsTrue(keyCodes.Length > 0, "All key codes fields in the Input Data must have at least one entry assigned.");
    }
}