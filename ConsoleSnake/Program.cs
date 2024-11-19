// See https://aka.ms/new-console-template for more information

using ConsoleSnake;

// 画布
var board= new Board();
// 蛇 
var snake= new Snake(board);
// 让蛇跑
snake.RunAsync();


// while (true)
// {
//         var key = Console.ReadKey(true);
//         Console.WriteLine(key.Key);
//         if (IsEsapeKey(key)) 
//             return;
//         // 判断是否按了 Esc 按键
//         snake.KeyPressed(key);
// }
// return;
// // window
// static bool IsEsapeKey(ConsoleKeyInfo key)=> key.Key == ConsoleKey.Escape;

while (true)
{
    // 使用 try-catch 来处理可能的平台差异
    try 
    {
        var key = Console.ReadKey(true);
        // 添加调试信息
        Console.WriteLine($"按下的键: {key.Key}, KeyChar: {key.KeyChar}");
        
        if (IsEscapeKey(key)) 
        {
            break; // 使用 break 替代 return 更清晰
        }
        
        snake.KeyPressed(key);
    }
    catch (InvalidOperationException ex)
    {
        Console.WriteLine($"读取按键时出错: {ex.Message}");
        break;
    }
}

// 修改检测 Escape 键的方法，增加额外判断
static bool IsEscapeKey(ConsoleKeyInfo key) => 
    key.Key == ConsoleKey.Escape || 
    key.KeyChar == '\u001b'; // ESC 字符的 Unico
                             // de 值