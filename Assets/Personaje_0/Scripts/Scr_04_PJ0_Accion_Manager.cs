using UnityEngine;

public class Scr_04_PJ0_Accion_Manager : MonoBehaviour
{

    //Scripts 1
    private Scr_01_Control_Manager inputManager;
    private Scr_02_Estado_Manager stateManager;
    private Scr_03_Estadisticas statsManager;

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
    public Rigidbody rigidBody;
    private Animator animator;

    //Rotación
    public GameObject playerObjetive;
    private Vector3 vectorLeft;
    private Vector3 vectorRight;
    private string leftAnimation;
    private string rightAnimation;

    //Restablecer Saltos
    private int resetJump;
    public bool KnockDown;
 
    void Awake()
    {
        rigidBody = GetComponent<Rigidbody>();
        inputManager = GetComponent<Scr_01_Control_Manager>();
        statsManager = GetComponent<Scr_03_Estadisticas>();

        animator = GetComponentInChildren<Animator>();
        animationManager = GetComponentInChildren<Scr_05_PJ0_Anim_Manager>();
        stateManager = GetComponentInChildren<Scr_02_Estado_Manager>();
        hitboxManager = GetComponentInChildren<Scr_06_PJ0_Hitbox_Manager>();
        hurtboxManager = GetComponentInChildren<Scr_08_PJ0_Hurtbox_Manager>();

        resetJump = statsManager.jumpAmount;
    }

