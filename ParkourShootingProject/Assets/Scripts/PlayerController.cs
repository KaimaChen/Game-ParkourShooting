using UnityEngine;

public class PlayerController : MonoBehaviour
{
    public float m_forwardSpeed = 5f;
    public float m_sideSpeed = 10f;

    public Bullet m_bullet;
    public Transform m_spawnPos;
    public float m_atkCd = 0.1f;
    private float m_atkValidTime;
    public float m_bulletAngle = 15;
    public ShootType m_shootType = ShootType.Single;

    public int m_targetTrack = 2;

    void Update()
    {
        Vector3 movement = UpdateMovement();
        if (movement.x == 0)
            UpdateShoot();
    }

    Vector3 UpdateMovement()
    {
        float targetX = m_targetTrack * GameManager.Instance.m_trackWidth;
        if (Mathf.Approximately(transform.position.x, targetX))
        {
            if (Input.GetKeyDown(KeyCode.A) && m_targetTrack > 0)
                m_targetTrack--;
            else if (Input.GetKeyDown(KeyCode.D) && m_targetTrack < GameManager.Instance.m_trackCount - 1)
                m_targetTrack++;
        }

        targetX = m_targetTrack * GameManager.Instance.m_trackWidth;
        float deltaX = targetX - transform.position.x;
        float movementX = 0;
        if (deltaX > 0)
            movementX = Mathf.Clamp(m_sideSpeed * Time.deltaTime, 0, deltaX);
        else if (deltaX < 0)
            movementX = Mathf.Clamp(-m_sideSpeed * Time.deltaTime, deltaX, 0);

        Vector3 movement = new Vector3(movementX, 0, m_forwardSpeed * Time.deltaTime);
        transform.Translate(movement);

        return movement;
    }

    void UpdateShoot()
    {
        if (Time.time < m_atkValidTime)
            return;

        m_atkValidTime = Time.time + m_atkCd;

        if(m_shootType == ShootType.Five)
        {
            CreateBullet(0);
            CreateBullet(-m_bulletAngle);
            CreateBullet(m_bulletAngle);
            CreateBullet(-m_bulletAngle * 2);
            CreateBullet(m_bulletAngle * 2);
        }
        else if(m_shootType == ShootType.Three)
        {
            CreateBullet(0);
            CreateBullet(-m_bulletAngle);
            CreateBullet(m_bulletAngle);
        }
        else
        {
            CreateBullet(0);
        }
    }

    void CreateBullet(float angle)
    {
        Bullet bullet = Bullet.GetFromPool(m_bullet);
        bullet.transform.position = m_spawnPos.position;
        bullet.transform.rotation = Quaternion.Euler(new Vector3(0, angle, 0));
    }

    private void OnCollisionEnter(Collision collision)
    {
        var go = collision.gameObject;
        if(go.layer == LayerMask.NameToLayer("Obstacle"))
        {
            GameManager.Instance.GameOver(this);
        }
        else if(go.layer == LayerMask.NameToLayer("Bonus"))
        {
            Bonus bonus = go.GetComponentInParent<Bonus>();
            switch (bonus.m_type)
            {
                case BonusType.ThreeFire:
                    m_shootType = ShootType.Three;
                    break;
                case BonusType.FiveFire:
                    m_shootType = ShootType.Five;
                    break;
                case BonusType.AtkSpeedUp:
                    m_atkCd *= bonus.m_atkCdPercent;
                    break;
                default:
                    Debug.LogError("未处理的奖励类型: " + bonus.m_type);
                    break;
            }

            Destroy(bonus.gameObject);
        }
    }

    private void OnTriggerEnter(Collider other)
    {
        var go = other.gameObject;
        if(go.layer == LayerMask.NameToLayer("Goal"))
        {
            GameManager.Instance.GameOver(this);
        }
        else if(go.layer == LayerMask.NameToLayer("Teleporter"))
        {
            var teleporter = go.GetComponentInParent<Teleporter>();
            teleporter.Teleport(this);
        }
    }
}

public enum ShootType
{
    Single,
    Three,
    Five,
}