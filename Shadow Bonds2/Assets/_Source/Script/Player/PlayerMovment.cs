using System;
using _Source.Script.Game;
using UnityEngine;

namespace _Source.Script.Player
{
    public class PlayerMovment : MonoBehaviour
    { 
        [SerializeField] private SpriteRenderer playerSpriteRenderer; // SpriteRenderer игрока

        [Header("Player Sprites")]
        [SerializeField] private Sprite spriteUp;    // Спрайт вверх
        [SerializeField] private Sprite spriteDown;  // Спрайт вниз
        [SerializeField] private Sprite spriteLeft;  // Спрайт влево
        [SerializeField] private Sprite spriteRight; // Спрайт вправо

        private Rigidbody2D rb;
        private Vector2 movement;
        private bool isMousePressed; // Проверка нажатия ЛКМ
        private bool isMoving; // Флаг, показывающий, что игрок двигается
        private bool canMove; // Флаг, разрешающий движение (используется для проверки нажатия клавиш)

        private void Start()
        {
            rb = GetComponent<Rigidbody2D>();
            canMove = true; // Игрок может двигаться при старте
        }

        private void Update()
        {
            // Проверка нажатия ЛКМ
            isMousePressed = Input.GetMouseButton(0);

            // Если мышь нажата, останавливаем движение и поворачиваем игрока в сторону мыши
            if (isMousePressed)
            {
                StopMovementAndLookAtMouse();
            }
            else
            {
                // Если клавиша для движения нажата и движение разрешено, двигаем игрока
                if (canMove && (Input.GetAxisRaw("Horizontal") != 0 || Input.GetAxisRaw("Vertical") != 0))
                {
                    movement.x = Input.GetAxisRaw("Horizontal");
                    movement.y = Input.GetAxisRaw("Vertical");
                    isMoving = true;
                }
                else
                {
                    isMoving = false;
                }

                if (isMoving)
                {
                    MovePlayer();
                    UpdatePlayerDirection();
                }
            }
        }

        private void FixedUpdate()
        {
            // Если движение разрешено, двигаем игрока по физике
            if (isMoving && !isMousePressed)
            {
                rb.velocity = movement.normalized * 5f; // Скорость = 5
            }
            else if (!isMousePressed)
            {
                rb.velocity = Vector2.zero; // Останавливаем движение, если мышь не нажата
            }
        }

        private void MovePlayer()
        {
            // Двигаем игрока по физике
            rb.velocity = movement.normalized * 5f;
        }

        private void UpdatePlayerDirection()
        {
            // Поворот игрока в зависимости от направления движения
            if (!isMousePressed)
            {
                if (movement.y > 0)
                    playerSpriteRenderer.sprite = spriteUp; // Вверх
                else if (movement.y < 0)
                    playerSpriteRenderer.sprite = spriteDown; // Вниз
                else if (movement.x > 0)
                    playerSpriteRenderer.sprite = spriteRight; // Вправо
                else if (movement.x < 0)
                    playerSpriteRenderer.sprite = spriteLeft; // Влево
            }
        }

        private void StopMovementAndLookAtMouse()
        {
            // Останавливаем движение и поворачиваем игрока к мыши
            rb.velocity = Vector2.zero;

            Vector3 mousePosition = Camera.main.ScreenToWorldPoint(Input.mousePosition);
            Vector2 directionToMouse = (mousePosition - transform.position).normalized;

            // Поворот игрока в сторону мыши
            if (Mathf.Abs(directionToMouse.x) > Mathf.Abs(directionToMouse.y))
            {
                if (directionToMouse.x > 0)
                    playerSpriteRenderer.sprite = spriteRight; // Вправо
                else
                    playerSpriteRenderer.sprite = spriteLeft; // Влево
            }
            else
            {
                if (directionToMouse.y > 0)
                    playerSpriteRenderer.sprite = spriteUp; // Вверх
                else
                    playerSpriteRenderer.sprite = spriteDown; // Вниз
            }

            // Когда ЛКМ нажата, движение игрока запрещено
            canMove = false;
        }

        // Когда отпускаем клавишу для движения (например, "W", "A", "S", "D"), разрешаем движение
        private void EnableMovement()
        {
            canMove = true;
        }

        private void LateUpdate()
        {
            // Восстанавливаем движение, если мышь отпущена
            if (!isMousePressed)
            {
                canMove = true; // Разрешаем движение, когда мышь отпущена
            }
        }
    }
}
