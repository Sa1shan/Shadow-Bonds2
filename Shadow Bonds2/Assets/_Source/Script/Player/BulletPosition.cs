using UnityEngine;

namespace _Source.Script.Player
{
    public class Bullet : MonoBehaviour
    {
        [SerializeField] private SpriteRenderer spriteRenderer; // SpriteRenderer для объекта
        [SerializeField] private Sprite spriteUp;   // Спрайт для движения вверх
        [SerializeField] private Sprite spriteDown; // Спрайт для движения вниз
        [SerializeField] private Sprite spriteLeft; // Спрайт для движения влево
        [SerializeField] private Sprite spriteRight;// Спрайт для движения вправо
        
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

        public void SetDirection(Vector2 newDirection)
        {
            _direction = newDirection; // Устанавливаем направление пули
        }
        
        private void Update()
        {
            ChangeSpriteBasedOnMousePosition();
        }
        
        private void ChangeSpriteBasedOnMousePosition()
        {
            Vector3 mousePosition = Camera.main.ScreenToWorldPoint(Input.mousePosition);
            Vector2 directionToMouse = (mousePosition - transform.position).normalized;

            // Поворот спрайта в зависимости от положения мыши
            if (Mathf.Abs(directionToMouse.x) > Mathf.Abs(directionToMouse.y))
            {
                if (directionToMouse.x > 0)
                    spriteRenderer.sprite = spriteRight; // Вправо
                else
                    spriteRenderer.sprite = spriteLeft;  // Влево
            }
            else
            {
                if (directionToMouse.y > 0)
                    spriteRenderer.sprite = spriteUp;   // Вверх
                else
                    spriteRenderer.sprite = spriteDown; // Вниз
            }
        }
    }
}
 