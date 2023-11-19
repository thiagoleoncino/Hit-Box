using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Scr_10_Fisicas_Manager : MonoBehaviour
{
    private Scr_02_Estado_Manager stateManager;
    private Scr_03_Estadisticas statsManager;
    private Scr_04_PJ0_Accion_Manager actionManager;
    private Scr_08_PJ0_Hurtbox_Manager hurtboxManager;
    
    [HideInInspector]
    public Rigidbody rigidBody;

    public int resetJump;

    //Rotación
    private Vector3 vectorLeft;
    private Vector3 vectorRight;

    void Awake()
    {
        rigidBody = GetComponent<Rigidbody>();

        stateManager = GetComponent<Scr_02_Estado_Manager>();
        statsManager = GetComponent<Scr_03_Estadisticas>();
        actionManager = GetComponent<Scr_04_PJ0_Accion_Manager>();
        hurtboxManager = GetComponentInChildren<Scr_08_PJ0_Hurtbox_Manager>();

        resetJump = statsManager.jumpAmount;
    }

    private void FixedUpdate()
    {
        //Acciones en Tierra
        if (stateManager.estateGrounded)
        {
            //Girar sobre si mismo
            TurnAround();

            //Reseta la cantidad de saltos disponibles
            resetJump = statsManager.jumpAmount;
        }
    }

    void TurnAround()
    {
        if (actionManager.playerObjetive.transform.position.x > transform.position.x)
        {
            transform.eulerAngles = new Vector3(0, 0, 0);
            vectorLeft = Vector3.left;
            vectorRight = Vector3.right;
        }
        if (actionManager.playerObjetive.transform.position.x < transform.position.x)
        {
            transform.eulerAngles = new Vector3(0, 180, 0);
            vectorLeft = Vector3.right;
            vectorRight = Vector3.left;
        }
    } //Girar

    //Fisicas Universales
    public void FisicasWalk(float dir)
    {
        rigidBody.velocity = new Vector3(statsManager.groundSpeed * dir, rigidBody.velocity.y, rigidBody.velocity.z);
    }
    public void FisicasJump()
    {
        rigidBody.velocity = new Vector3(rigidBody.velocity.x, statsManager.jumpPower, rigidBody.velocity.z);
        resetJump--;
    }
    public void FisicasDiagonalJump(float dir)
    {
        rigidBody.velocity = new Vector3(statsManager.groundSpeed * dir, statsManager.jumpPower, rigidBody.velocity.z);
        resetJump--;
    }
    public void FisicasStop()
    {
        rigidBody.velocity = new Vector3(0, 0, 0);
    }
    public void FisicasKnockback()
    {
        //Trae los datos de los golpes de Scr_08_PJ0_Hurtbox_Manager
        rigidBody.velocity = new Vector3(hurtboxManager.hurtobxManagerKnockback.x * vectorLeft.x, hurtboxManager.hurtobxManagerKnockback.y, rigidBody.velocity.z);
    }
    public void FisicasRoll(float dir)
    {
        rigidBody.velocity = new Vector3(dir, rigidBody.velocity.y, rigidBody.velocity.z);
        Physics.IgnoreLayerCollision(6, 6, true);
    }

    //Fisicas de los Ataques Especiales
    public void FisicasEspecialAbajo()
    {
        rigidBody.velocity = new Vector3(10 * vectorRight.x, rigidBody.velocity.y, rigidBody.velocity.z);
    }
    public void FisicasEspecialArriba()
    {
        rigidBody.velocity = new Vector3(rigidBody.velocity.x, 20, rigidBody.velocity.z);
    }

}
