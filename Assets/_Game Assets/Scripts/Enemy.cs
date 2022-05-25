using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Enemy : MonoBehaviour
{
    Rigidbody2D m_rb2D;
    Animator m_animator;

    [SerializeField] Attack_Enemy m_clawEffect;
    [SerializeField] float m_attackDelay = 0.2f;
    Coroutine m_attackCoroutine;

    [SerializeField] float m_movespeed = 2.0f;

    public Player playerTarget { set; private get; }

    public bool isDead { get; private set; } = false;

    public void AttackDirection(int dir)
    {
        if (playerTarget && !playerTarget.isDead && m_attackCoroutine == null)
        {
            m_attackCoroutine = StartCoroutine(AttackCoroutine(dir));

            if(m_animator)
            {
                m_animator.SetFloat("attackDelay", 1 / m_attackDelay);
                m_animator.SetInteger("direction", dir);
                m_animator.SetTrigger("attack");
            }
        }
    }

    public void Dead()
    {
        m_rb2D.isKinematic = true;
        isDead = true;
        Destroy(gameObject, 0.5f);

        if(m_animator)
            m_animator.SetTrigger("dead");
    }

    private void Awake()
    {
        m_rb2D = GetComponent<Rigidbody2D>();
        m_animator = GetComponent<Animator>();
    }

    private void Update()
    {
        if(!isDead)
        {
            if(Vector2.Distance(transform.position, playerTarget.transform.position) > 1.1f)
            {
                m_rb2D.MovePosition(Vector2.MoveTowards(transform.position, playerTarget.transform.position, m_movespeed * Time.deltaTime));
            } else
            {
                int dir = _ConvertToDirection(playerTarget.transform.position - transform.position);
                AttackDirection(dir);
            }
        }
    }

    private IEnumerator AttackCoroutine(int dir)
    {
        m_clawEffect.transform.position = (Vector2)transform.position + _ConvertDirToVector2(dir);
        m_clawEffect.gameObject.SetActive(true);

        var clawAnimator = m_clawEffect.GetComponent<Animator>();
        if(clawAnimator) clawAnimator.SetFloat("attackDelay", 1 / m_attackDelay);

        yield return new WaitForSeconds(m_attackDelay);

        m_clawEffect.gameObject.SetActive(false);
        m_attackCoroutine = null;
    }

    private int _ConvertToDirection(Vector2 vector)
    {
        if (vector.x == 0.0f && vector.y > 0.0f)
            return 0;

        if (vector.x > 0.0f && vector.y == 0.0f)
            return 1;

        if (vector.x == 0.0f && vector.y < 0.0f)
            return 2;

        if (vector.x < 0.0f && vector.y == 0.0f)
            return 3;

        return -1;
    }

    private Vector2 _ConvertDirToVector2(int dir)
    {
        if (dir == 0) return Vector2.up;
        if (dir == 1) return Vector2.right;
        if (dir == 2) return Vector2.down;
        if (dir == 3) return Vector2.left;

        return Vector2.zero;
    }
}
