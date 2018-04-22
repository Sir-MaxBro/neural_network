using System;
using System.Collections.Generic;
using System.Threading.Tasks;

namespace Combine
{
    class Program
    {
        
        static int num = 1;
        static readonly string message = "Введите символы через запятую (или нажмите \"Y\", чтобы выйти):";
        static void Main(string[] args)
        {
            string stroka = StartString();
            while (stroka.ToUpper() != "Y")
            {
                num = 1;
                string[] s = stroka.ToLower().Split(' ');
                int[] a = new int[s.Length];
                try
                {
                    for (int i = 0; i < s.Length; i++)
                    {
                        a[i] = char.Parse(s[i]);
                    }
                }
                catch (SystemException e)
                {
                    Console.WriteLine("Неправильно введены данные. Пожалуйста, введите еще раз:");
                    stroka = StartString();
                    continue;
                }
                Array.Sort(a);
                Print(a);
                while (NextSet(a))
                    Print(a);
                GC.Collect();
                stroka = StartString();
            }
        }

        static string StartString()
        {
            Console.WriteLine(message);
            return Console.ReadLine();
        }

        static void Swap(int[] a, int i, int j)
        {
            int s = a[i];
            a[i] = a[j];
            a[j] = s;
        }
        static bool NextSet(int[] a)
        {
            int n = a.Length;
            int j = n - 2;
            while (j != -1 && a[j] >= a[j + 1]) j--;
            if (j == -1)
                return false; // больше перестановок нет
            int k = n - 1;
            while (a[j] >= a[k]) k--;
            Swap(a, j, k);
            int l = j + 1, r = n - 1; // сортируем оставшуюся часть последовательности
            while (l < r)
                Swap(a, l++, r--);
            return true;
        }

        static void Print(int[] a)  // вывод перестановки
        {
            int n = a.Length;
            Console.Write((num++) + ") ");
            for (int i = 0; i < n; i++)
                Console.Write((char)a[i] + " ");
            Console.WriteLine();
        }        
    }
}
