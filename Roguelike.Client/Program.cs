using SadConsole;
using SadConsole.Configuration;
using Roguelike.Client;
using Roguelike.Core;
using RogueSharp.Random;

int seed = Environment.TickCount;
var random = new DotNetRandom(seed);
Settings.WindowTitle = $"Roguelike - Seed {seed}";

var generator = new MapGenerator(80, 48, 20, 13, 7, random);
var map = generator.CreateMap();

var player = new Player();
map.PlacePlayer(player);
generator.PlaceMonsters(map);


var gameState = new GameState(map, player);
gameState.Messages.Add("The rogue arrives on level 1");
gameState.Messages.Add($"Level created with seed '{seed}'");
var commands = new CommandSystem(gameState, random);
commands.ActivateMonsters();

string fontPath = Path.Combine(AppContext.BaseDirectory, "Fonts", "Cheepicus12.font");

Builder config = new Builder()
    .SetWindowSizeInCells(100, 70)
    .SetStartingScreen(host => new RootScreen(gameState, commands))
    .IsStartingScreenFocused(true)
    .ConfigureFonts((fonts, host) => fonts.UseCustomFont(fontPath));

Game.Create(config);
Game.Instance.Run();
Game.Instance.Dispose();
