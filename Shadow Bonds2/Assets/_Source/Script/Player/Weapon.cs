using UnityEngine;

public class Weapon : MonoBehaviour
{
    [SerializeField] private SpriteRenderer weaponSpriteRenderer; // Компонент SpriteRenderer оружия
    [SerializeField] private Sprite upWeaponSprite;
    [SerializeField] private Sprite downWeaponSprite;
    [SerializeField] private Sprite leftWeaponSprite;
    [SerializeField] private Sprite rightWeaponSprite;

    public void SetDirection(Vector3 direction)
    {
        // Смена спрайта оружия в зависимости от направления
        if (Mathf.Abs(direction.y) > Mathf.Abs(direction.x)) // Больше движение по вертикали
        {
            if (direction.y > 0)
                weaponSpriteRenderer.sprite = upWeaponSprite; // Оружие смотрит вверх
            else
                weaponSpriteRenderer.sprite = downWeaponSprite; // Оружие смотрит вниз
        }
        else // Больше движение по горизонтали
        {
            if (direction.x > 0)
                weaponSpriteRenderer.sprite = rightWeaponSprite; // Оружие смотрит вправо
            else
                weaponSpriteRenderer.sprite = leftWeaponSprite; // Оружие смотрит влево
        }
    }
}
