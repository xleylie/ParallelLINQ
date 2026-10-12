using System.Diagnostics;
using System.Timers;

var numbers = Enumerable.Range(1, 5_000_000);

long HeavyCalculation(int number)
{
    long result = 0;

    for (int i = 1; i <= 10_000; i++)
    {
        result += (number * i) % 1234567;
    }

    return result;
}

//LINQ
Stopwatch sw1 = Stopwatch.StartNew();

var results1 = numbers
    .Select(HeavyCalculation)
    .ToList();

sw1.Stop();
long linqTime = sw1.ElapsedMilliseconds;

//PLINQ
Stopwatch sw2 = Stopwatch.StartNew();

var results2 = numbers
    .AsParallel()
    .Select(HeavyCalculation)
    .ToList();

sw2.Stop();
long plinqTime = sw2.ElapsedMilliseconds;


Console.WriteLine("========== PERFORMANCE ==========");
Console.WriteLine($"LINQ: {sw1.ElapsedMilliseconds} ms");
Console.WriteLine($"PLINQ: {sw2.ElapsedMilliseconds} ms");

double Speedup = (double)linqTime / plinqTime;
Console.WriteLine($"Speedup: {Speedup} x");

//PLINQ Experiment
Stopwatch sw3 = Stopwatch.StartNew();

var results3 = numbers
    .AsParallel()
    .WithDegreeOfParallelism(2)
    .Select(HeavyCalculation)
    .ToList();

sw2.Stop();
long plinqTime2 = sw3.ElapsedMilliseconds;


Stopwatch sw4 = Stopwatch.StartNew();

var results4 = numbers
    .AsParallel()
    .WithDegreeOfParallelism(4)
    .Select(HeavyCalculation)
    .ToList();

sw2.Stop();
long plinqTime3 = sw4.ElapsedMilliseconds;

Stopwatch sw5 = Stopwatch.StartNew();

var results5 = numbers
    .AsParallel()
    .WithDegreeOfParallelism(8)
    .Select(HeavyCalculation)
    .ToList();

sw2.Stop();
long plinqTime4 = sw5.ElapsedMilliseconds;


Console.WriteLine("==========PLINQ PERFORMANCE ==========");
Console.WriteLine($"PLINQ.WithDegreeOfParallelism(2): {sw2.ElapsedMilliseconds} ms");
Console.WriteLine($"PLINQ.WithDegreeOfParallelism(4): {sw2.ElapsedMilliseconds} ms");
Console.WriteLine($"PLINQ.WithDegreeOfParallelism(8): {sw2.ElapsedMilliseconds} ms");

//Order experiment
var numbers1 = Enumerable.Range(1, 100);

var result1 = numbers1
    .AsParallel()
    .Select(x => x)
    .ToList();
foreach (var result in result1) { Console.WriteLine(result); }

var result2 = numbers1
    .AsParallel()
    .AsOrdered()
    .Select(x => x)
    .ToList();

foreach (var result in result2) { Console.WriteLine(result); }
/*PLINQ может менять порядок, так как вычисления производятся на разных потоках
параллельно. Разные потоки могут закончить свою работу в разное время, соответственно
исходный порядок не сохраняется.
*/

/*AsOrdered() нужен, чтобы гарантировать, что PLINQ вернет нам данные в исходном порядке
*/

/*Сохранение порядка требует дополнительных ресурсов и координации между потоками,
что может увеличить время выполнения и потребление памяти, поэтому PLINQ может работать медленнее
*/