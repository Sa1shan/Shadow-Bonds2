using UnityEngine;

namespace _Source.Script.Player
{
    public class Weapon : MonoBehaviour
    { 
        [SerializeField] private GameObject bulletPrefab; // Префаб пули
        [SerializeField] private Transform shotPoint; // Точка, из которой будет вылетать пуля

        [SerializeField] private SpriteRenderer playerSpriteRenderer; // SpriteRenderer игрока
        [SerializeField] private SpriteRenderer weaponSpriteRenderer; // SpriteRenderer оружия

        [Header("Weapon Sprites")]
        [SerializeField] private Sprite weaponUpSprite;    // Спрайт оружия вверх
        [SerializeField] private Sprite weaponDownSprite;  // Спрайт оружия вниз
        [SerializeField] private Sprite weaponLeftSprite;  // Спрайт оружия влево
        [SerializeField] private Sprite weaponRightSprite; // Спрайт оружия вправо

        private Vector3 _defaultWeaponPosition; // Стандартная позиция оружия

        private void Start()
        {
            _defaultWeaponPosition = transform.localPosition;
        }

        private void Update()
        {
            // Проверяем нажатие на кнопку для стрельбы
            if (Input.GetButtonDown("Fire1"))
            {
                Shoot();
            }

            UpdateWeaponSprite();
        }

        private void UpdateWeaponSprite()
        {
            // Проверяем текущий спрайт игрока и меняем спрайт оружия
            if (playerSpriteRenderer.sprite == weaponUpSprite)
            {
                weaponSpriteRenderer.sprite = weaponUpSprite;
            }
            else if (playerSpriteRenderer.sprite == weaponDownSprite)
            {
                weaponSpriteRenderer.sprite = weaponDownSprite;
            }
            else if (playerSpriteRenderer.sprite == weaponLeftSprite)
            {
                weaponSpriteRenderer.sprite = weaponLeftSprite;
            }
            else if (playerSpriteRenderer.sprite == weaponRightSprite)
            {
                weaponSpriteRenderer.sprite = weaponRightSprite;
            }
        }

        private void Shoot()
        {
            // Создаем пулю в точке выстрела
            GameObject bullet = Instantiate(bulletPrefab, shotPoint.position, shotPoint.rotation);

            // Получаем позицию мыши
            Vector3 mousePosition = Camera.main.ScreenToWorldPoint(Input.mousePosition);
            Vector2 direction = (mousePosition - transform.position).normalized;

            // Задаем направление и скорость пули через Rigidbody2D
            Bullet bulletScript = bullet.GetComponent<Bullet>();
            if (bulletScript != null)
            {
                bulletScript.SetDirection(direction);
            }
        }
    }
}
