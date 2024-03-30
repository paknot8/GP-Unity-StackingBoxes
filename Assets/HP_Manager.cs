using UnityEngine;

public class Health : MonoBehaviour
{
    [SerializeField] private Game_Manager gameManager;

    private void OnTriggerEnter2D(Collider2D collision)
    {
        gameManager.SubstractLifePoint();
    }   
}
