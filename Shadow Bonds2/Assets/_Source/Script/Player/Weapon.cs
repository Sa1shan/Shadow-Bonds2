using _Source.Script.Enemy;
using TMPro;
using UnityEngine;

namespace _Source.Script.Player
{
    public class Weapon : MonoBehaviour
    {
        [Header("UI")] [SerializeField] private TextMeshProUGUI tmPro;
        
        [Header("Bullet")]
        [SerializeField] private GameObject bulletPrefab; 
        [SerializeField] private Transform shotPoint; 
        [SerializeField] private int maxBullets = 12;
        [SerializeField] private int currentBullets = 12;
        [SerializeField] private float timeToDestroy;
        [SerializeField] private EnemyDie enemyDie;
        
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
            tmPro.text = $"{currentBullets}";
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
            
            Destroy(bullet, timeToDestroy);
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
