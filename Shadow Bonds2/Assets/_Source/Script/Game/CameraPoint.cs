using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Camerapoint : MonoBehaviour
{
    public Transform player;  // Игрок, за которым будет следовать камера
    public float smoothSpeed = 0.125f;  // Скорость плавного следования
    public Vector3 offset;  // Смещение камеры относительно игрока
    void LateUpdate()
    {
        // Вычисляем целевую позицию камеры с учетом смещения
        Vector3 targetPosition = player.position + offset;

        // Плавно перемещаем камеру к целевой позиции
        transform.position = Vector3.Lerp(transform.position, targetPosition, smoothSpeed);
    }
}
