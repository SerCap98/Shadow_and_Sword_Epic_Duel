using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class HUDCorrector : MonoBehaviour
{
        // Referencia al transform del personaje
        public Transform characterTransform;

        void LateUpdate()
        {
            if (characterTransform != null)
            {
                // Aquí se cancela la rotación en el eje X. 
                // Si la escala en X del personaje es -1 (invertido), la escala en X del HUD será -1 también,
                // lo que hará que el HUD se mire como si no estuviera invertido.
                Vector3 localScale = transform.localScale;
                localScale.x = characterTransform.localScale.x * 1;
                transform.localScale = localScale;
            }
        }
   

}
