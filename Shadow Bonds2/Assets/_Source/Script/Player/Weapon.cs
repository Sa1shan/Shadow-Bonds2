using UnityEngine;

namespace _Source.Script.Player
{
    public class Weapon : MonoBehaviour
    {
        [Header("Bullet")]
        [SerializeField] private GameObject bulletPrefab; // Префаб пули
        [SerializeField] private Transform shotPoint; // Точка, из которой будет вылетать пуля
        [SerializeField] private int maxBullets = 12;
        [SerializeField] private int currentBullets = 12;
        
        [Header("Sprite Renderer")]
        [SerializeField] private SpriteRenderer playerSpriteRenderer; // SpriteRenderer игрока
        [SerializeField] private SpriteRenderer weaponSpriteRenderer; // SpriteRenderer оружия

        [Header("Weapon Sprites")]
        [SerializeField] private Sprite weaponUpSprite;    // Спрайт оружия вверх
        [SerializeField] private Sprite weaponDownSprite;  // Спрайт оружия вниз
        [SerializeField] private Sprite weaponLeftSprite;  // Спрайт оружия влево
        [SerializeField] private Sprite weaponRightSprite; // Спрайт оружия вправо

        [HideInInspector] public int CurrentBullets => currentBullets;
        [HideInInspector] public bool ismoving;
        
        private Vector3 _defaultWeaponPosition; // Стандартная позиция оружия
        private float _shotForce = 20f;
        private float _spreadAmount = 0.2f;
        
        private void Start()
        {
            _defaultWeaponPosition = transform.localPosition;
        }

        private void Update()
        {
            // Проверяем нажатие на кнопку для стрельбы
            if (Input.GetButtonDown("Fire1") && currentBullets > 0)
            {
                Shoot();
            }

            if (Input.GetKeyDown(KeyCode.R))
            {
                Reload();
            }

            UpdateWeaponSprite();
        }

        private void UpdateWeaponSprite()
        {
            // Проверяем текущий спрайт игрока и меняем спрайт оружия
            if (playerSpriteRenderer.sprite.name == "sprites_up")
            {
                weaponSpriteRenderer.sprite = weaponUpSprite;
                ResetZPosition(1f); 
            }
            else if (playerSpriteRenderer.sprite.name == "sprites_down")
            {
                weaponSpriteRenderer.sprite = weaponDownSprite;
                ResetZPosition(-0.1f);
            }
            else if (playerSpriteRenderer.sprite.name == "sprites_left")
            {
                weaponSpriteRenderer.sprite = weaponLeftSprite;
                ResetZPosition(-0.1f);
            }
            else if (playerSpriteRenderer.sprite.name == "sprites_right")
            {
                weaponSpriteRenderer.sprite = weaponRightSprite;
                ResetZPosition(-0.1f);
            }
        }
        
        private void ResetZPosition(float zValue)
        {
            // Устанавливаем позицию по z с поддержкой десятичных значений
            Vector3 position = transform.localPosition;
            position.z = zValue;
            transform.localPosition = position;
        }
        
        private void Shoot()
        {
            currentBullets--;

            // Создаем пулю в точке выстрела
            GameObject bullet = Instantiate(bulletPrefab, shotPoint.position, shotPoint.rotation);

            // Получаем позицию мыши
            Vector3 mousePosition = Camera.main.ScreenToWorldPoint(Input.mousePosition);
            Vector2 direction = (mousePosition - transform.position).normalized;


            // Если игрок двигается, добавляем случайный разброс
            if (ismoving)
            {
                direction += new Vector2(Random.Range(-_spreadAmount, _spreadAmount), Random.Range(-_spreadAmount, _spreadAmount));
            }

            // Задаем направление и скорость пули через Rigidbody2D
            Bullet bulletScript = bullet.GetComponent<Bullet>();
            if (bulletScript != null)
            {
                bulletScript.SetDirection(direction);
            }
    
            // Применяем силу пули (если требуется для твоей логики)
            Rigidbody2D bulletRb = bullet.GetComponent<Rigidbody2D>();
            bulletRb.velocity = direction * _shotForce;
        }
        
        void Reload()
        {
            if (currentBullets == 0) // Если нет пуль, начинаем перезарядку
            {
                Debug.Log("Reloading...");
                currentBullets = maxBullets; // Сбрасываем количество пуль
            }
            else
            {
                Debug.Log("No need to reload. Bullets left: " + currentBullets);
            }
        }
    }
}
