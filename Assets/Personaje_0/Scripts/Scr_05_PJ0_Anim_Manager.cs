using UnityEngine;

public class Scr_05_PJ0_Anim_Manager : MonoBehaviour
{
    //Scripts
    private Scr_01_Control_Manager inputManager;
    private Scr_02_Estado_Manager stateManager;
    private Scr_04_PJ0_Accion_Manager actionManager;
    private Scr_06_PJ0_Hitbox_Manager hitboxManager;
    private Scr_08_PJ0_Hurtbox_Manager hurtboxManager;
    private Scr_10_Fisicas_Manager fisicasManager;

    //Componentes
    private Animator animator;

    //Stop Effect
    public bool boolStop;
    public float stopTime = 10;

    //Shake Effect
    private Vector3 originalPosition;
    public float shakeAmount;

    //Ataque Especial Neutral
    public GameObject proyectilPJ0;
    public Transform especialNeutral;

    public float NewFloat;

    void Awake()
    {
        inputManager = GetComponentInParent<Scr_01_Control_Manager>();
        Transform parentTransform = transform.parent;
        stateManager = parentTransform.GetComponentInChildren<Scr_02_Estado_Manager>();
        actionManager = GetComponentInParent<Scr_04_PJ0_Accion_Manager>();
        hitboxManager = GetComponentInChildren<Scr_06_PJ0_Hitbox_Manager>();
        hurtboxManager = GetComponentInChildren<Scr_08_PJ0_Hurtbox_Manager>();
        fisicasManager = GetComponentInParent<Scr_10_Fisicas_Manager>();
        animator = GetComponent<Animator>();
    }

    void Update()
    {
        originalPosition = transform.parent.position;

        if (boolStop)
        {
            stopTime--;
        }

        if (stopTime <= 0)
        {
            boolStop = false;
            animator.speed = 1f;
            transform.position = originalPosition;
            fisicasManager.rigidBody.constraints &= ~(RigidbodyConstraints.FreezePositionX | RigidbodyConstraints.FreezePositionY);
        }
    }

    public void HandleHit() 
    {
        boolStop = true;
        stopTime = hitboxManager.PurebaHitstunDebil;
        animator.speed = 0f;
        fisicasManager.rigidBody.constraints |= RigidbodyConstraints.FreezePositionX | RigidbodyConstraints.FreezePositionY;
        Debug.Log("HandleHit");
    }

    public void HandleShake() //Hitstun Codigo
    {
        Vector3 randomOffset = new Vector3(Random.Range(-shakeAmount, shakeAmount), 0, 0) * 0.1f;
        transform.position = originalPosition + randomOffset;
        animator.speed = 0f;
    }

    #region Eventos de Animacion

    #region Eventos Universales
    //Caida
    public void Fall()
    {
        stateManager.passiveAction = true;
    }

    //Ataques
    public void Cancel_Prevention()
    {
        hitboxManager.hitboxApagar = true;
        boolStop = false;
    }

    public void Terminar_Ataque()
    {
        stateManager.passiveAction = true;
        inputManager.buttonPunch = false;
        inputManager.buttonKick = false;
        hitboxManager.hitboxApagar = true;
    }

    //Hitstun
    public void StartHitstun() 
    {
        stopTime = hurtboxManager.hurtobxManagerHitstun;
        shakeAmount = hurtboxManager.hurtobxManagerShake;
        boolStop = true;
    }
    public void StartKnockback()
    {
        fisicasManager.FisicasKnockback();
    }
    public void EndHitstun()
    {
        stateManager.passiveAction = true;
        hurtboxManager.hurtboxApagar = true;
        hurtboxManager.hurtboxHit = false;
        Physics.IgnoreLayerCollision(6, 6, false);
        fisicasManager.rigidBody.constraints &= ~RigidbodyConstraints.FreezePositionY;
    }

    public void StartKnocdkown()
    {
        actionManager.ActionKnockdown();
    }

    public void EndKnocdkown()
    {
        animator.speed = 0f;
        stopTime = 10;
        shakeAmount = 0;
        actionManager.KnockDown = true;
    }

    #endregion

    #region Eventos de Personaje

    //Ataques Especiales
    public void Anim_Especial_Abajo()
    {
        fisicasManager.FisicasEspecialAbajo();
    }
    public void Anim_Especial_Neutral()
    {
        GameObject nuevoProyectil = Instantiate(proyectilPJ0, especialNeutral.position, especialNeutral.rotation);
        if (inputManager.player1)
        {
            nuevoProyectil.tag = "Hitbox_Player_1";
        }
        if (inputManager.player2)
        {
            nuevoProyectil.tag = "Hitbox_Player_2";
        }

    }
    public void Anim_Especial_Arriba()
    {
        fisicasManager.FisicasEspecialArriba();
    }

    #endregion

    #endregion
}
