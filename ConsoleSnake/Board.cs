using ConsoleSnake;

public class Board
{
    // 自动实现的属性，当创建实例之后属相自动绑定
    public int Height { get; set; } = Console.WindowHeight;
    public int Width { get; set; } = Console.WindowWidth;

    public void WriteAt(Point point)
    {
        Console.SetCursorPosition(point.X, point.Y);
        Console.Write("@");
    }
}