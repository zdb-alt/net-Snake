public class Board
{
    // 自动实现的属性，当创建实例之后属相自动绑定
    public int Height { get; set; }

    public int Width { get; set; }

    // 构造函数
    public Board()
    {
        // 设置宽度、高度
        Width = Console.WindowWidth;
        Height = Console.WindowHeight;
    }
}