using System;
using System.Collections.Generic;

namespace Lab6
{
    // --- 1. ІНТЕРФЕЙСИ ---

    // Інтерфейс для будь-якої функції, яку можна обчислити
    public interface IFunction
    {
        double Calculate(double x);
    }

    // Інтерфейс для об'єктів, які можуть надати інформацію про себе
    public interface IDisplayable
    {
        string GetInfo();
    }

    // --- 2. АБСТРАКТНИЙ КЛАС ---

    // Базовий абстрактний клас, що реалізує інтерфейси
    public abstract class FunctionBase : IFunction, IDisplayable
    {
        // Абстрактні методи, які обов'язково мають бути створені у спадкоємцях
        public abstract double Calculate(double x);
        public abstract string GetInfo();
    }

    // --- 3. КЛАС "ДРОБОВО-ЛІНІЙНА ФУНКЦІЯ" ---

    /// <summary>
    /// Клас функції виду (a1*x + a0) / (b1*x + b0)
    /// </summary>
    public class LinearFractionalFunction : FunctionBase
    {
        // Константа для порівняння з нулем (щоб уникнути проблем з точністю double)
        protected const double Tolerance = 1e-9;

        // Приватні поля (Інкапсуляція: camelCase)
        private double _a1;
        private double _a0;
        private double _b1;
        private double _b0;

        // Публічні властивості (PascalCase)
        public double A1 { get => _a1; set => _a1 = value; }
        public double A0 { get => _a0; set => _a0 = value; }
        public double B1 { get => _b1; set => _b1 = value; }
        public double B0 { get => _b0; set => _b0 = value; }

        // Конструктор за замовчуванням
        public LinearFractionalFunction()
        {
            _b0 = 1; // Уникаємо ділення на 0 за замовчуванням
        }

        // Конструктор з параметрами
        public LinearFractionalFunction(double a1, double a0, double b1, double b0)
        {
            A1 = a1;
            A0 = a0;
            B1 = b1;
            B0 = b0;
        }

        // Реалізація методу обчислення
        public override double Calculate(double x)
        {
            double numerator = A1 * x + A0;
            double denominator = B1 * x + B0;

            // Валідація: Кидаємо виняток замість простого виведення тексту
            if (Math.Abs(denominator) < Tolerance)
            {
                throw new DivideByZeroException($"Знаменник дорівнює нулю при x = {x}");
            }

            return numerator / denominator;
        }

        // Реалізація методу інформації
        public override string GetInfo()
        {
            return $"Дробово-лiнiйна: ({A1}x + {A0}) / ({B1}x + {B0})";
        }
    }

    // --- 4. КЛАС "ДРОБОВА (КВАДРАТИЧНА) ФУНКЦІЯ" ---

    /// <summary>
    /// Дробова функція (квадратична) виду (a2*x^2 + a1*x + a0) / (b2*x^2 + b1*x + b0)
    /// Спадкується від лінійної.
    /// </summary>
    public class FractionalFunction : LinearFractionalFunction
    {
        // Нові поля для квадратичних коефіцієнтів
        private double _a2;
        private double _b2;

        public double A2 { get => _a2; set => _a2 = value; }
        public double B2 { get => _b2; set => _b2 = value; }

        // Конструктор за замовчуванням
        public FractionalFunction() : base() { }

        // Конструктор з усіма параметрами. Викликає конструктор батька (base)
        public FractionalFunction(double a2, double a1, double a0, double b2, double b1, double b0)
            : base(a1, a0, b1, b0)
        {
            A2 = a2;
            B2 = b2;
        }

        // Перевизначення (Override) методу обчислення
        public override double Calculate(double x)
        {
            // Оптимізація: просте множення швидше за Math.Pow
            double x2 = x * x;

            double numerator = A2 * x2 + A1 * x + A0;
            double denominator = B2 * x2 + B1 * x + B0;

            if (Math.Abs(denominator) < Tolerance)
            {
                throw new DivideByZeroException($"Знаменник дорівнює нулю при x = {x}");
            }

            return numerator / denominator;
        }

        // Перевизначення методу інформації
        public override string GetInfo()
        {
            return $"Квадратична: ({A2}x^2 + {A1}x + {A0}) / ({B2}x^2 + {B1}x + {B0})";
        }
    }

    // --- 5. ГОЛОВНА ПРОГРАМА ---

    class Program
    {
        static void Main(string[] args)
        {
            // Вмикаємо українську мову в консолі
            Console.OutputEncoding = System.Text.Encoding.UTF8;
            Console.WriteLine("=== Лабораторна робота 6: Абстракцiя, Iнтерфейси та Полiморфiзм ===\n");

            try
            {
                // ДЕМОНСТРАЦІЯ ПОЛІМОРФІЗМУ
                // Створюємо колекцію абстрактних FunctionBase, куди кладемо різних нащадків
                List<FunctionBase> functions = new List<FunctionBase>
                {
                    // (2x + 5) / (1x - 2)
                    new LinearFractionalFunction(2, 5, 1, -2),

                    // (1x^2 + 2x + 1) / (1x^2 + 0x - 4)
                    new FractionalFunction(1, 2, 1, 1, 0, -4)
                };

                // Точка, в якій шукаємо значення
                double x = 1.0;

                Console.WriteLine($"Обчислення значень при x = {x}:\n");

                foreach (var func in functions)
                {
                    // 1. Виведення інформації (через інтерфейс IDisplayable)
                    Console.WriteLine(func.GetInfo());

                    // 2. Обчислення (через інтерфейс IFunction)
                    // Використовуємо try-catch всередині циклу, щоб помилка в одній функції не зупинила інші
                    try
                    {
                        double result = func.Calculate(x);
                        Console.WriteLine($"Результат: {result:F4}");
                    }
                    catch (DivideByZeroException ex)
                    {
                        Console.ForegroundColor = ConsoleColor.Red;
                        Console.WriteLine($"Помилка: {ex.Message}");
                        Console.ResetColor();
                    }
                    Console.WriteLine(new string('-', 40));
                }

                // --- ДЕМОНСТРАЦІЯ ОБРОБКИ ПОМИЛКИ ---
                Console.WriteLine("\nТест обробки виняткових ситуацiй (x = 2):");
                // Беремо першу функцію: (2x+5)/(x-2). При x=2 знаменник буде 0.
                var errorFunc = functions[0];

                Console.WriteLine(errorFunc.GetInfo());
                Console.WriteLine("Спроба обчислення...");

                // Цей виклик впаде з помилкою, яку ми зловимо нижче
                double val = errorFunc.Calculate(2);
                Console.WriteLine($"Результат: {val}");
            }
            catch (Exception ex)
            {
                // Цей блок зловить DivideByZeroException з останнього тесту
                Console.ForegroundColor = ConsoleColor.Red;
                Console.WriteLine($"\nЗЛОВЛЕНО КРИТИЧНУ ПОМИЛКУ: {ex.Message}");
                Console.ResetColor();
            }

            Console.WriteLine("\nРоботу завершено. Натиснiть Enter...");
            Console.ReadLine();
        }
    }
}
