using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public enum Control_Jugador
{
    player1,
    player2
}

public class Scr_01_Control_Manager : MonoBehaviour
{
    [Header("Player Bool")]
    //Determina que jugador esta al control
    public Control_Jugador Jugador_Actual;
    public bool player1
    {
        get => Jugador_Actual == Control_Jugador.player1;
        set { if (value) Jugador_Actual = Control_Jugador.player1; }
    }
    public bool player2
    {
        get => Jugador_Actual == Control_Jugador.player2;
        set { if (value) Jugador_Actual = Control_Jugador.player2; }
    }

    [Header("Bool de Accion")]
    public bool idle;

    [Space] //Botones de Movimiento
    public bool buttonLeft;
    public bool buttonRight;
    public bool buttonUp;
    public bool buttonDown;

    [Space] //Botones de Accion
    public bool buttonJump;
    public bool buttonPunch;
    public bool buttonKick;
    public bool buttonSpecial;

    [Space] //Triggers
    public bool buttonBlock;
    public bool buttonTrigger;

    void Update()
    {
        if (player1)
        {
            ControlPlayer1();
        }

        if (player2)
        {
            ControlPlayer2();
        }

        idle = (!buttonLeft && !buttonRight && !buttonJump && !buttonPunch && !buttonKick && !buttonSpecial && !buttonBlock); //Variable que detecta si una Accion se esta realizando
    }

    void UpdateButtonState(KeyCode key, ref bool buttonState)
    {
        if (Input.GetKeyDown(key))
        {
            buttonState = true;
        }
        if (Input.GetKeyUp(key))
        {
            buttonState = false;
        }
    }

    void ControlPlayer1()
    {
        //Botones de Movimiento
        UpdateButtonState(KeyCode.S, ref buttonDown); //Boton Abajo
        UpdateButtonState(KeyCode.A, ref buttonLeft); //Boton Izquierda
        UpdateButtonState(KeyCode.W, ref buttonUp); //Boton Arriba
        UpdateButtonState(KeyCode.D, ref buttonRight); //Boton Derecha
        UpdateButtonState(KeyCode.Space, ref buttonJump); //Salto & Doble Salto

        //Botones de Accion
        UpdateButtonState(KeyCode.J, ref buttonPunch); //Ataque Puño
        UpdateButtonState(KeyCode.K, ref buttonKick); //Ataque Patada
        UpdateButtonState(KeyCode.L, ref buttonSpecial); //Ataque Especial

        //Triggers
        UpdateButtonState(KeyCode.O, ref buttonTrigger); //Boton de Acción
        UpdateButtonState(KeyCode.U, ref buttonBlock); //Boton de Bloqueo
    } //Controles del Jugador 1

    void ControlPlayer2()
    {
        //Botones de Movimiento
        UpdateButtonState(KeyCode.DownArrow, ref buttonDown); //Boton Abajo
        UpdateButtonState(KeyCode.LeftArrow, ref buttonLeft); //Boton Izquierda
        UpdateButtonState(KeyCode.UpArrow, ref buttonUp); //Boton Arriba
        UpdateButtonState(KeyCode.RightArrow, ref buttonRight); //Boton Derecha
        UpdateButtonState(KeyCode.Keypad0, ref buttonJump); //Salto & Doble Salto

        //Botones de Accion
        UpdateButtonState(KeyCode.Keypad1, ref buttonPunch); //Ataque Puño
        UpdateButtonState(KeyCode.Keypad2, ref buttonKick); //Ataque Patada
        UpdateButtonState(KeyCode.Keypad3, ref buttonSpecial); //Ataque Especial

        //Triggers
        UpdateButtonState(KeyCode.Keypad6, ref buttonTrigger); //Boton de Acción
        UpdateButtonState(KeyCode.Keypad4, ref buttonBlock); //Boton de Bloqueo
    } //Controles del Jugador 2

}
