using UnityEngine;

public class Scr_04_PJ0_Accion_Manager : MonoBehaviour
{
    //Scripts 1
    private Scr_01_Control_Manager inputManager;
    private Scr_02_Estado_Manager stateManager;
    private Scr_03_Estadisticas statsManager;
    private Scr_10_Fisicas_Manager fisicasManager;

    //Scripts 2
    private Scr_05_PJ0_Anim_Manager animationManager;
    private Scr_06_PJ0_Hitbox_Manager hitboxManager;
    private Scr_08_PJ0_Hurtbox_Manager hurtboxManager;

    //Estado Actual
    public string actualAction;

    #region Componentes de Animacion

    private string actualAnimation;

    //Anim Movimiento
    const string idleAnimation = "01_Idle";
    const string walkForwardAnimation = "03_Walk_F";
    const string walkBackwardAnimation = "04_Walk_B"; 
    const string jumpAnimation = "05_Jump";
    const string doubleJumpAnimation = "06_Double_Jump";
    const string fallAnimation = "07_Fall";

    //Anim Ataques
    const string punch1Animation = "08_01_Ataque_Puño";
    const string kick1Animation = "09_01_Ataque_Patada";

    const string airPunchAnimation = "10_Ataque_Puño_Aereo";
    const string airKickAnimation = "11_Ataque_Patada_Aereo";

    const string uniquePunchAnimation = "12_Ataque_Puño_Unico";
    const string uniqueKickAnimation = "13_Ataque_Patada_Unico";

    //Anim Strings <- Varia segun el personaje
    const string punch2Animation = "08_02_Ataque_Puño";
    const string punch3Animation = "08_03_Ataque_Puño";
    const string punch4Animation = "08_04_Ataque_Puño";
    const string kick2Animation = "09_02_Ataque_Patada";

    //Anim Ataques Especiales
    const string specialDownAnimation = "14_Ataque_Especial_Abajo";
    const string specialNeutralAnimation = "15_Ataque_Especial_Neutral";
    const string specialUpAnimation = "16_Ataque_Especial_Arriba";
    const string specialAirAnimation = "17_Ataque_Especial_Aereo";

    //Anim Hitstun
    const string hitstunHighAnimation = "18_Hitstun_Alto";
    const string hitstunMediumAnimation = "19_Hitstun_Medio";
    const string hitstunLowAnimation = "20_Hitstun_Bajo";

    const string hitstunTripAnimation = "21_Hitstun_Trip";
    const string hitstunAirAnimation = "22_Hitstun_Air";
    const string hitstunLauncherAnimation = "23_Hitstun_Launcher";
    const string hitstunKnockdownAnimation = "24_Hitstun_Knockdown";

    //Anim Tech
    const string recoveryTechAnimation = "25_Recovery_Tech";
    const string recoveryRollAnimation = "26_Recovery_Roll";

    //Anim Guard
    const string guardAnimation = "27_Guard";
    const string hitGuardAnimation = "28_Hit_Guard";
    const string stunAnimation = "29_Stun";
    const string breakAnimation = "30_Break";
    #endregion

    //Componentes
    [HideInInspector]
    private Animator animator;

    //Rotación
    public GameObject playerObjetive;
    public string leftAnimation;
    private string rightAnimation;

    public bool KnockDown;
 
    void Awake()
    {
        inputManager = GetComponent<Scr_01_Control_Manager>();
        statsManager = GetComponent<Scr_03_Estadisticas>();
        fisicasManager = GetComponent<Scr_10_Fisicas_Manager>();

        animator = GetComponentInChildren<Animator>();
        animationManager = GetComponentInChildren<Scr_05_PJ0_Anim_Manager>();
        stateManager = GetComponentInChildren<Scr_02_Estado_Manager>();
        hitboxManager = GetComponentInChildren<Scr_06_PJ0_Hitbox_Manager>();
        hurtboxManager = GetComponentInChildren<Scr_08_PJ0_Hurtbox_Manager>();
    }

