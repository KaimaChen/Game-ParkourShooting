using UnityEngine;

public class Pile : Obstacle
{
    public TextMesh m_numText;
    public Transform m_bonus;

    void Update()
    {
        m_numText.text = m_life.ToString();
    }

    public override void TakeDamage(int damage)
    {
        m_life = Mathf.Clamp(m_life - damage, 0, m_life);
        if (m_life <= 0)
        {
            Destroy(gameObject);
            var pos = m_bonus.position;
            pos.y = 0;
            m_bonus.position = pos;
        }
    }
}
