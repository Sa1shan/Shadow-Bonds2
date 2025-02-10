using UnityEngine;

namespace _Source.Script.Enemy
{
    public class MoveBetweenPoints : MonoBehaviour
    {
        [Header("Movement Settings")]
        [SerializeField] private Transform startPoint;  // Начальная точка
        [SerializeField] private Transform endPoint;    // Конечная точка
        [SerializeField] private float moveSpeed = 3f;  // Скорость перемещения

        private void Update()
        {
            // Перемещаем объект между двумя точками с использованием Lerp
            float step = moveSpeed * Time.deltaTime;  // Расстояние, которое нужно пройти за кадр
            transform.position = Vector3.MoveTowards(transform.position, endPoint.position, step);

            // Когда объект достигнет конечной точки, можно установить его в начальную точку или остановить
            if (transform.position == endPoint.position)
            {
                // Для возврата к начальной точке
                Transform temp = startPoint;
                startPoint = endPoint;
                endPoint = temp;
            }
        }
    }
}
