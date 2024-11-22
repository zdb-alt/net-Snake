// See https://aka.ms/new-console-template for more information

using ConsoleSnake;

// 去掉 console 后面的 光标
Console.CursorVisible = false;
// 清空画布
Console.Clear();
// CancellationToken
var cts = new CancellationTokenSource();
// 画布
var board= new Board();
// 蛇 
var snake= new Snake(board, cts);
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
        // Console.WriteLine($"按下的键: {key.Key}, KeyChar: {key.KeyChar}");
        
        if (IsEscapeKey(key)) 
        {
            cts.Cancel();
            // 等待 1 秒钟 结束掉程序
            await Task.Delay(21000);
            // 使用 break 替代 return 更清晰
            break; 
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