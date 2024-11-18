// See https://aka.ms/new-console-template for more information

Console.WriteLine("Hello, Snake!");

var board= new Board();
var snake= new Snake(board);
snake.RunAsync();

while (true)
{
    var key = Console.ReadKey(true);
    if (IsEsapeKey(key)) 
        return;
    // 判断是否按了 esc 按键
    snake.KeyPressed(key);
}

return;
static bool IsEsapeKey(ConsoleKeyInfo key)=> key.Key == ConsoleKey.Escape;