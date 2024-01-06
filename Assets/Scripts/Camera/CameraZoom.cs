using UnityEngine;

public class SplitScreenCamera : MonoBehaviour
{
    public Transform player1;
    public Transform player2;

    private Camera cam;

    private void Start()
    {
        // Obtén la referencia de la cámara
        cam = GetComponent<Camera>();
    }

    private void Update()
    {
        if (player1 != null && player2 != null)
        {
            // Encuentra el centro entre los dos jugadores
            Vector3 middlePoint = (player1.position + player2.position) / 2f;

            // Ajusta la posición de la cámara
            transform.position = new Vector3(middlePoint.x, middlePoint.y, transform.position.z);

            // Ajusta el tamaño de la cámara para contener ambos jugadores
            float distance = Vector3.Distance(player1.position, player2.position);
            float size = distance / 2f;

            cam.orthographicSize = Mathf.Max(size, 5f); // Ajusta 5f según tus necesidades
        }
    }
}
