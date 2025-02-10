using System;
using _Source.Script.Game;
using UnityEngine;

namespace _Source.Script.Player
{
    public class PlayerMovment : MonoBehaviour
    {
        [SerializeField] private Weapon weapon;
        
        [Header("Player Settings")]
        [SerializeField] private float moveSpeed = 5f; // Скорость перемещения игрока

        [Header("Animator Settings")]
        [SerializeField] private Animator animator;

        [Header("Camera Settings")]
        [SerializeField] private Transform cameraTransform; // Ссылка на трансформ камеры
        [SerializeField] private Vector3 cameraOffset = new Vector3(0, 0, -10); // Смещение камеры относительно игрока

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
            weapon.ismoving = rb.velocity.magnitude > 0.1f;

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
                rb.velocity = movement.normalized * moveSpeed; // Используем переменную moveSpeed
            }
            else if (!isMousePressed)
            {
                rb.velocity = Vector2.zero; // Останавливаем движение, если мышь не нажата
            }
        }

        private void MovePlayer()
        {
            // Двигаем игрока по физике
            rb.velocity = movement.normalized * moveSpeed; // Используем переменную moveSpeed
        }

        private void UpdatePlayerDirection()
        {
            // Поворот игрока в зависимости от направления движения
            if (!isMousePressed)
            {
                if (movement.y > 0)
                {
                    animator.Play("walk up crossbow");
                }
                else if (movement.y < 0)
                {
                    animator.Play("walk down crossbow");
                }
                else if (movement.x > 0)
                {
                    animator.Play("walk right crossbow");
                }
                else if (movement.x < 0)
                {
                    animator.Play("walk left crossbow");
                    Debug.Log("left");
                }
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
                    animator.Play("walk right crossbow");
                else
                    animator.Play("walk left crossbow");
            }
            else
            {
                if (directionToMouse.y > 0)
                    animator.Play("walk up crossbow");
                else
                    animator.Play("walk down crossbow");
            }

            // Когда ЛКМ нажата, движение игрока запрещено
            canMove = false;
        }

        private void EnableMovement()
        {
            // Разрешаем движение, когда отпускаем клавишу для движения
            canMove = true;
        }

        private void LateUpdate()
        {
            // Восстанавливаем движение, если мышь отпущена
            if (!isMousePressed)
            {
                canMove = true; // Разрешаем движение, когда мышь отпущена
            }

            // Камера следит за игроком, добавляем смещение
            if (cameraTransform != null)
            {
                cameraTransform.position = transform.position + cameraOffset;
            }
        }

        private void SetDirection(string direction)
        {
            // Устанавливаем параметры в Animator для смены направления
            animator.SetTrigger(direction);
        }
        
        private void Rotates(float yAngle)
        {
            // Вращаем объект вокруг оси Z на заданный угол
            transform.Rotate(0, yAngle, 0);
        }
        
        private void UpdateAnimator()
        {
            // Обновляем параметры анимации в зависимости от состояния игрока
            animator.SetBool("IsMoving", isMoving);

            if (movement.x != 0 || movement.y != 0)
            {
                animator.SetFloat("MoveX", movement.x);
                animator.SetFloat("MoveY", movement.y);
            }
        }
    }
}
