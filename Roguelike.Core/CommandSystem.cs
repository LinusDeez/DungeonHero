using RogueSharp.Random;

namespace Roguelike.Core;

public class CommandSystem(GameState state, IRandom random)
{
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
            state.Player.Gold += monster.Gold;
            state.Messages.Add($"{monster.Name} dies. You find {monster.Gold} gold.");
        } 
        else if (defender is Player) 
        {
            state.Messages.Add($"You died.");    
        }
        
    }
}
