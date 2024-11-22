namespace ConsoleSnake;

public class Snake
{
    // 常量定义
    private const int MinSnakeLength = 4;    // 蛇的最小长度
    private const int MaxSnakeLength = 6;    //  蛇的最大长度（达到即获胜）
    
    
    // 核心属性
    private readonly Board _board;   // 游戏板
    private LinkedList<Point> Body { get; } = [];
    private Point _foodPlace;    // 食物位置
    private LinkedList<Direction> KeyList { get; } = [];
    private readonly TimeSpan _moveDelay = TimeSpan.FromMilliseconds(150);    // 移动间隔
    private readonly CancellationTokenSource _cts;

    public Snake(Board board, CancellationTokenSource cts)
    {
        _board = board;
        _cts = cts;
        // 初始化蛇身，放在屏幕中央
        for (int i = 0; i < MinSnakeLength; i++)
            Body.AddLast(new Point(_board.Width / 2 + i, _board.Height / 2));
        // 绘制蛇身
        foreach (var point in Body) _board.WriteAt(point);
        // 生成初始食物
        _foodPlace = PutFoodRandomly();
        // 设置初始移动方向
        KeyList.AddLast(Direction.Left);


    }

    // 食物生成逻辑
    private Point PutFoodRandomly()
    {
        Point foodPoint;
        do
        {
            foodPoint = new Point(
                new Random().Next(0, _board.Width), // X坐标：0 到 游戏板宽度
                new Random().Next(0, _board.Height)); // Y坐标：0 到 游戏板高度
        } while (Body.Contains(foodPoint));  // 检查是否与蛇身重叠

        // 在控制台绘制食物
        Console.SetCursorPosition(foodPoint.X, foodPoint.Y);
        Console.WriteLine("X");
        return foodPoint;
    }

    // 游戏主循环
    public async void RunAsync()
    {
        while (true)
        {
            if (_cts.IsCancellationRequested)
            {
                Console.WriteLine("Snake stop 掉了");
                return;
            
            }
            // 获取移动方向
            var way = Direction.Left;
            if (KeyList.Count > 0)
            {
                way = KeyList.First!.Value; // 一定有 First
                KeyList.RemoveFirst();
            }
            // 移动并检查结果
            var dead = Move(way);
            // 检查游戏结束条件
            if (dead)
            {
                Console.WriteLine("Game Over");
                
                // 结束程序
                Environment.Exit(0);
            }
            // 检查胜利条件
            if (Body.Count >= MaxSnakeLength)
            {
                Console.WriteLine("You win");
                Environment.Exit(0);
            }
            // 保持移动方向
            if (KeyList.Count == 0) KeyList.AddLast(way);
            await Task.Delay(_moveDelay);

        }
    }

    // 移动机制
    private bool Move(Direction way)
    {
        // 根据方向计算下一个位置
        var headingPoint = way switch
        {
            Direction.Up => new Point(Body.First!.Value.X, Body.First.Value.Y - 1),
            Direction.Down => new Point(Body.First!.Value.X, Body.First.Value.Y + 1),
            Direction.Left => new Point(Body.First!.Value.X - 1, Body.First.Value.Y),
            Direction.Right => new Point(Body.First!.Value.X + 1, Body.First.Value.Y),
            _ => throw new ArgumentOutOfRangeException(nameof(way), way, null)
        };
        return MoveResult(headingPoint);

    }

    // 移动结果处理
    private bool MoveResult(Point point)
    {

        if(HitWall(point)) return true;  // 撞墙检测
        if(EatSelf(point)) return true;  // 自噬检测
        if (EatFood(point)) return false; // 吃到食物
        MoveSafely(point);// 正常移动
        return false;

    }

    // 具体移动实现
    private void MoveSafely(Point point)
    {
        // 在头部添加新位置
        Body.AddFirst(point);
        Console.SetCursorPosition(point.X,point.Y);
        Console.Write("@");

        // 删除尾部
        var last = Body.Last!.Value;
        Body.RemoveLast();
        Console.SetCursorPosition(last.X,last.Y);
        Console.Write(" ");
    }

    private bool EatFood(Point point)
    {
        if (point.Equals(_foodPlace))
        {
            Body.AddFirst(point);
            Console.SetCursorPosition(point.X,point.Y);
            Console.WriteLine("@");
            _foodPlace = PutFoodRandomly();
            return true;
        }

        return false;
    }
    
    private bool EatSelf(Point point) => Body.Contains(point);

    private bool HitWall(Point point)
    {
        if (point.X < 0 || point.X >= _board.Width ||
            point.Y <0 || point.Y >= _board.Height ) return true;
        return false;
    }


    // 按键处理
    public void KeyPressed(ConsoleKeyInfo key)
    {
        // 将按键转换为方向并加入队列
        if (key.Key == ConsoleKey.LeftArrow)
        {
            KeyList.AddLast(Direction.Left);
        }else if (key.Key == ConsoleKey.RightArrow)
        {
            KeyList.AddLast(Direction.Right);
        }else if (key.Key == ConsoleKey.UpArrow)
        {
            KeyList.AddLast(Direction.Up);
        }else if (key.Key == ConsoleKey.DownArrow)
        {
            KeyList.AddLast(Direction.Down);
        }
    }
}