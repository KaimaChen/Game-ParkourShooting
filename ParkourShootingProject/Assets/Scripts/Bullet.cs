using UnityEngine;
using System.Collections.Generic;

[RequireComponent(typeof(Rigidbody))]
public class Bullet : MonoBehaviour
{
    private static readonly List<Bullet> m_pool = new List<Bullet>();

    public float m_speed = 10f;
    public int m_damage = 1;
    public float m_lifeTime = 5f;
    
    private float m_endTime;

    private Rigidbody m_rigid;

    private void Awake()
    {
        m_rigid = GetComponent<Rigidbody>();
    }

    private void OnDestroy()
    {
        //销毁场景才会销毁子弹，这时就清空pool
        m_pool.Clear();
    }

    private void Update()
    {
        if (Time.time >= m_endTime)
        {
            ReturnToPool();
            return;
        }

        var velocity = transform.forward;
        velocity.z *= m_speed;
        m_rigid.velocity = transform.forward * m_speed;
    }

    private void OnCollisionEnter(Collision collision)
    {
        var go = collision.gameObject;
        if(go.layer == LayerMask.NameToLayer("Obstacle"))
        {
            var pile = go.GetComponentInParent<Obstacle>();
            pile.TakeDamage(m_damage);
            ReturnToPool();
        }
    }

    private void Init()
    {
        gameObject.SetActive(true);
        m_endTime = Time.time + m_lifeTime;

        
    }

    public void ReturnToPool()
    {
        gameObject.SetActive(false);
        m_pool.Add(this);
    }

    public static Bullet GetFromPool(Bullet prefab)
    {
        if(m_pool.Count > 0)
        {
            var bullet = m_pool[m_pool.Count - 1];
            m_pool.RemoveAt(m_pool.Count - 1);
            bullet.Init();
            return bullet;
        }
        else
        {
            GameObject go = Instantiate(prefab.gameObject);
            go.transform.SetParent(prefab.transform.parent);
            var bullet = go.GetComponent<Bullet>();
            bullet.Init();
            return bullet;
        }
    }
}