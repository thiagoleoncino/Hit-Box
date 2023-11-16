using System.Collections;
using System.Collections.Generic;
using Unity.VisualScripting;
using UnityEditor.Experimental.GraphView;
using UnityEditor.PackageManager.UI;
using UnityEngine;
using static Unity.VisualScripting.Metadata;

public class Scr_Tiles_Manager : MonoBehaviour
{
    void Start()
    {
        // Obtén todos los transform de los hijos
        Transform[] childTransforms = GetComponentsInChildren<Transform>();

        // Si hay al menos un hijo
        if (childTransforms.Length > 1) // Exclude the parent's transform itself
        {
            // Encuentra los límites del espacio ocupado por los hijos
            Vector3 minBounds = childTransforms[1].position;
            Vector3 maxBounds = childTransforms[1].position;
            for (int i = 2; i < childTransforms.Length; i++)
            {
                Vector3 position = childTransforms[i].position;
                minBounds = Vector3.Min(minBounds, position);
                maxBounds = Vector3.Max(maxBounds, position);
            }

            // Calcula el centro y tamaño del collider del objeto padre
            Vector3 center = (maxBounds + minBounds) / 2f;
            Vector3 size = maxBounds - minBounds;

            // Agrega un collider al objeto padre usando los cálculos anteriores
            BoxCollider parentCollider = gameObject.AddComponent<BoxCollider>();
            parentCollider.center = center - transform.position;
            parentCollider.size = size;
        }
    }
}
