using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

namespace _Source.Script.Player
{
    public class Health : MonoBehaviour
    {
        [SerializeField] private GameObject player;
        [SerializeField] private GameObject enemy;
        [SerializeField] private float triggerDistance = 3f; 
        [SerializeField] private float timeInterval = 5f;
        [SerializeField] private List<Image> hearts;
        [SerializeField] private float damage = 5f;
        public float playerhp = 100f;
    
        private float timer = 0f; // Таймер для отсчета времени

        void Update()
        {
            // Отсчитываем время
            timer += Time.deltaTime;

            // Если прошло 5 секунд, проверяем расстояние и применяем урон
            if (timer >= timeInterval)
            {
                if (enemy != null)
                {
                    // Вычисляем расстояние между игроком и врагом
                    float distance = Vector2.Distance(player.transform.position, enemy.transform.position);

                    // Если расстояние меньше triggerDistance, наносим урон игроку
                    if (distance < triggerDistance)
                    {
                        playerhp -= damage;
                        Debug.Log(playerhp);
                    }
                }

                // Сброс таймера
                timer = 0f;
            }
            UpdatePlayerHp();
        }
        
        private void UpdatePlayerHp()
        {
            if (playerhp == 100)
            {
                foreach (var heart in hearts)
                {
                    heart.gameObject.SetActive(true);
                }
            }
            if (playerhp == 80)
            {
                hearts[4].gameObject.SetActive(false);
            }
            if (playerhp == 60)
            {
                hearts[3].gameObject.SetActive(false);
            }
            if (playerhp == 40)
            {
                hearts[2].gameObject.SetActive(false);
            }
            if (playerhp == 20)
            {
                hearts[1].gameObject.SetActive(false);
            }
            if (playerhp == 0)
            {
                hearts[0].gameObject.SetActive(false);
            }
        }
    }
}
