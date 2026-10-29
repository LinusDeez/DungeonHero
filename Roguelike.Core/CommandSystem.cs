using RogueSharp;
using RogueSharp.Random;

namespace Roguelike.Core;

public class CommandSystem(GameState state, IRandom random)
{
    private int CountSuccesses(int dice, int chance)
    {
        int successes = 0;
        for (int i = 0; i < dice; i++)
        {
            if (random.Next(1, 100) >= 100 - chance)
            {
                successes++;
            }
        }
        return successes;
    }

    private void ResolveDeath(Actor defender)
    {
        if (defender is Monster monster)
        {
            state.Map.RemoveMonster(monster);
            state.Schedule.Remove(monster);
            state.Player.Gold += monster.Gold;
            state.Messages.Add($"{monster.Name} dies. You find {monster.Gold} gold.");
        } 
        else if (defender is Player) 
        {
            state.Messages.Add($"You died.");    
        }   
    }

    private void MonsterTurn(Monster monster)
    {
        Player player = state.Player;

        if (!monster.TurnsAlerted.HasValue)
        {
            var monsterFov = new FieldOfView(state.Map);
            monsterFov.ComputeFov(monster.X, monster.Y, monster.Awareness, true);

            if (monsterFov.IsInFov(player.X, player.Y))
            {
                state.Messages.Add($"{monster.Name} is eager to fight {player.Name}.");
                monster.TurnsAlerted = 1;
            }
        }

        if (monster.TurnsAlerted.HasValue)
        {
            MoveTowardsPlayer(monster);
            monster.TurnsAlerted++;

            if (monster.TurnsAlerted > 15)
            {
                monster.TurnsAlerted = null;
            }
        }
    }

    private void MoveTowardsPlayer(Monster monster)
    {
        Player player = state.Player;
        DungeonMap map = state.Map;

        // Free Actor Positions for the PathFinder.
        map.SetIsWalkable(monster.X, monster.Y, true);
        map.SetIsWalkable(player.X, player.Y, true);

        var pathFinder = new PathFinder(map);
        var path = pathFinder.TryFindShortestPath(
            map.GetCell(monster.X, monster.Y),
            map.GetCell(player.X, player.Y)
        );
        
        // Block Actor Positions.
        map.SetIsWalkable(monster.X, monster.Y, false);
        map.SetIsWalkable(player.X, player.Y, false);

        // No way. e.g. another monster blocks the way.
        if (path == null)
        {
            return;
        }

        ICell? next = path.TryStepForward();
        if (next == null)
        {
            return;
        } 
        else if (next.X == player.X && next.Y == player.Y)
        {
            Attack(monster, player);
        } 
        else
        {
            map.SetActorPosition(monster, next.X, next.Y);
        }
    }

    public bool MovePlayer(Direction direction)
    {
        int x = state.Player.X;
        int y = state.Player.Y;

        switch (direction)
        {
            case Direction.Up:
                y = y - 1;
                break;
            case Direction.Down:
                y = y + 1;
                break;
            case Direction.Left:
                x = x - 1;
                break;
            case Direction.Right:
                x = x + 1;
                break;
        }

        Monster? monster = state.Map.GetMonsterAt(x, y);
        if (monster != null)
        {
            Attack(state.Player, monster);
            return true;
        }

        return state.Map.SetActorPosition(state.Player, x, y);
    }

    public void Attack(Actor attacker, Actor defender)
    {
        int hits = CountSuccesses(attacker.Attack, attacker.AttackChance);
        int blocks = CountSuccesses(defender.Defense, defender.DefenseChance);
        int damage = hits - blocks;

        state.Messages.Add($"{attacker.Name} attacks {defender.Name}: {hits} hits, {blocks} blocked.");

        if (damage > 0)
        {
            defender.Health -= damage;
            state.Messages.Add($"{defender.Name} takes {damage} damage.");

            if (defender.Health <= 0)
            {
                ResolveDeath(defender);
            }
        }
        else
        {
            state.Messages.Add($"{defender.Name} blocks all the damage.");
        }
    }

    public void EndPlayerTurn()
    {
        state.Schedule.Add(state.Player);
        ActivateMonsters();
    }

    public void ActivateMonsters()
    {
        while (!state.GameIsOver && state.Schedule.Get() is Monster monster)
        {
            MonsterTurn(monster);
            state.Schedule.Add(monster);
        }
    }
}
