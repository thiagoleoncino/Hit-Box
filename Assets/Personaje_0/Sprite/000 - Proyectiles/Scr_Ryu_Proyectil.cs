using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Scr_Ryu_Proyectil : MonoBehaviour
{
    [Header("Caracteristicas Ataque")]
    public float moveSpeed = 5.0f;
    public float HB_Daño;
    public float HB_Hitstun;
    public float HB_Shake;
    public string HB_Peso;
    public Vector3 HB_Knockback;
    public float HB_Knockback_Hitstun;

    private void Update()
    {
        // Obtener la dirección hacia adelante del objeto
        Vector3 moveDirection = transform.right;

        // Normalizar la dirección para mantener una velocidad constante si la magnitud no es 1
        moveDirection.Normalize();

        // Mover el objeto en la dirección hacia adelante
        transform.position += moveDirection * moveSpeed * Time.deltaTime;
    }

    private void OnTriggerEnter(Collider other)
    {
            Scr_09_PJ0_Hurtbox Hurtbox = other.GetComponent<Scr_09_PJ0_Hurtbox>();
            if (Hurtbox != null)
            {
                Hurtbox.Hit_Data(HB_Daño, HB_Hitstun, HB_Shake, HB_Peso, HB_Knockback, HB_Knockback_Hitstun);
            }
            Destroy(gameObject);
    }
}
