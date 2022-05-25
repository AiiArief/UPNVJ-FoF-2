using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Player : MonoBehaviour
{
    Animator m_animator;

    public bool isDead { get; private set; } = false;

    [SerializeField] Attack_Player m_slashEffect;
    [SerializeField] float m_attackDelay = 0.2f;
    Coroutine m_attackCoroutine;

    [SerializeField] RectTransform m_gameOverUI;

    public void AttackDirection(int dir)
    {
        if (isDead)
        {
            print("Player sudah mati");
            return;
        }

        if (m_attackCoroutine != null)
            return;

        print("Menyerang ke arah : " + dir);
        m_attackCoroutine = StartCoroutine(AttackCoroutine(dir));

        if(m_animator)
        {
            m_animator.SetFloat("attackDelay", 1 / m_attackDelay);
            m_animator.SetInteger("direction", dir);
            m_animator.SetTrigger("attack");
        }
    }

    public void Dead()
    {
        print("Player mati dibunuh kodok");
        isDead = true;

        if(m_gameOverUI)
            m_gameOverUI.gameObject.SetActive(true);

        if(m_animator)
            m_animator.SetTrigger("dead");
    }

    private void Awake()
    {
        m_animator = GetComponent<Animator>();
    }

    private IEnumerator AttackCoroutine(int dir)
    {
        m_slashEffect.transform.position = _ConvertDirToVector2(dir);
        m_slashEffect.gameObject.SetActive(true);

        var slashAnimator = m_slashEffect.GetComponent<Animator>();
        if(slashAnimator) slashAnimator.SetFloat("attackDelay", 1 / m_attackDelay);

        yield return new WaitForSeconds(m_attackDelay);

        m_slashEffect.gameObject.SetActive(false);
        m_attackCoroutine = null;
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
