public class LocationGenerationModel
{
    public int PlatformCount { get; set; } = 10; // Количество платформ
    public float AreaSize { get; set; } = 50f; // Размер области генерации
    public float MinHeight { get; set; } = -5f; // Минимальная высота платформы
    public float MaxHeight { get; set; } = 5f; // Максимальная высота платформы
    public int Seed { get; set; } = 0; // Сид для случайной генерации
}
