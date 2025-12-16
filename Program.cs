using System;
using System.Collections.Generic;
using System.Linq; // Потрібно для деяких функцій List<T>, які ми залишимо

// Простір імен залишається OOP_Lab6
namespace OOP_Lab6
{
    class Program
    {
        static Random rnd = new Random();

        static void Main()
        {
            Console.OutputEncoding = System.Text.Encoding.UTF8;
            Console.InputEncoding = System.Text.Encoding.UTF8;

            int maxCount;
            Console.WriteLine("Введіть максимальну кількість персонажів:");
            while (!int.TryParse(Console.ReadLine(), out maxCount) || maxCount <= 0)
            {
                Console.WriteLine("❌ Помилка! Введіть число > 0:");
            }

            // Вимога 1: Використання List<GameCharacter>
            List<GameCharacter> characters = new List<GameCharacter>();
            int choice;

            do
            {
                Console.WriteLine("\n===== М Е Н Ю =====");
                Console.WriteLine("1 - Додати персонажа");
                Console.WriteLine("2 - Переглянути всіх персонажів");
                Console.WriteLine("3 - Знайти персонажа");
                Console.WriteLine("4 - Демонстрація поведінки");
                Console.WriteLine("5 - Видалити персонажа");
                Console.WriteLine("6 - Продемонструвати static-методи");
                Console.WriteLine("0 - Вийти");
                Console.Write("Ваш вибір: ");

                while (!int.TryParse(Console.ReadLine(), out choice) || choice < 0 || choice > 6)
                {
                    Console.WriteLine("❌ Введіть число від 0 до 6!");
                }

                switch (choice)
                {
                    case 1: AddCharacter(characters, maxCount); break;
                    case 2: ShowAll(characters); break;
                    case 3: SearchCharacter(characters); break;
                    case 4: DemoBehavior(characters); break;
                    case 5: DeleteCharacter(characters); break;
                    case 6: DemoStaticMethods(); break;
                    case 0: Console.WriteLine("Вихід із програми..."); break;
                }

            } while (choice != 0);
        }

        // 1. ДОДАВАННЯ (Рефакторинг: додавання рядком винесено в CharacterManager)

        static void AddCharacter(List<GameCharacter> list, int max)
        {
            if (list.Count >= max)
            {
                Console.WriteLine("Досягнуто максимальної кількості персонажів!");
                return;
            }

            Console.WriteLine("1 - Ввести параметри вручну");
            Console.WriteLine("2 - Ввести рядок (TryParse)");
            Console.Write("Ваш вибір: ");

            int mode;
            while (!int.TryParse(Console.ReadLine(), out mode) || (mode != 1 && mode != 2))
                Console.WriteLine("❌ Введіть 1 або 2");

            // Додавання через TryParse (ВИКЛИК CharacterManager)
            if (mode == 2)
            {
                Console.WriteLine("Формат рядка: Name;Class;Level;Health;XP");
                Console.Write("Введіть рядок: ");
                string input = Console.ReadLine();

                if (CharacterManager.AddCharacterFromString(list, input)) // Виклик тестованого методу
                {
                    Console.WriteLine("✅ Персонажа успішно додано через TryParse!");
                }
                else
                {
                    Console.WriteLine("❌ Некоректний формат рядка або недійсна валідація!");
                }
                return;
            }

            // Додавання вручну (Залишаємо в Program.cs, оскільки це I/O)
            Console.Write("Ім’я персонажа: ");
            string name = Console.ReadLine();

            int level;
            Console.Write("Рівень (1–100): ");
            while (!int.TryParse(Console.ReadLine(), out level) || level < 1 || level > 100)
            {
                Console.WriteLine("❌ Введіть число від 1 до 100!");
                Console.Write("Рівень (1–100): ");
            }

            int health;
            Console.Write("Здоровʼя (0–100): ");
            while (!int.TryParse(Console.ReadLine(), out health) || health < 0 || health > 100)
            {
                Console.WriteLine("❌ Введіть число від 0 до 100!");
                Console.Write("Здоровʼя (0–100): ");
            }

            double xp;
            Console.Write("Досвід: ");
            while (!double.TryParse(Console.ReadLine(), out xp) || xp < 0)
            {
                Console.WriteLine("❌ Введіть додатне число!");
                Console.Write("Досвід: ");
            }

            int cls;
            Console.WriteLine("Клас (0-Warrior, 1-Mage, 2-Archer, 3-Healer, 4-Assassin): ");
            while (!int.TryParse(Console.ReadLine(), out cls) || cls < 0 || cls > 4)
                Console.WriteLine("❌ Введіть число від 0 до 4!");

            CharacterClass classType = (CharacterClass)cls;
            int ctor = rnd.Next(1, 4);
            GameCharacter ch;

            try
            {
                switch (ctor)
                {
                    case 1:
                        ch = new GameCharacter();
                        Console.WriteLine("🟢 Використано конструктор БЕЗ параметрів");
                        ch.Name = name;
                        ch.Level = level;
                        ch.Health = health;
                        ch.Experience = xp;
                        ch.SetClass(classType);
                        break;

                    case 2:
                        ch = new GameCharacter(name, classType);
                        Console.WriteLine("🟡 Використано конструктор (name, class)");
                        ch.Level = level;
                        ch.Health = health;
                        ch.Experience = xp;
                        break;

                    default:
                        ch = new GameCharacter(name, level, health, xp, classType);
                        Console.WriteLine("🔵 Використано ПОВНИЙ конструктор");
                        break;
                }

                list.Add(ch);
                Console.WriteLine("Персонажа успішно додано!");
            }
            catch (Exception ex)
            {
                // Важливо: об'єкт може бути частково створений, якщо конструктор без параметрів був успішним,
                // але валідація властивості (наприклад, Name) провалилася. У цьому випадку об'єкт не додається.
                Console.WriteLine($"Помилка: {ex.Message}");
            }
        }

