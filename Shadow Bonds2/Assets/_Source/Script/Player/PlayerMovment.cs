using System;
using UnityEngine;

namespace _Source.Script.Player
{
    public class PlayerMovment : MonoBehaviour
    {
        [SerializeField] private float moveSpeed = 5f; // Скорость движения
        [SerializeField] private SpriteRenderer spriteRenderer; // Компонент SpriteRenderer персонажа
        [SerializeField] private Sprite upSprite;
        [SerializeField] private Sprite downSprite;
        [SerializeField] private Sprite leftSprite;
        [SerializeField] private Sprite rightSprite;

        [SerializeField] private Weapon weaponController; // Ссылка на скрипт оружия

        private Vector3 targetPosition;

        private void Update()
        {
            // Получаем позицию мыши в мировых координатах
            targetPosition = Camera.main.ScreenToWorldPoint(Input.mousePosition);
            targetPosition.z = 0; // Сбрасываем Z, чтобы остаться в 2D

            // Проверяем нажатие клавиши W
            if (Input.GetKey(KeyCode.W))
            {
                MoveToTarget(targetPosition);
            }
        }

        private void MoveToTarget(Vector3 targetPosition)
        {
            // Рассчитываем направление к цели
            Vector3 direction = (targetPosition - transform.position).normalized;

            // Двигаем игрока в направлении цели
            transform.position += direction * moveSpeed * Time.deltaTime;

            // Смена спрайта персонажа
            if (Mathf.Abs(direction.y) > Mathf.Abs(direction.x)) // Больше движение по вертикали
            {
                if (direction.y > 0)
                    spriteRenderer.sprite = upSprite; // Движение вверх
                else
                    spriteRenderer.sprite = downSprite; // Движение вниз
            }
            else // Больше движение по горизонтали
            {
                if (direction.x > 0)
                    spriteRenderer.sprite = rightSprite; // Движение вправо
                else
                    spriteRenderer.sprite = leftSprite; // Движение влево
            }

            // Передаём направление в скрипт оружия
            weaponController.SetDirection(direction);
        }
    }
}
