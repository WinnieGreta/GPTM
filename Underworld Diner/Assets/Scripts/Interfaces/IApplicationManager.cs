namespace Interfaces
{
    public interface IApplicationManager
    {
        void LoadGameScene(string name);
        void LoadMainMenu();
        void OnAppExit();
    }
}