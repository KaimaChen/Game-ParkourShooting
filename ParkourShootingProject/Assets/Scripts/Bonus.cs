using UnityEngine;

public class Bonus : MonoBehaviour
{
    public BonusType m_type;
    public float m_atkCdPercent = 0.8f;
}

public enum BonusType
{
    ThreeFire,
    FiveFire,
    AtkSpeedUp,
}