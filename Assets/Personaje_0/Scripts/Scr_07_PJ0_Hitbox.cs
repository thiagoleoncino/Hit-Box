using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Scr_07_PJ0_Hitbox : MonoBehaviour
{
    [Header("Cancel")]
    public bool confirm;

    [Header("Caracteristicas Ataque")]
    public float hitboxDamage;
    public float hitboxHitstun;
    public float hitboxShake;
    public string hitboxWeight;
    public Vector3 hitboxKnockback;
    public float hitboxKnockbackHitstun;

    private int _hitCount; 
    public int hitCount
    {
        get { return _hitCount; }
        set
        {
            if (value != _hitCount)
            {
                _hitCount = value;
                OnHitCountChanged(); // Llama al evento cuando el valor cambia
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

    private void OnTriggerEnter(Collider other)
    {
        if(other.gameObject.tag == "Tag_Player_2")
        {
            confirm = true;
            hitCount++;
            Scr_09_PJ0_Hurtbox Hurtbox = other.GetComponent<Scr_09_PJ0_Hurtbox>();
            if (Hurtbox != null)
            {
                Hurtbox.Hit_Data(hitboxDamage, hitboxHitstun, hitboxShake, hitboxWeight, hitboxKnockback, hitboxKnockbackHitstun);
            }
        }

        if (other.gameObject.tag == "Tag_Player_1")
        {
            confirm = true;
            hitCount++;
            Scr_09_PJ0_Hurtbox Hurtbox = other.GetComponent<Scr_09_PJ0_Hurtbox>();
            if (Hurtbox != null)
            {
                Hurtbox.Hit_Data(hitboxDamage, hitboxHitstun, hitboxShake, hitboxWeight, hitboxKnockback, hitboxKnockbackHitstun);
            }
        }
    }
}
