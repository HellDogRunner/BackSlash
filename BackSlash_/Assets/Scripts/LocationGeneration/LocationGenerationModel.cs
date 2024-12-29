public class LocationGenerationModel
{
    public int PlatformCount { get; set; } = 10; // ���������� ��������
    public float AreaSize { get; set; } = 50f; // ������ ������� ���������
    public float MinHeight { get; set; } = -5f; // ����������� ������ ���������
    public float MaxHeight { get; set; } = 5f; // ������������ ������ ���������
    public int Seed { get; set; } = 0; // ��� ��� ��������� ���������
}
