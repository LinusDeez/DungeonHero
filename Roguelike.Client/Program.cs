using SadConsole;
using SadConsole.Configuration;
using Roguelike.Client;
using Roguelike.Core;

Settings.WindowTitle = "Roguelike";

var player = new Player();
var map = new MapGenerator(80, 48).CreateMap();
map.SetActorPosition(player, player.X, player.Y);
var gameState = new GameState(map, player);
var commands = new CommandSystem(gameState);

Builder config = new Builder()
    .SetWindowSizeInCells(100, 70)
    .SetStartingScreen(host => new RootScreen(gameState, commands))
    .IsStartingScreenFocused(true)
    .SetDefaultFontSize(IFont.Sizes.Half);


Game.Create(config);
Game.Instance.Run();
Game.Instance.Dispose();