    private void FixedUpdate()
    {
        //Acciones en Tierra
        if(stateManager.estateGrounded)
        {
            //Reseta la cantidad de saltos disponibles
            resetJump = statsManager.jumpAmount; 

            //Girar sobre si mismo
            TurnAround();
        }

        //Acciones Pasivas en Tierra
        if (stateManager.estateGrounded && stateManager.passiveAction)
        {
            //Bloqueo
            if (inputManager.buttonBlock)
            {
                Guard();
            }
            else
            {
                //Idle Pose
                if (inputManager.idle)
                {
                    actualAction = "Idle";
                    stateManager.passiveAction = true;
                    ChangeAnimationState(idleAnimation);
                }

                //Movimiento Horizontal y Salto en Diagonal
                if (inputManager.buttonRight)
                {
                    MoveCharacter(statsManager.groundSpeed, "Movimiento", "Salto_Diagonal_1");
                }
                else if (inputManager.buttonLeft)
                {
                    MoveCharacter(-statsManager.groundSpeed, "Movimiento", "Salto_Diagonal_2");
                }
                else
                {
                    rigidBody.velocity = new Vector3(0, 0, rigidBody.velocity.z);
                }

                //Salto Neutral
                if (inputManager.buttonJump)
                {
                    actualAction = "Salto_Neutral";
                    stateManager.cancelableAction = true;
                    ChangeAnimationState(jumpAnimation);
                    rigidBody.velocity = new Vector3(rigidBody.velocity.x, statsManager.jumpPower, rigidBody.velocity.z);
                    inputManager.buttonJump = false;
                    resetJump--;
                }

                //Ataques
                if (!inputManager.buttonTrigger)
                {
                    if (inputManager.buttonPunch)
                    {
                        Attack("Puño_1", true, false, punch1Animation);
                    }
                    if (inputManager.buttonKick)
                    {
                        Attack("Patada_1", true, false, kick1Animation);
                    }
                }

                //Acciones dentro del Boton de Accion
                if (inputManager.buttonTrigger)
                {
                    //Ataques Unicos
                    if (inputManager.buttonPunch)
                    {
                        Attack("Puño_Unico", false, true, uniquePunchAnimation);
                    }
                    if (inputManager.buttonKick)
                    {
                        Attack("Patada_Unico", false, true, uniqueKickAnimation);
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
            ChangeAnimationState(fallAnimation);
        }

        //Acciones Cancelables en Aire
        if (stateManager.estateAirborn && stateManager.cancelableAction || stateManager.estateAirborn && stateManager.passiveAction)
        {
            //Doble Salto
            if (inputManager.buttonJump && resetJump > 0)
            {
                actualAction = "Salto_Doble";
                stateManager.cancelableAction = true;
                ChangeAnimationState(doubleJumpAnimation);
                rigidBody.velocity = new Vector3(rigidBody.velocity.x, statsManager.jumpPower, rigidBody.velocity.z);
                resetJump--;
            }

            //Ataques Normales
            if (inputManager.buttonPunch)
            {
                Attack("Puño_Aire", true, false, airPunchAnimation);
            }
            if (inputManager.buttonKick)
            {
                Attack("Patada_Aire", true, false, airKickAnimation);
            }

            //Ataque Especial
            if (inputManager.buttonSpecial)
            {
                Attack("Especial_Aereo", false, true, specialAirAnimation);
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
                    Attack("Puño_2", true, false, punch2Animation);
                }
            }
            //String 1+1+
            if (actualAction == "Ataque_Puño_2" && hitboxManager.hitboxTrueConfirm)
            {
                //1+1+1
                if (inputManager.buttonPunch)
                {
                    Attack("Puño_3", false, true, punch3Animation);
                }
                //1+1+2
                if (inputManager.buttonKick)
                {
                    Attack("Puño_4", false, true, punch4Animation);
                }
            }
            //String 2+
            if (actualAction == "Ataque_Patada_1" && hitboxManager.hitboxTrueConfirm)
            {
                //2+2
                if (inputManager.buttonKick)
                {
                    Attack("Patada_2", true, false, kick2Animation);
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
                    Attack("Especial_Abajo", false, true, specialDownAnimation);
                }
            }

            //Especial Neutral
            if (!inputManager.buttonDown && !inputManager.buttonUp && inputManager.buttonSpecial)
            {
                Attack("Especial_Neutral", false, true, specialNeutralAnimation);
            }

            //Especial Arriba
            if (inputManager.buttonUp)
            {
                if (inputManager.buttonSpecial)
                {
                    Attack("Especial_Arriba", false, true, specialUpAnimation);
                }
            }
        }

        //Hitstun
        if(hurtboxManager.hurtboxHit || stateManager.noCancelableAction && hurtboxManager.hurtboxHit)
        {
            ChangeHitstun();
        }

        if (stateManager.passiveAction)
        {
            hitboxManager.hitboxApagar = true;
        }

        if(KnockDown)
        {
           KnockdownState();
        }
    }

    //Important Voids
    public void ChangeAnimationState(string newAnimation) //Cambio de Animaciones
    {
        if (actualAnimation == newAnimation) return;

        animator.Play(newAnimation);
        actualAnimation = newAnimation;
    }
    void TurnAround()
    {
        if (playerObjetive.transform.position.x > transform.position.x)
        {
            transform.eulerAngles = new Vector3(0, 0, 0);
            vectorLeft = Vector3.left;
            vectorRight = Vector3.right;
            leftAnimation = walkBackwardAnimation;
            rightAnimation = walkForwardAnimation;
        }
        if (playerObjetive.transform.position.x < transform.position.x)
        {
            transform.eulerAngles = new Vector3(0, 180, 0);
            vectorLeft = Vector3.right;
            vectorRight = Vector3.left;
            leftAnimation = walkForwardAnimation;
            rightAnimation = walkBackwardAnimation;
        }
    } //Girar
    void MoveCharacter(float speed, string moveAction, string jumpAction)
    {
        actualAction = moveAction;
        stateManager.passiveAction = true;
        ChangeAnimationState(speed > 0 ? rightAnimation : leftAnimation);
        rigidBody.velocity = new Vector3(speed, rigidBody.velocity.y, rigidBody.velocity.z);

        if (inputManager.buttonJump)
        {
            actualAction = jumpAction;
            stateManager.cancelableAction = true;
            ChangeAnimationState(jumpAnimation);
            rigidBody.velocity = new Vector3(speed, statsManager.jumpPower, rigidBody.velocity.z);
            inputManager.buttonJump = false;
            resetJump--;
        }
    } //Movimiento horizontal y diagonal
    void Attack(string action, bool semiCancelable, bool noCancelable, string anim)
    {
        actualAction = "Ataque_" + action;
        stateManager.semiCancelableAction = semiCancelable;
        stateManager.noCancelableAction = noCancelable;
        ChangeAnimationState(anim);
        inputManager.buttonPunch = false;
        inputManager.buttonKick = false;
        animator.speed = 1f; //Para que la animacion no se congele con el HitStop

        if (stateManager.estateGrounded)
        {
            rigidBody.velocity = new Vector3(0, 0, 0);
        }
    } //Ataques Normales, Strings y Especiales
    public void ChangeHitstun()
    {
        //Hitbox Comun
        actualAction = "Hurt";
        stateManager.noCancelableAction = true;
        hitboxManager.hitboxApagar = true;

        if (stateManager.estateGrounded)
        {
            if (hurtboxManager.hurtobxManagerAnim == "Alto")
            {
                Hitstun(hitstunHighAnimation);
            }
            if (hurtboxManager.hurtobxManagerAnim == "Medio")
            {
                Hitstun(hitstunMediumAnimation);
            }
            if (hurtboxManager.hurtobxManagerAnim == "Bajo")
            {
                Hitstun(hitstunLowAnimation);
            }
            if (hurtboxManager.hurtobxManagerAnim == "Trip")
            {
                Hitstun(hitstunTripAnimation);
            }
            if (hurtboxManager.hurtobxManagerAnim == "Launcher")
            {
                Hitstun(hitstunLauncherAnimation);
            }
        }

        if (stateManager.estateAirborn)
        {
            if (hurtboxManager.hurtobxManagerAnim == "Alto" || hurtboxManager.hurtobxManagerAnim == "Medio" || hurtboxManager.hurtobxManagerAnim == "Bajo")
            {
                Hitstun(hitstunAirAnimation);
            }
            if (hurtboxManager.hurtobxManagerAnim == "Launcher")
            {
                Hitstun(hitstunLauncherAnimation);
            }
        }
    }
    void Hitstun(string anim)
    {
        ChangeAnimationState(anim);
        animationManager.HandleShake();
    } //Hitstun
    public void Reiniciar() 
    {
        animator.Play(actualAnimation, 0, 0f);
    } //Reinicar Animacion de Hitstun
    public void Knockback()
    {
        rigidBody.velocity = new Vector3(hurtboxManager.hurtobxManagerKnockback.x * vectorLeft.x, hurtboxManager.hurtobxManagerKnockback.y, rigidBody.velocity.z);
    }
    public void Knockdown()
    {
        ChangeAnimationState(hitstunKnockdownAnimation);
    }
    public void KnockdownState()
    {
        if(inputManager.buttonUp)
        {
            ChangeAnimationState(recoveryTechAnimation);
            hurtboxManager.hurtboxHit = false;
            animator.speed = 1f;
            KnockDown = false;
        }

        if (inputManager.buttonLeft)
        {
            Roll(-statsManager.rollDistance);
        }

        if (inputManager.buttonRight)
        {
            Roll(statsManager.rollDistance);
        }
    }
    public void Roll(float Roll)
    {
        ChangeAnimationState(recoveryRollAnimation);
        hurtboxManager.hurtboxHit = false;
        animator.speed = 1f;
        KnockDown = false;
        rigidBody.velocity = new Vector3(Roll, rigidBody.velocity.y, rigidBody.velocity.z);
        rigidBody.constraints = rigidBody.constraints | RigidbodyConstraints.FreezePositionY;
        Physics.IgnoreLayerCollision(6, 6, true);
    }

    void Guard()
    {
        actualAction = "Guard";
        stateManager.passiveAction = true;
        ChangeAnimationState(guardAnimation);
        rigidBody.velocity = new Vector3(0, 0, 0);
    }

    //Fisicas de los Ataques Especiales
    public void Especial_Abajo()
    {
        rigidBody.velocity = new Vector3(10 * vectorRight.x, rigidBody.velocity.y, rigidBody.velocity.z);
    }
    public void Especial_Arriba()
    {
        rigidBody.velocity = new Vector3(rigidBody.velocity.x, 20, rigidBody.velocity.z);
    }

}