    private void FixedUpdate()
    {
        //Acciones en Tierra
        if(stateManager.estateGrounded)
        {
            //Girar sobre si mismo
            ActionTurnAround();
        }

        //Acciones Pasivas en Tierra
        if (stateManager.estateGrounded && stateManager.passiveAction)
        {
            //Bloqueo
            if (inputManager.buttonBlock)
            {
                ActionGuard();
            }
            else
            {
                //Idle Pose
                if (inputManager.idle)
                {
                    actualAction = "Idle";
                    stateManager.passiveAction = true;
                    ActionChangeAnimationState(idleAnimation);
                }

                //Movimiento Horizontal y Salto en Diagonal
                if (inputManager.buttonRight)
                {
                    ActionMoveCharacter(1, "Movimiento", "Salto_Diagonal_1");
                }
                else if (inputManager.buttonLeft)
                {
                    ActionMoveCharacter(-1, "Movimiento", "Salto_Diagonal_2");
                }
                else
                {
                    fisicasManager.FisicasStop();
                }

                //Salto Neutral
                if (inputManager.buttonJump)
                {
                    actualAction = "Salto_Neutral";
                    stateManager.cancelableAction = true;
                    ActionChangeAnimationState(jumpAnimation);
                    inputManager.buttonJump = false;
                    
                    fisicasManager.FisicasJump();
                }

                //Ataques
                if (!inputManager.buttonTrigger)
                {
                    if (inputManager.buttonPunch)
                    {
                        ActionAttack("Puño_1", true, false, punch1Animation);
                    }
                    if (inputManager.buttonKick)
                    {
                        ActionAttack("Patada_1", true, false, kick1Animation);
                    }
                }

                //Acciones dentro del Boton de Accion
                if (inputManager.buttonTrigger)
                {
                    //Ataques Unicos
                    if (inputManager.buttonPunch)
                    {
                        ActionAttack("Puño_Unico", false, true, uniquePunchAnimation);
                    }
                    if (inputManager.buttonKick)
                    {
                        ActionAttack("Patada_Unico", false, true, uniqueKickAnimation);
                    }
                }
            }
        }

        //Acciones Pasivas en Aire
        if (stateManager.estateAirborn && stateManager.passiveAction)
        {
            //Caer
            actualAction = "Caer";
            stateManager.passiveAction = true;
            ActionChangeAnimationState(fallAnimation);
        }

        //Acciones Cancelables en Aire
        if (stateManager.estateAirborn && stateManager.cancelableAction || stateManager.estateAirborn && stateManager.passiveAction)
        {
            //Doble Salto
            if (inputManager.buttonJump && fisicasManager.resetJump > 0)
            {
                actualAction = "Salto_Doble";
                stateManager.cancelableAction = true;
                ActionChangeAnimationState(doubleJumpAnimation);

                fisicasManager.FisicasJump();
            }

            //Ataques Normales
            if (inputManager.buttonPunch)
            {
                ActionAttack("Puño_Aire", true, false, airPunchAnimation);
            }
            if (inputManager.buttonKick)
            {
                ActionAttack("Patada_Aire", true, false, airKickAnimation);
            }

            //Ataque Especial
            if (inputManager.buttonSpecial)
            {
                ActionAttack("Especial_Aereo", false, true, specialAirAnimation);
            }
        } 

        //Strings
        if(stateManager.semiCancelableAction)
        {
            //String 1+
            if (actualAction == "Ataque_Puño_1" && hitboxManager.hitboxTrueConfirm)
            {
                //1+1
                if (inputManager.buttonPunch)
                {
                    ActionAttack("Puño_2", true, false, punch2Animation);
                }
            }
            //String 1+1+
            if (actualAction == "Ataque_Puño_2" && hitboxManager.hitboxTrueConfirm)
            {
                //1+1+1
                if (inputManager.buttonPunch)
                {
                    ActionAttack("Puño_3", false, true, punch3Animation);
                }
                //1+1+2
                if (inputManager.buttonKick)
                {
                    ActionAttack("Puño_4", false, true, punch4Animation);
                }
            }
            //String 2+
            if (actualAction == "Ataque_Patada_1" && hitboxManager.hitboxTrueConfirm)
            {
                //2+2
                if (inputManager.buttonKick)
                {
                    ActionAttack("Patada_2", true, false, kick2Animation);
                }
            }
        }

        //Ataques Especiales
        if (stateManager.estateGrounded && stateManager.passiveAction || stateManager.semiCancelableAction && hitboxManager.hitboxTrueConfirm) 
        {
            //Especial Abajo
            if (inputManager.buttonDown)
            {
                if (inputManager.buttonSpecial)
                {
                    ActionAttack("Especial_Abajo", false, true, specialDownAnimation);
                }
            }

            //Especial Neutral
            if (!inputManager.buttonDown && !inputManager.buttonUp && inputManager.buttonSpecial)
            {
                ActionAttack("Especial_Neutral", false, true, specialNeutralAnimation);
            }

            //Especial Arriba
            if (inputManager.buttonUp)
            {
                if (inputManager.buttonSpecial)
                {
                    ActionAttack("Especial_Arriba", false, true, specialUpAnimation);
                }
            }
        }

        //Hitstun
        if(hurtboxManager.hurtboxHit || stateManager.noCancelableAction && hurtboxManager.hurtboxHit)
        {
            ActionChangeHitstun();
        }

        if (stateManager.passiveAction)
        {
            hitboxManager.hitboxApagar = true;
        }

        if(KnockDown)
        {
           ActionKnockdownState();
        }
    }

