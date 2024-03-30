using UnityEngine;

public class Health : MonoBehaviour
{
    [SerializeField] private GameManager gameManager;

    private void OnTriggerEnter2D(Collider2D collision)
    {
        gameManager.SubstractLifePoint();
    }   
}
