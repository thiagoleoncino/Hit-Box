using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Scr_09_PJ0_Hurtbox : MonoBehaviour
{
    public bool Hit_Confirm;
    public bool collisionOccurred = false;
    private Scr_01_Control_Manager inputManager;
    private Scr_08_PJ0_Hurtbox_Manager hurtboxManager;

    [SerializeField]
    private int _hitCount;
    public int hitCount
    {
        get { return _hitCount; }
        set
        {
            if (value != _hitCount)
            {
                _hitCount = value;
                OnHitCountChanged();
            }
        }
    }

    public event System.Action HitCountChanged;
    private void OnHitCountChanged()
    {
        if (HitCountChanged != null)
        {
            HitCountChanged.Invoke();
        }
    }

    private void Awake()
    {
        Transform parentTransform = transform.parent;
        inputManager = parentTransform.GetComponentInParent<Scr_01_Control_Manager>();
        hurtboxManager = GetComponentInParent<Scr_08_PJ0_Hurtbox_Manager>();
    }

    private void OnTriggerEnter(Collider other)
    {
        if (inputManager.player1)
        {
            if (!collisionOccurred && other.gameObject.tag == "Hitbox_Player_2")
            {
                Hit_Confirm = true;
                hitCount++;
                Debug.Log("Hit P2");
                collisionOccurred = false;
            }
        }

        if (inputManager.player2)
        {
            if (!collisionOccurred && other.gameObject.tag == "Hitbox_Player_1")
            {
                Hit_Confirm = true;
                hitCount++;
                Debug.Log("Hit P1");
                collisionOccurred = false;
            }
        }
    }

    private void OnTriggerStay(Collider other)
    {
        if (other.gameObject.tag == "Hitbox_Player_1")
        {
            collisionOccurred = false;
        }

        if (other.gameObject.tag == "Hitbox_Player_2")
        {
            collisionOccurred = false;
        }
    }

    private void OnTriggerExit(Collider other)
    {
        if (other.gameObject.tag == "Hitbox_Player_1")
        {
            collisionOccurred = true;
        }

        if (other.gameObject.tag == "Hitbox_Player_2")
        {
            collisionOccurred = true;
        }
    }

    public void Hit_Data(float HB_Daño, float HB_Hitstun, float HB_Shake, string HB_Peso, Vector3 HB_Knockback, float HB_Knockback_Hitstun)
    {
        hurtboxManager.hurtobxManagerDamage = HB_Daño;
        hurtboxManager.hurtobxManagerHitstun = HB_Hitstun;
        hurtboxManager.hurtobxManagerShake = HB_Shake;
        hurtboxManager.hurtobxManagerAnim = HB_Peso;
        hurtboxManager.hurtobxManagerKnockback = HB_Knockback;
        hurtboxManager.hurtobxManagerKnockbackHitstun = HB_Knockback_Hitstun;
    }
}

