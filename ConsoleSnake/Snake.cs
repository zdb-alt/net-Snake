using ConsoleSnake;

public class Snake
{
    private const int MinSnakeLength = 4;
    private const int MaxSnakeLength = 6;

    private readonly Board _board;

    private readonly LinkedList<Point> _body = new();
    private Point _foodPlace;
    private readonly LinkedList<Direction> _keyList = new();
    private readonly TimeSpan _moveDelay = TimeSpan.FromMilliseconds(150);

    public Snake(Board board)
    {
        _board = board;
        for (int i = 0; i < MinSnakeLength; i++)
            _body.AddLast(new Point(_board.Width / 2 + i, _board.Height / 2));
        // 将 snake 在 console 上 画出来
        foreach (var point in _body)
        {
            Console.SetCursorPosition(point.X, point.Y);
            Console.Write("@");
        }

        // 食物（食物跟蛇不能重叠在一起）
        _foodPlace = PutFoodRandmly();

        _keyList.AddLast(Direction.Left);


    }

    private Point PutFoodRandmly()
    {
        Point foodPoint;
        // 蛇的身体跟食物不能重叠在一起
        do
        {
            foodPoint = new Point(
                new Random().Next(0, _board.Width),
                new Random().Next(0, _board.Height));
        } while (_body.Contains(foodPoint));

        // draw the food on console
        Console.SetCursorPosition(foodPoint.X, foodPoint.Y);
        Console.WriteLine("X");
        return foodPoint;
    }


    public async void RunAsync()
    {
        while (true)
        {
            /**
         * 蛇往前走的四种可能 详见：MoveResult 方法
         */

            // 预设往左边走
            var way = Direction.Left;
            if (_keyList.Count > 0)
            {
                way = _keyList.First!.Value; // 一定有 First
                _keyList.RemoveFirst();
            }


            var dead = Move(way);
            // 蛇死掉 就结束程序
            if (dead)
            {
                Console.WriteLine("Game Over");
                // 结束程序
                Environment.Exit(0);
            }

            // 如果蛇的长度 大于等于 预期的长度。玩家赢
            if (_body.Count >= MaxSnakeLength)
            {
                Console.WriteLine("You win");
                Environment.Exit(0);
            }

            if (_keyList.Count == 0) _keyList.AddLast(way);
            await Task.Delay(_moveDelay);

        }
    }

private bool Move(Direction way)
    {
        var headingPoint = way switch
        {
            Direction.Up => new Point(_body.First!.Value.X, _body.First.Value.Y - 1),
            Direction.Down => new Point(_body.First!.Value.X, _body.First.Value.Y + 1),
            Direction.Left => new Point(_body.First!.Value.X - 1, _body.First.Value.Y),
            Direction.Right => new Point(_body.First!.Value.X + 1, _body.First.Value.Y),
        };
        return MoveResult(headingPoint);

    }

    private bool MoveResult(Point point)
    {
        
        // 撞墙了，死掉了
       if(HitWall(point)) return true;
       // 吃到自己了
       if(EatSelf(point)) return true;
       // 吃到食物了 正常走
       if (EatFood(point)) return false;
       // 继续走
       MoveSafely(point);
       return false;

    }

    private void MoveSafely(Point point)
    {
        _body.AddFirst(point);
        Console.SetCursorPosition(point.X,point.Y);
        Console.Write("@");

        var last = _body.Last!.Value;
        _body.RemoveLast();
        Console.SetCursorPosition(last.X,last.Y);
        Console.Write(" ");
    }

    private bool EatFood(Point point)
    {
        if (point.X==_foodPlace.X && point.Y == _foodPlace.Y)
        {
            _body.AddFirst(point);
            Console.SetCursorPosition(point.X,point.Y);
            Console.WriteLine("@");
            _foodPlace = PutFoodRandmly();
            return true;
        }

        return false;
    }

    private bool EatSelf(Point point) => _body.Contains(point);

    private bool HitWall(Point point)
    {
       if (point.X < 0 || point.X >= _board.Width ||
           point.Y <0 || point.Y >= _board.Height ) return true;
       return false;
    }


    public void KeyPressed(ConsoleKeyInfo key)
    {
        if (key.Key == ConsoleKey.LeftArrow)
        {
            _keyList.AddLast(Direction.Left);
        }else if (key.Key == ConsoleKey.RightArrow)
        {
            _keyList.AddLast(Direction.Right);
        }else if (key.Key == ConsoleKey.UpArrow)
        {
            _keyList.AddLast(Direction.Up);
        }else if (key.Key == ConsoleKey.DownArrow)
        {
            _keyList.AddLast(Direction.Down);
        }
    }
}