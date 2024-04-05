using UnityEngine;

public class HealthPointsManager : MonoBehaviour
{
    [SerializeField] private GameManager gameManager;
    private void OnTriggerEnter2D(Collider2D collision) => gameManager.SubtractLifePoint();
}
