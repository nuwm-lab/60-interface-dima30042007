using System;

namespace MathFunctions
{
    // 1. Базовий клас: "Дробово-лінійна функція"
    // Формула: (a1*x + a0) / (b1*x + b0)
    public class LinearFractionalFunction
    {
        // Поля для коефіцієнтів (protected, щоб їх бачив клас-спадкоємець)
        protected double a1, a0;
        protected double b1, b0;

        // Метод 1: Завдання коефіцієнтів
        public virtual void SetCoefficients(double a1, double a0, double b1, double b0)
        {
            this.a1 = a1;
            this.a0 = a0;
            this.b1 = b1;
            this.b0 = b0;
        }

        // Метод 2: Виведення коефіцієнтів на екран (Virtual, щоб можна було змінити у спадкоємця)
        public virtual void Display()
        {
            Console.WriteLine("--- Дробово-лiнiйна функцiя ---");
            Console.WriteLine($"Вигляд: ({a1}x + {a0}) / ({b1}x + {b0})");
            Console.WriteLine($"Коефiцiєнти: a1={a1}, a0={a0}, b1={b1}, b0={b0}");
        }

        // Метод 3: Знаходження значення в точці x0
        public virtual double CalculateValue(double x0)
        {
            double numerator = a1 * x0 + a0;     // чисельник
            double denominator = b1 * x0 + b0;   // знаменник

            // Перевірка ділення на нуль (базова безпека)
            if (denominator == 0)
            {
                Console.WriteLine("Помилка: Дiлення на нуль!");
                return 0;
            }

            return numerator / denominator;
        }
    }

    // 2. Похідний клас: "Дробова функція" (квадратична)
    // Формула: (a2*x^2 + a1*x + a0) / (b2*x^2 + b1*x + b0)
    public class FractionalFunction : LinearFractionalFunction
    {
        // Додаємо нові коефіцієнти для x^2
        protected double a2;
        protected double b2;

        // Перевантажуємо метод завдання коефіцієнтів (приймаємо більше параметрів)
        public void SetCoefficients(double a2, double a1, double a0, double b2, double b1, double b0)
        {
            // Встановлюємо нові
            this.a2 = a2;
            this.b2 = b2;
            // Використовуємо метод батька для встановлення старих (base)
            base.SetCoefficients(a1, a0, b1, b0);
        }

        // Перевизначаємо (Override) метод виведення
        public override void Display()
        {
            Console.WriteLine("--- Дробова функцiя (квадратична) ---");
            Console.WriteLine($"Вигляд: ({a2}x^2 + {a1}x + {a0}) / ({b2}x^2 + {b1}x + {b0})");
            Console.WriteLine($"Коефiцiєнти: a2={a2}, a1={a1}, a0={a0}, b2={b2}, b1={b1}, b0={b0}");
        }

        // Перевизначаємо (Override) метод обчислення
        public override double CalculateValue(double x0)
        {
            // Math.Pow(x0, 2) - це піднесення до квадрату
            double numerator = a2 * Math.Pow(x0, 2) + a1 * x0 + a0;
            double denominator = b2 * Math.Pow(x0, 2) + b1 * x0 + b0;

            if (denominator == 0)
            {
                Console.WriteLine("Помилка: Дiлення на нуль!");
                return 0;
            }

            return numerator / denominator;
        }
    }

    // 3. Головний клас програми для тестування
    class Program
    {
        static void Main(string[] args)
        {
            // --- Тест 1: Дробово-лінійна функція ---
            LinearFractionalFunction linearFunc = new LinearFractionalFunction();

            // Задаємо коефіцієнти: (2x + 5) / (1x + 1)
            linearFunc.SetCoefficients(2, 5, 1, 1);
            linearFunc.Display();

            double x1 = 2;
            Console.WriteLine($"Значення в точцi x={x1}: {linearFunc.CalculateValue(x1)}");
            Console.WriteLine(); // пустий рядок для краси


            // --- Тест 2: Дробова (квадратична) функція ---
            FractionalFunction quadraticFunc = new FractionalFunction();

            // Задаємо коефіцієнти: (1x^2 + 2x + 3) / (1x^2 + 0x + 1)
            quadraticFunc.SetCoefficients(1, 2, 3, 1, 0, 1);
            quadraticFunc.Display();

            double x2 = 2;
            Console.WriteLine($"Значення в точцi x={x2}: {quadraticFunc.CalculateValue(x2)}");

            Console.ReadKey(); // Чекаємо натискання клавіші
        }
    }
}
