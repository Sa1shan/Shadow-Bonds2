using System;
using UnityEngine;
using UnityEngine.Serialization;

namespace _Source.Script.Player
{
    public class Bullet : MonoBehaviour
    {
        [SerializeField] private SpriteRenderer spriteRenderer; // SpriteRenderer для объекта
        
        [SerializeField] private Sprite NSide;
        [SerializeField] private Sprite SSide;
        [SerializeField] private Sprite WSide;
        [SerializeField] private Sprite ESide;
        [SerializeField] private Sprite WNSide;
        [SerializeField] private Sprite NESide;
        [SerializeField] private Sprite WSSide;
        [SerializeField] private Sprite SESide;
        
        private Rigidbody2D _rb; // Ссылка на Rigidbody2D пули
        private Vector2 _direction; // Направление пули
        [SerializeField] private float bulletSpeed = 10f; // Скорость пули

        private void Start()
        {
            _rb = GetComponent<Rigidbody2D>();
            if (_rb == null)
            {
                Debug.LogError("Rigidbody2D не найден на пуле!");
                return;
            }

            // Устанавливаем скорость пули
            _rb.velocity = _direction * bulletSpeed;
        }

        private void Update()
        {
            if (IsOutOfBounds())
            {
                Destroy(gameObject); // Удаляем пулю, если она вышла за пределы камеры
            }
        }

        public void SetDirection(Vector2 newDirection)
        {
            _direction = newDirection; // Устанавливаем направление пули
            SetSpriteBasedOnDirection(_direction); // Фиксируем спрайт при выстреле
        }
    
        private void SetSpriteBasedOnDirection(Vector2 direction)
        {
            float angle = Mathf.Atan2(direction.y, direction.x) * Mathf.Rad2Deg;

            // Проверяем угол и назначаем спрайт на основе диапазона углов
            if (angle >= -22.5f && angle < 22.5f)           // Право (синий)
                spriteRenderer.sprite = ESide;
            else if (angle >= 22.5f && angle < 67.5f)       // Право-вверх (правый фиолетовый)
                spriteRenderer.sprite = NESide;
            else if (angle >= 67.5f && angle < 112.5f)      // Вверх (верхний синий)
                spriteRenderer.sprite = NSide;
            else if (angle >= 112.5f && angle < 157.5f)     // Лево-вверх (левый фиолетовый)
                spriteRenderer.sprite = WNSide;
            else if ((angle >= 157.5f && angle <= 180f) || (angle >= -180f && angle < -157.5f)) // Лево (зелёный)
                spriteRenderer.sprite = WSide;
            else if (angle >= -157.5f && angle < -112.5f)   // Лево-вниз (левый фиолетовый)
                spriteRenderer.sprite = WSSide;
            else if (angle >= -112.5f && angle < -67.5f)    // Вниз (нижний синий)
                spriteRenderer.sprite = SSide;
            else if (angle >= -67.5f && angle < -22.5f)     // Право-вниз (правый фиолетовый)
                spriteRenderer.sprite = SESide;
        }
        
        bool IsOutOfBounds()
        {
            // Преобразуем позицию объекта в координаты экрана
            Vector3 screenPos = Camera.main.WorldToViewportPoint(transform.position);

            // Проверяем, если объект находится за пределами экрана
            if (screenPos.x < 0 || screenPos.x > 1 || screenPos.y < 0 || screenPos.y > 1)
            {
                return true; // Объект за пределами экрана
            }

            return false; // Объект внутри экрана
        }
    }
}
 