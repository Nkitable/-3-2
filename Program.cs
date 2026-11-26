using System;
using System.Collections.Generic;
using System.IO;
using System.Text;

namespace Lab3_Var2
{
    public class TextProcessor
    {
        private static readonly HashSet<char> Vowels = new HashSet<char>(
            "аеєиіїоуюяАЕЄИІЇОУЮЯaeiouyAEIOUY"
        );

        public static string RemoveEverySecondWord(string text)
        {
            if (string.IsNullOrWhiteSpace(text))
                return string.Empty;

            string[] words = text.Split((char[]?)null, StringSplitOptions.RemoveEmptyEntries);
            var result = new List<string>();

            for (int i = 0; i < words.Length; i++)
            {
                if ((i + 1) % 2 != 0)
                    result.Add(words[i]);
            }

            return string.Join(" ", result);
        }

        public static string RemoveWordsStartingAndEndingWithSameLetter(string text)
        {
            if (string.IsNullOrWhiteSpace(text))
                return string.Empty;

            string[] words = text.Split((char[]?)null, StringSplitOptions.RemoveEmptyEntries);
            var result = new List<string>();

            foreach (string word in words)
            {
                var letters = ExtractLetters(word);
                if (letters.Count >= 2 &&
                    char.ToLowerInvariant(letters[0]) == char.ToLowerInvariant(letters[letters.Count - 1]))
                {
                    continue;
                }
                result.Add(word);
            }

            return string.Join(" ", result);
        }

        public static string RemovePalindromes(string text)
        {
            if (string.IsNullOrWhiteSpace(text))
                return string.Empty;

            string[] words = text.Split((char[]?)null, StringSplitOptions.RemoveEmptyEntries);
            var result = new List<string>();

            foreach (string word in words)
            {
                var letters = ExtractLetters(word);
                if (letters.Count >= 2 && IsPalindrome(letters))
                    continue;

                result.Add(word);
            }

            return string.Join(" ", result);
        }

        public static Dictionary<char, int> CountVowels(string text)
        {
            var counts = new Dictionary<char, int>();
            if (string.IsNullOrEmpty(text))
                return counts;

            foreach (char ch in text)
            {
                if (Vowels.Contains(ch))
                {
                    char key = char.ToLower(ch);
                    counts[key] = counts.TryGetValue(key, out int current) ? current + 1 : 1;
                }
            }

            return counts;
        }

        private static bool IsPalindrome(List<char> letters)
        {
            int left = 0;
            int right = letters.Count - 1;
            while (left < right)
            {
                if (char.ToLowerInvariant(letters[left]) != char.ToLowerInvariant(letters[right]))
                    return false;
                left++;
                right--;
            }
            return true;
        }

        private static List<char> ExtractLetters(string s)
        {
            var list = new List<char>();
            foreach (char ch in s)
            {
                if (char.IsLetter(ch))
                    list.Add(ch);
            }
            return list;
        }
    }

    public class Program
    {
        static void Task1()
        {
            Console.WriteLine("\n--- Завдання 1 (Варіант 2) ---");
            Console.Write("Введіть текстовий рядок: ");
            string input = Console.ReadLine() ?? string.Empty;

            string withoutEverySecond = TextProcessor.RemoveEverySecondWord(input);
            string withoutSameStartEnd = TextProcessor.RemoveWordsStartingAndEndingWithSameLetter(input);

            Console.WriteLine($"а) Без кожного другого слова: {withoutEverySecond}");
            Console.WriteLine($"б) Без слів, що починаються і закінчуються на одну літеру: {withoutSameStartEnd}");
        }

        static void Task2()
        {
            Console.WriteLine("\n--- Завдання 2 (Варіант 2: підрахунок голосних у файлі) ---");
            Console.Write("Введіть шлях до текстового файлу (або натисніть Enter для text_task2.txt): ");
            string? path = Console.ReadLine();
            if (string.IsNullOrWhiteSpace(path))
            {
                path = "text_task2.txt";
                if (!File.Exists(path))
                {
                    File.WriteAllText(path, "Привіт, світе! Це тестовий файл для підрахунку голосних літер.", Encoding.UTF8);
                }
            }

            if (!File.Exists(path))
            {
                Console.WriteLine($"Файл '{path}' не знайдено.");
                return;
            }

            string content = File.ReadAllText(path, Encoding.UTF8);
            var counts = TextProcessor.CountVowels(content);

            Console.WriteLine($"Вміст файлу '{path}': {content}");
            Console.WriteLine("Кількість входжень кожної голосної літери:");
            if (counts.Count == 0)
            {
                Console.WriteLine("Голосних літер не знайдено.");
            }
            else
            {
                foreach (var pair in counts)
                {
                    Console.WriteLine($"'{pair.Key}' -> {pair.Value}");
                }
            }
        }

        static void Task3()
        {
            Console.WriteLine("\n--- Завдання 3 (Варіант для завдання 3: 2 + 5 = 7 -> видалення слів-паліндромів) ---");
            string inputFile = "input.txt";
            string outputFile = "output.txt";

            if (!File.Exists(inputFile))
            {
                File.WriteAllText(inputFile, "шалаш програмування дід сонце око місто", Encoding.UTF8);
            }

            string sourceText = File.ReadAllText(inputFile, Encoding.UTF8);
            string filteredText = TextProcessor.RemovePalindromes(sourceText);

            var outBuilder = new StringBuilder();
            outBuilder.AppendLine("Початковий рядок з input.txt:");
            outBuilder.AppendLine(sourceText);
            outBuilder.AppendLine("Результат (після видалення слів-паліндромів):");
            outBuilder.AppendLine(filteredText);

            File.WriteAllText(outputFile, outBuilder.ToString(), Encoding.UTF8);

            Console.WriteLine($"Прочитано з '{inputFile}': {sourceText}");
            Console.WriteLine($"Записано у '{outputFile}': {filteredText}");
        }

        static void Main(string[] args)
        {
            Console.InputEncoding = Encoding.UTF8;
            Console.OutputEncoding = Encoding.UTF8;

            while (true)
            {
                Console.WriteLine("\n================ ЛАБОРАТОРНА РОБОТА №3 (ВАРІАНТ 2) ================");
                Console.WriteLine("1 - Завдання 1 (рядки: видалення кожного 2-го слова та слів з однаковою 1-ю і останньою літерою)");
                Console.WriteLine("2 - Завдання 2 (файл: підрахунок кожної голосної літери)");
                Console.WriteLine("3 - Завдання 3 (input.txt -> output.txt, варіант 7: видалення слів-паліндромів)");
                Console.WriteLine("0 - Вихід");
                Console.Write("Оберіть пункт меню: ");
                string? choice = Console.ReadLine();

                switch (choice)
                {
                    case "1":
                        Task1();
                        break;
                    case "2":
                        Task2();
                        break;
                    case "3":
                        Task3();
                        break;
                    case "0":
                        return;
                    default:
                        Console.WriteLine("Невірний вибір. Спробуйте ще раз.");
                        break;
                }
            }
        }
    }
}