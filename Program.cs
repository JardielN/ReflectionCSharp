using System;
using System.Reflection;
using Calculator;
using Microsoft.CSharp.RuntimeBinder;

namespace ClientApp
{
    public class Program
    {
        static void Main(string[] args)
        {
            const string CalculatorTypeName = "Calculator.Calculator";

            if(args.Length != 1)
            {
                ShowUsage();
                return;
            }

            UsingReflection(args[0]);

            void ShowUsage()
            {
                Console.WriteLine($"Usage: {nameof(ClientApp)} path");
                Console.WriteLine();
                Console.WriteLine("copy Calculator.dll to an addin directory");
                Console.WriteLine("and pass the absolute path of this directory" +
                    "when starting the application to load the library");
            }

            // Crear una instancia de tipo Calculator. El metodo
            // getCalculator() carga el assembly dinamicamente 
            // usando el metodo LoadFile de la clase Assembly
            // y crea una instancia con el metodo CreateInstance()
            object? GetCalculator(string addinPath)
            {
                Assembly assembly = Assembly.LoadFile(addinPath);
                return assembly.CreateInstance(CalculatorTypeName);
            }

            void UsingReflection(string addinPath)
            {
                double x = 3;
                double y = 4;
                object calc = GetCalculator(addinPath)
                    ?? throw new InvalidOperationException("GetCalculator returned null");

                object? result = calc.GetType().GetMethod("Add")
                    ?.Invoke(calc, new object[] { x, y })
                    ?? throw new InvalidOperationException("Add method not found");
                Console.WriteLine($"The result of {x} and {y} is {result}");
            }
        }
    }
}
