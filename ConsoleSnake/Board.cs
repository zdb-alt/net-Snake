public class Board
{
    
    public int Height { get; set; }

    public int Width { get; set; }

    public Board()
    {
        Width = Console.WindowWidth;
        Height = Console.WindowHeight;
    }
}