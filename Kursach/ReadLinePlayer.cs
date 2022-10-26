namespace Kursach;

public class ReadLinePlayer : IPlayer
{
    private IKeyboardMapper keyboardMapper;
    public Point GetMove(Game game)
    {
        Console.WriteLine("Сделайте ход");
#pragma warning disable CS8604 // Possible null reference argument.
        return keyboardMapper.Map(Console.ReadLine(), game.CurrentPoint);
#pragma warning restore CS8604 // Possible null reference argument.
    }

    public string GetName()
    {
        return "Человек";
    }

    public Point GetFirstMove(Game game)
    {
        throw new NotImplementedException();
    }

    public ReadLinePlayer(IKeyboardMapper mapper)
    {
        keyboardMapper = mapper;
    } 
}
