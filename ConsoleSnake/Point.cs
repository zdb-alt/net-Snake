// struct 是一种用户自定义的数据类型，可以把它理解为一个轻量级的类（class）。

using System.Text;

namespace ConsoleSnake;

// public  record struct Point
// {
//     public int Y { get; set; }
//
//     public int X { get; set; }
//     public Point(int x, int y)
//     {
//         X = x;
//         Y = y;
//     }
//
// }




/**
 * record struct 是 C# 10 引入的新特性，它结合了 record 和 struct 的优点：
   record 的特点：自动生成相等性比较和其他有用的方法
   struct 的特点：值类型，性能好
 */
public readonly record struct Point(int X, int Y) // 构造函数参数自动变成属性
{
  public override int GetHashCode()
  {
    return base.GetHashCode();
  }

  // 改写 Equals 方法
  public  bool Equals(Point other)
  {
    if (other is var point)
    {
      return point.X == X && point.Y == Y;
    }

    return false;
  }
}; 