        // ==================== 2. ВИВІД (Без змін, тільки I/O) ====================

        static void ShowAll(List<GameCharacter> list)
        {
            Console.WriteLine($"Загальна кількість створених персонажів: {GameCharacter.CreatedCount}");
            Console.WriteLine($"Активних персонажів: {GameCharacter.ActiveCount}");

            if (list.Count == 0)
            {
                Console.WriteLine("Список порожній!");
                return;
            }

            Console.WriteLine("\nН-р| Імʼя       | Клас       | Рівень | HP  | XP");
            Console.WriteLine("-------------------------------------------------");

            for (int i = 0; i < list.Count; i++)
            {
                var c = list[i];
                Console.WriteLine($"{i + 1,2} | {c.Name,-10} | {c.ClassType,-10} | {c.Level,6} | {c.Health,3} | {c.Experience}");
            }
        }

        // ==================== 3. ПОШУК (Рефакторинг: логіка пошуку винесена) ====================

        static void SearchCharacter(List<GameCharacter> list)
        {
            if (list.Count == 0)
            {
                Console.WriteLine("Список порожній!");
                return;
            }

            int option;
            Console.WriteLine("Шукати за:\n1 - Імʼям\n2 - Класом");
            while (!int.TryParse(Console.ReadLine(), out option) || (option != 1 && option != 2))
                Console.WriteLine("❌ Введіть 1 або 2!");

            List<GameCharacter> found;

            if (option == 1)
            {
                Console.Write("Введіть імʼя: ");
                string name = Console.ReadLine();
                // ВИКЛИК CharacterManager
                found = CharacterManager.SearchByName(list, name);
            }
            else
            {
                int cls;
                Console.Write("Введіть клас (0–4): ");
                while (!int.TryParse(Console.ReadLine(), out cls) || cls < 0 || cls > 4)
                    Console.WriteLine("❌ Введіть число від 0 до 4!");

                // ВИКЛИК CharacterManager
                found = CharacterManager.SearchByClass(list, (CharacterClass)cls);
            }

            if (found.Count == 0)
            {
                Console.WriteLine("❌ Персонажів не знайдено.");
                return;
            }

            foreach (var c in found)
                c.ShowStats();

        }

        // ==================== 4. ПОВЕДІНКА (Без змін, тільки I/O) ====================

