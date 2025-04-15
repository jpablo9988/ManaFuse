using UnityEngine;

[RequireComponent(typeof(Animator))]
public class CharacterAnimationManager : MonoBehaviour
{
    private Animator _animManager;
    [SerializeField]
    private string SpeedParameterName = "Speed";
    [SerializeField]
    private string AngleParameterName = "Angle";
    [SerializeField]
    private string ShootParameterName = "Shoot";
    void Awake()
    {
        _animManager = GetComponent<Animator>();
    }
    public void SetAnimationParameters(float speed, float directionAngle)
    {
        _animManager.SetFloat(AngleParameterName, directionAngle);
        _animManager.SetFloat(SpeedParameterName, speed);
    }
    public void Shoot()
    {
        _animManager.SetTrigger(ShootParameterName);
    }
    public void AnimationSpeed(float speed)
    {
        _animManager.speed = speed;
    }
}
