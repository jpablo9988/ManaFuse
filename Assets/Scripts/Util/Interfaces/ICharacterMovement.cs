using UnityEngine;

public interface ICharacterMovement
{
    public void Move(Vector2 m_Input);
    public void Rotate(Vector2 m_Input);
    public void InitiateSprint(Vector2 m_Input);

}
