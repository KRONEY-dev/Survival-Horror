public interface IUIElement
{
    void Init();
    void Show();
    void Show(object data);
    void Hide();
    void Update();
    void Dispose();
}