    //Important Voids
    public void ActionChangeAnimationState(string newAnimation) //Cambio de Animaciones
    {
        if (actualAnimation == newAnimation) return;

        animator.Play(newAnimation);
        actualAnimation = newAnimation;
    }
    void ActionTurnAround()
    {
        if (playerObjetive.transform.position.x > transform.position.x)
        {
            leftAnimation = walkBackwardAnimation;
            rightAnimation = walkForwardAnimation;
        }
        if (playerObjetive.transform.position.x < transform.position.x)
        {
            leftAnimation = walkForwardAnimation;
            rightAnimation = walkBackwardAnimation;
        }
    } //Girar Animaciones

    void ActionMoveCharacter(float dir, string moveAction, string jumpAction)
    {
        actualAction = moveAction;
        stateManager.passiveAction = true;
        ActionChangeAnimationState(dir > 0 ? rightAnimation : leftAnimation);

        fisicasManager.FisicasWalk(dir);

        if (inputManager.buttonJump)
        {
            actualAction = jumpAction;
            stateManager.cancelableAction = true;
            ActionChangeAnimationState(jumpAnimation);
            inputManager.buttonJump = false;

            fisicasManager.FisicasDiagonalJump(dir);
        }
    } //Movimiento horizontal y diagonal
    void ActionAttack(string action, bool semiCancelable, bool noCancelable, string anim)
    {
        actualAction = "Ataque_" + action;
        stateManager.semiCancelableAction = semiCancelable;
        stateManager.noCancelableAction = noCancelable;
        ActionChangeAnimationState(anim);
        inputManager.buttonPunch = false;
        inputManager.buttonKick = false;
        animator.speed = 1f; //Para que la animacion no se congele con el HitStop

        if (stateManager.estateGrounded)
        {
            fisicasManager.FisicasStop();
        }
    } //Ataques Normales, Strings y Especiales
    
    //Hitstun
    public void ActionChangeHitstun()
    {
        //Hitbox Comun
        actualAction = "Hurt";
        stateManager.noCancelableAction = true;
        hitboxManager.hitboxApagar = true;

        if (stateManager.estateGrounded)
        {
            if (hurtboxManager.hurtobxManagerAnim == "Alto")
            {
                ActionHitstun(hitstunHighAnimation);
            }
            if (hurtboxManager.hurtobxManagerAnim == "Medio")
            {
                ActionHitstun(hitstunMediumAnimation);
            }
            if (hurtboxManager.hurtobxManagerAnim == "Bajo")
            {
                ActionHitstun(hitstunLowAnimation);
            }
            if (hurtboxManager.hurtobxManagerAnim == "Trip")
            {
                ActionHitstun(hitstunTripAnimation);
            }
            if (hurtboxManager.hurtobxManagerAnim == "Launcher")
            {
                ActionHitstun(hitstunLauncherAnimation);
            }
        }

        if (stateManager.estateAirborn)
        {
            if (hurtboxManager.hurtobxManagerAnim == "Alto" || hurtboxManager.hurtobxManagerAnim == "Medio" || hurtboxManager.hurtobxManagerAnim == "Bajo")
            {
                ActionHitstun(hitstunAirAnimation);
            }
            if (hurtboxManager.hurtobxManagerAnim == "Launcher")
            {
                ActionHitstun(hitstunLauncherAnimation);
            }
        }
    }
    void ActionHitstun(string anim)
    {
        ActionChangeAnimationState(anim);
        animationManager.HandleShake();
    } 

    //Knockdown
    public void ActionKnockdown()
    {
        ActionChangeAnimationState(hitstunKnockdownAnimation);
    }
    public void ActionKnockdownState()
    {
        if(inputManager.buttonUp)
        {
            ActionChangeAnimationState(recoveryTechAnimation);
            hurtboxManager.hurtboxHit = false;
            animator.speed = 1f;
            KnockDown = false;
        }

        if (inputManager.buttonLeft)
        {
            ActionRoll(-statsManager.rollDistance);
        }

        if (inputManager.buttonRight)
        {
            ActionRoll(statsManager.rollDistance);
        }
    }
    public void ActionRoll(float speed)
    {
        ActionChangeAnimationState(recoveryRollAnimation);
        hurtboxManager.hurtboxHit = false;
        animator.speed = 1f;
        KnockDown = false;

        fisicasManager.FisicasRoll(speed);
    }

    void ActionGuard()
    {
        actualAction = "Guard";
        stateManager.passiveAction = true;
        ActionChangeAnimationState(guardAnimation);

        fisicasManager.FisicasStop();
    }
    public void ActionReiniciar()
    {
        animator.Play(actualAnimation, 0, 0f);
    } //Reinicar Animacion de Hitstun
}
