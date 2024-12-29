using UnityEngine;

public class GameInitializer : MonoBehaviour
{
    [SerializeField] private Transform player; // Ссылка на объект игрока
    [SerializeField] private LocationGenerationModel model;
    [SerializeField] private LocationView view;

    private LocationController controller;

    void Start()
    {
       // controller = new LocationController(model, view);
       // controller.SetPlayer(player); // Передаем игрока в контроллер
       //// controller.GenerateLocation(); // Генерация локации
    }
}