        static void DemoBehavior(List<GameCharacter> list)
        {
            Console.Write("Введіть імʼя персонажа: ");
            string name = Console.ReadLine();

            var ch = list.Find(c => c.Name == name);
            if (ch == null)
            {
                Console.WriteLine("❌ Персонажа не знайдено!");
                return;
            }

            int act;
            Console.WriteLine("1 - Завдати шкоди\n2 - Зцілити\n3 - Додати XP");
            while (!int.TryParse(Console.ReadLine(), out act) || act < 1 || act > 3)
                Console.WriteLine("❌ Введіть число від 1 до 3!");

            if (act == 1)
            {
                int dmg;
                Console.Write("Шкода: ");
                while (!int.TryParse(Console.ReadLine(), out dmg) || dmg < 0)
                    Console.WriteLine("❌ Введіть додатне число!");

                int type;
                while (true)
                {
                    Console.WriteLine("1 - Звичайна атака\n2 - Атака зі зброєю");
                    if (int.TryParse(Console.ReadLine(), out type) && (type == 1 || type == 2))
                        break;
                    Console.WriteLine("❌ Введіть 1 або 2!");
                }

                if (type == 1)
                {
                    Console.WriteLine($"⚔ Звичайна атака! Шкода: {dmg}");
                    ch.TakeDamage(dmg);
                }
                else
                {
                    ch.TakeDamage(dmg, "меч");
                }
            }
            else if (act == 2)
            {
                int heal;
                Console.Write("Зцілити на: ");
                while (!int.TryParse(Console.ReadLine(), out heal) || heal < 0)
                    Console.WriteLine("❌ Введіть додатне число!");
                Console.WriteLine($"{name} зцілюється на {heal} HP!");
                ch.Heal(heal);
            }
            else
            {
                double xp;
                Console.Write("XP: ");
                while (!double.TryParse(Console.ReadLine(), out xp) || xp < 0)
                    Console.WriteLine("❌ Введіть додатне число!");
                Console.WriteLine($"{name} отриммує {xp} XP!");
                ch.GainExperience(xp);
            }
        }

        // ==================== 5. ВИДАЛЕННЯ (Рефакторинг: логіка видалення винесена) ====================

        static void DeleteCharacter(List<GameCharacter> list)
        {
            if (list.Count == 0)
            {
                Console.WriteLine("Список порожній!");
                return;
            }

            int mode;
            while (true)
            {
                Console.WriteLine("Видалити за:");
                Console.WriteLine("1 - Номером");
                Console.WriteLine("2 - Класом");
                Console.Write("Ваш вибір: ");

                if (int.TryParse(Console.ReadLine(), out mode) && (mode == 1 || mode == 2))
                    break;

                Console.WriteLine("Помилка! Введіть 1 або 2.");
            }

            // 🔹 Видалення за номером
            if (mode == 1)
            {
                int index;
                while (true)
                {
                    Console.Write("Номер персонажа: ");
                    if (int.TryParse(Console.ReadLine(), out index) &&
                        index >= 1 && index <= list.Count)
                        break;

                    Console.WriteLine("Невірний номер!");
                }

                // ВИКЛИК CharacterManager
                if (CharacterManager.RemoveCharacterByIndex(list, index - 1))
                {
                    Console.WriteLine("Персонажа видалено.");
                }
                else
                {
                    Console.WriteLine("❌ Помилка видалення.");
                }
            }
            // 🔹 Видалення за класом
            else
            {
                int cls;
                while (true)
                {
                    Console.Write("Клас (0–4): ");
                    if (int.TryParse(Console.ReadLine(), out cls) && cls >= 0 && cls <= 4)
                        break;

                    Console.WriteLine("Помилка! Введіть число від 0 до 4.");
                }

                // ВИКЛИК CharacterManager
                int removedCount = CharacterManager.RemoveCharactersByClass(list, (CharacterClass)cls);

                if (removedCount == 0)
                    Console.WriteLine("Персонажів цього класу не знайдено.");
                else
                {
                    Console.WriteLine($"Видалено персонажів: {removedCount}");
                }

            }
        }

        // 6. Static-методи (Без змін, тільки I/O)

        static void DemoStaticMethods()
        {
            Console.WriteLine("\n=== Static-методи класу GameCharacter ===");
            Console.WriteLine($"Кількість створених персонажів: {GameCharacter.CreatedCount}");
            Console.WriteLine($"Активних персонажів: {GameCharacter.ActiveCount}");

            Console.WriteLine(GameCharacter.GetGameInfo());

            string example = "Arthur;0;10;80;150";
            Console.WriteLine("\nTryParse приклад:");
            Console.WriteLine(example);

            if (GameCharacter.TryParse(example, out GameCharacter ch))
            {
                Console.WriteLine("Успішно:");
                Console.WriteLine(ch);
            }
            else
            {
                Console.WriteLine("❌ Помилка парсингу");
            }
        }

    }
}