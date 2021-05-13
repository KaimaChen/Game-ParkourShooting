using UnityEngine;

public class Obstacle : MonoBehaviour
{
    public int m_life = 50;

    public virtual void TakeDamage(int damage)
    {
        m_life = Mathf.Clamp(m_life - damage, 0, m_life);
        if (m_life <= 0)
            Destroy(gameObject);
    }
}
