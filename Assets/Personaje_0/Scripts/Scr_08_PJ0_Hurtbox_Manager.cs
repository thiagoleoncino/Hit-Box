using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Scr_08_PJ0_Hurtbox_Manager: MonoBehaviour
{
    public Scr_09_PJ0_Hurtbox hurtBox01;

    public bool hurtboxApagar;
    public bool hurtboxHit;

    public float hurtobxManagerDamage;
    public float hurtobxManagerHitstun;
    public float hurtobxManagerShake;
    public string hurtobxManagerAnim;
    public Vector3 hurtobxManagerKnockback;
    public float hurtobxManagerKnockbackHitstun;

    private Scr_04_PJ0_Accion_Manager actionManager;
    private void Awake()
    {
        Transform parentTransform = transform.parent;
        actionManager = parentTransform.GetComponentInParent<Scr_04_PJ0_Accion_Manager>();
    }

    void Start()
    {
        hurtBox01.HitCountChanged += HandleHitCountChanged;
    }

    void HandleHitCountChanged()
    {
        actionManager.ActionReiniciar();
    }

    private void Update()
    {
        if (hurtboxApagar)
        {
            hurtBox01.Hit_Confirm = false;
            hurtboxApagar = false;
        }
        if (hurtBox01.Hit_Confirm)
        {
            hurtboxHit = true;
            hurtboxApagar = true;
        }
    }
}