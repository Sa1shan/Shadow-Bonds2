using System;
using UnityEngine;

namespace _Source.Script.Enemy
{
    public class Move : MonoBehaviour
    {
        [SerializeField] private GameObject player;  // Игрок
        [SerializeField] private GameObject enemy;   // Враг
        [SerializeField] private float moveSpeed = 2f; // Скорость движения врага
        [SerializeField] private MoveBetweenPoints moveBetweenPoints;
        private bool _isEnter = false;

        private Rigidbody2D _rb;

        private void Start()
        {
            // Получаем ссылку на Rigidbody2D врага
            _rb = enemy.GetComponent<Rigidbody2D>();

            // Изначально включаем движение между точками
            moveBetweenPoints.enabled = true;
        }

        private void OnTriggerEnter2D(Collider2D other)
        {
            if (other.CompareTag("Player"))
            {
                Debug.Log("enter");
                _isEnter = true;
                // Отключаем движение между точками, когда игрок входит в триггер
                moveBetweenPoints.enabled = false;
            }
        }
        
        private void Update()
        {
            if (_isEnter)
            {
                MoveEnemyTowardsPlayer();
            }
        }

        private void MoveEnemyTowardsPlayer()
        {
            // Получаем позицию врага и игрока
            Vector2 enemyPosition = enemy.transform.position;
            Vector2 playerPosition = player.transform.position;

            // Рассчитываем направление от врага к игроку
            Vector2 direction = (playerPosition - enemyPosition).normalized; // Нормализуем, чтобы направление было единичным

            // Устанавливаем скорость движения врага через Rigidbody2D
            _rb.velocity = direction * moveSpeed; // Двигаем врага в сторону игрока с заданной скоростью
        }

    }
}
