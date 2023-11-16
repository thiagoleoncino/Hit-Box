using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Scr_Prueba : MonoBehaviour
{    
    private bool collisionOccurred = false;

    [Header("Caracteristicas Ataque")]
    public float Daño;
    public float Hitstun;
    public float Shake;
    public string Peso;
    public Vector3 Knockback;
    public float KnockHitstun;

    //Shake Effect
    private Vector3 Pos_Original;
    public float Shake_Amount;

    private void OnTriggerEnter(Collider other)
    {
        if (!collisionOccurred && other.gameObject.tag == "Hitbox_Player_1")
        {
            Debug.Log("Hit");
            collisionOccurred = false;
            Hit_Shake();
        }
    }

    private void OnTriggerStay(Collider other)
    {
        if (other.gameObject.tag == "Hitbox_Player_1")
        {
            collisionOccurred = false;
        }
    }

    private void OnTriggerExit(Collider other)
    {
        // Reset the flag when the other object exits the trigger zone.
        if (other.gameObject.tag == "Hitbox_Player_1")
        {
            collisionOccurred = true;
        }
    }

    public void Hit_Data(float HB_Daño, float HB_Hitstun, float HB_Shake, string HB_Peso, Vector3 HB_Knockback, float HB_Knockback_Hitstun)
    {
        Daño = HB_Daño;

        Hitstun = HB_Hitstun;

        Shake = HB_Shake;

        Peso = HB_Peso;

        Knockback = HB_Knockback;

        KnockHitstun = HB_Knockback_Hitstun;
    }

    private void Update()
    {
        Pos_Original = transform.position;
    }

    private void Hit_Shake() //Hitstun Codigo
    {
        Vector3 randomOffset = new Vector3(Random.Range(-Shake_Amount, Shake_Amount), 0, 0) * 0.1f;
        transform.position = Pos_Original + randomOffset;
    }
}