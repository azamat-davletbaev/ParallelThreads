using ParallelCalc;
using System.Drawing;

Console.WriteLine("Домашнее задание по теме: Многопоточный проект");

// делегат, обертка для вызова методов расчета
Action<Func<int, (int Sum, long Milliseconds)>, int, string> calcAction = (func, count, message) =>
{
    var result = func.Invoke(count);
    Console.WriteLine($"{message} (всего итераций {count*100000}) => {result.Sum} : {result.Milliseconds} мсек");
};

//количество итераций расчета х100_000
int k1 = 1;
int k100 = 100;
int k1000 = 1000;

var calc = new Calc();

Console.WriteLine($"Инициализирован массив из {calc.items.Count()} элементов");

calcAction(calc.GetSum, k1, "Обычное суммирование");
calcAction(calc.GetSum, k100, "Обычное суммирование");
calcAction(calc.GetSum, k1000, "Обычное суммирование");

calcAction(calc.GetSumByLINQ, k1, "Суммирование AsParallel().Sum()");
calcAction(calc.GetSumByLINQ, k100, "Суммирование AsParallel().Sum()");
calcAction(calc.GetSumByLINQ, k1000, "Суммирование AsParallel().Sum()");

calcAction(calc.GetSumByThread, k1, "Cуммирование через Threads");
calcAction(calc.GetSumByThread, k100, "Cуммирование через Threads");
calcAction(calc.GetSumByThread, k1000, "Cуммирование через Threads");

Console.ReadLine();



    