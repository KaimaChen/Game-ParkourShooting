using UnityEngine;

public class Teleporter : MonoBehaviour
{
    public Transform m_target;

    public void Teleport(PlayerController player)
    {
        player.transform.position = m_target.position;
        player.m_targetTrack = Mathf.RoundToInt(m_target.position.x / GameManager.Instance.m_trackWidth);
    }
}
