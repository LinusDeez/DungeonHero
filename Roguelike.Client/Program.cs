using SadConsole;
using SadConsole.Configuration;
using Roguelike.Client;
using Roguelike.Core;
using RogueSharp.Random;

int seed = Environment.TickCount;
var random = new DotNetRandom(seed);
Settings.WindowTitle = $"Roguelike - Seed {seed}";

var map = new MapGenerator(80, 48, 20, 13, 7, random).CreateMap();

var player = new Player();
player.X = map.Rooms[0].Center.X;
player.Y = map.Rooms[0].Center.Y;
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
