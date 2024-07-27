namespace CheatSheet.Examples;

public class Threads : IDemoSheet
{
    public void RunDemo()
    {
        Console.WriteLine("Hello from threads!\n");

        Thread thread1 = new Thread(new ThreadStart(PrintHello));
        Thread thread2 = new Thread(new ThreadStart(() => PrintName("Bob")));
        thread1.Start();
        thread2.Start();

        // There is also the Task class
        // Task.Run can be awaited
        Task.Run(() => PrintName("Tom"));

        var threads = new List<Thread>();
        for (int i = 1; i <= 5; i++)
        {
            // These should print randomly
            var printer = new DelayedPrint(i);
            Thread t = new Thread(printer.Print);
            threads.Add(t);
            t.Start();
        }

        while (threads.Any(t => t.ThreadState != ThreadState.Stopped)) { }
        Console.WriteLine("Finished delay prints");

        threads.Clear();
        for (int i = 1; i <= 5; i++)
        {
            // While complete order is not guaranteed,
            // ID 1 will always print first
            // This does seem run in order quite consistently though
            var printer = new DelayedPrintWithLock(i);
            Thread t = new Thread(printer.Print);
            threads.Add(t);
            t.Start();
        }

        while (threads.Any(t => t.ThreadState != ThreadState.Stopped)) { }
        Console.WriteLine("Finished with lock delayed prints");
    }

    private void PrintHello()
    {
        Console.WriteLine($"Hello world!");
    }

    private void PrintName(string name)
    {
        Console.WriteLine($"Hello, {name}");
    }


}

public class DelayedPrint
{
    private int _id;
    private TimeSpan _delay;
    public DelayedPrint(int id)
    {
        _id = id;
        _delay = TimeSpan.FromMilliseconds(new Random().Next(200, 500));
    }

    public void Print()
    {
        Thread.Sleep(_delay);
        Console.WriteLine($"Hello from id {_id} with a delay of {_delay.Milliseconds}");
    }
}

public class DelayedPrintWithLock
{
    private static object _lock = new object();
    private int _id;
    private TimeSpan _delay;
    public DelayedPrintWithLock(int id)
    {
        _id = id;
        _delay = TimeSpan.FromMilliseconds(new Random().Next(200, 500));
    }

    public void Print()
    {
        lock (_lock)
        {
            Thread.Sleep(_delay);
            Console.WriteLine($"Hello from id {_id} with a delay of {_delay.Milliseconds}");
        }
    }
}
