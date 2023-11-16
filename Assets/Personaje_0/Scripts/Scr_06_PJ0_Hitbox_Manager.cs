using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Scr_06_PJ0_Hitbox_Manager : MonoBehaviour
{
    public float PurebaHitstunDebil;

    //Numero de Hitbox
    public Scr_07_PJ0_Hitbox hitBox01;
    public Scr_07_PJ0_Hitbox hitBox02;

    //Bools de Confirmacion
    public bool hitboxApagar;
    public bool hitboxTrueConfirm;

    //Otros
    private Scr_04_PJ0_Accion_Manager actionManager;
    private Scr_05_PJ0_Anim_Manager animationManager;

    [System.Serializable]
    public class AttackStats
    {
        public float attackDamage;
        public float attackStun;
        public float attackShake;
        public float attackKnockbackX;
        public float attackKnockbackY;
    }

    public AttackStats[] stringPunch1 = new AttackStats[0];
    public AttackStats[] stringPunch2 = new AttackStats[0];
    public AttackStats[] stringPunch3 = new AttackStats[0];
    public AttackStats[] stringPunch4 = new AttackStats[0];

    public AttackStats[] stringKick1 = new AttackStats[0];
    public AttackStats[] stringKick2 = new AttackStats[0];

    public AttackStats[] airPunch = new AttackStats[0];
    public AttackStats[] airKick = new AttackStats[0];

    public AttackStats[] uniquePunch = new AttackStats[0];
    public AttackStats[] uniqueKick = new AttackStats[0];

    private void Awake()
    {
        Transform parentTransform = transform.parent;
        actionManager = parentTransform.GetComponentInParent<Scr_04_PJ0_Accion_Manager>();
        animationManager = GetComponentInParent<Scr_05_PJ0_Anim_Manager>();
    }

    void Start()
    {
        hitBox01.HitCountChanged += HandleHitCountChanged;
        hitBox02.HitCountChanged += HandleHitCountChanged;
    }

    void HandleHitCountChanged()
    {
        animationManager.HandleHit();
    }

    private void Update()
    {
        //Si True apagan todos los Confirm y True_Confrim
        if (hitboxApagar)
        {
            hitboxTrueConfirm = false;
            hitBox01.confirm = false;
            hitBox02.confirm = false;
            hitboxApagar = false;
        }

        //True_Confirm <- True si cualquiera de las Hitbox colisiona correctamente
        if (hitBox01.confirm || hitBox02.confirm)
        {
            hitboxTrueConfirm = true;
        }

        //Valores de Ataques - String Punc
        if (actionManager.actualAction == "Ataque_Puño_1")
        {
            AtackStats(hitBox01, stringPunch1[0], "Alto");
            PurebaHitstunDebil = stringPunch1[0].attackStun;
        }
        if (actionManager.actualAction == "Ataque_Puño_2")
        {
            AtackStats(hitBox01, stringPunch2[0], "Medio");
            PurebaHitstunDebil = stringPunch2[0].attackStun;
        }
        if (actionManager.actualAction == "Ataque_Puño_3")
        {
            AtackStats(hitBox01, stringPunch3[0], "Alto");
            AtackStats(hitBox02, stringPunch3[1], "Launcher");
            PurebaHitstunDebil = stringPunch3[0].attackStun;
        }
        if (actionManager.actualAction == "Ataque_Puño_4")
        {
            AtackStats(hitBox01, stringPunch4[0], "Bajo");
            AtackStats(hitBox02, stringPunch4[0], "Bajo");
            PurebaHitstunDebil = stringPunch4[0].attackStun;
        }

        //Valores de Ataques - String Kick
        if (actionManager.actualAction == "Ataque_Patada_1")
        {
            AtackStats(hitBox01, stringKick1[0], "Alto");
            PurebaHitstunDebil = stringKick1[0].attackStun;
        }
        if (actionManager.actualAction == "Ataque_Patada_2")
        {
            AtackStats(hitBox01, stringKick2[0], "Alto");
            PurebaHitstunDebil = stringKick2[0].attackStun;
        }

        //Valores de Ataques - Ataques Aereos
        if (actionManager.actualAction == "Ataque_Puño_Aire")
        {
            AtackStats(hitBox01, airPunch[0], "Alto");
            PurebaHitstunDebil = airPunch[0].attackStun;
        }
        if (actionManager.actualAction == "Ataque_Patada_Aire")
        {
            AtackStats(hitBox01, airKick[0], "Alto");
            PurebaHitstunDebil = airPunch[0].attackStun;
        }

        //Valores de Ataques - Ataques Unicos
        if (actionManager.actualAction == "Ataque_Puño_Unico")
        {
            AtackStats(hitBox01, uniquePunch[0], "Bajo");
            PurebaHitstunDebil = uniquePunch[0].attackStun;
            AtackStats(hitBox02, uniquePunch[1], "Alto");
        }
        if (actionManager.actualAction == "Ataque_Patada_Unico")
        {
            AtackStats(hitBox01, uniqueKick[0], "Trip");
            PurebaHitstunDebil = uniqueKick[0].attackStun;
        }
    }

    void AtackStats(Scr_07_PJ0_Hitbox HB, AttackStats attack, string attackWeight)
    {
        HB.hitboxDamage = attack.attackDamage;
        HB.hitboxHitstun = attack.attackStun;
        HB.hitboxShake = attack.attackShake;
        HB.hitboxWeight = attackWeight;
        HB.hitboxKnockback = new Vector3(attack.attackKnockbackX, attack.attackKnockbackY, 0);
        HB.hitboxKnockbackHitstun = 0;
    }
}
