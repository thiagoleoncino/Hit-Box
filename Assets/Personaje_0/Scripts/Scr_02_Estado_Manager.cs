using System.Collections;
using System.Collections.Generic;
using Unity.VisualScripting;
using UnityEngine;
public enum TipoAccion
{
    Pasiva,
    Cancelable,
    SemiCancelable,
    NoCancelable
}

public class Scr_02_Estado_Manager : MonoBehaviour
{
    [Header("Ground Check")]   
    public bool estateAirborn;
    public bool estateGrounded;
    private BoxCollider boxCollider;
    private LayerMask groundLayer;
    private bool IsGrounded()
    {
     return Physics.Raycast(boxCollider.bounds.center, Vector3.down, 
     boxCollider.bounds.extents.y + 0.1f, groundLayer);
    }  //Ground Check Codigo

    bool landing = true;

    [Header("Estado del Personaje")]
    public TipoAccion Estado_Actual;
    public bool passiveAction
    {
        get => Estado_Actual == TipoAccion.Pasiva;
        set { if (value) Estado_Actual = TipoAccion.Pasiva; }
    }
    public bool cancelableAction
    {
        get => Estado_Actual == TipoAccion.Cancelable;
        set { if (value) Estado_Actual = TipoAccion.Cancelable; }
    }
    public bool semiCancelableAction
    {
        get => Estado_Actual == TipoAccion.SemiCancelable;
        set { if (value) Estado_Actual = TipoAccion.SemiCancelable; }
    }
    public bool noCancelableAction
    {
        get => Estado_Actual == TipoAccion.NoCancelable;
        set { if (value) Estado_Actual = TipoAccion.NoCancelable; }
    }

    private Scr_04_PJ0_Accion_Manager accionManager;

    void Awake()
    {
        //Ground Check
        boxCollider = GetComponent<BoxCollider>();
        groundLayer = LayerMask.GetMask("Stage");

        //Estados del Jugador
        passiveAction = true;

        accionManager = GetComponent<Scr_04_PJ0_Accion_Manager>();
    }

    void Update()
    {
        estateGrounded = IsGrounded(); //Estado Grounded
        estateAirborn = !IsGrounded(); //Estado Airborn

        if (accionManager.actualAction != "Hurt")
        {
            if (landing && !estateAirborn)
            {
                passiveAction = true;
            }
            landing = estateAirborn;
        } //Landing
    }
}
