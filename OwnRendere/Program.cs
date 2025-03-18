using OpenTK.Windowing.Desktop;
using OwnRendere;

GameWindowSettings settings = new GameWindowSettings()
{
    UpdateFrequency = 60
};

NativeWindowSettings windowSettings = new NativeWindowSettings()
{
    Title = "Render",
    ClientSize = new OpenTK.Mathematics.Vector2i(800, 600)
};

Game game = new Game(settings, windowSettings);
game.Run();
