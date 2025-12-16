using System;

namespace OOP_Lab6
{
    public enum CharacterClass
    {
        Warrior,
        Mage,
        Archer,
        Healer,
        Assassin
    }

    public class GameCharacter
    {
        // STATIC PARTS
        private static int _createdCount = 0;
        private static int _activeCount = 0;

        // Лічильник коректно створених / існуючих на даний момент об'єктів
        public static int CreatedCount => _createdCount;
        public static int ActiveCount => _activeCount;

        // Статична характеристика предметної області
        public static int isMaxLevel { get; } = 100;

        // Довільний static-метод
        public static string GetGameInfo()
        {
            return "Гра: RPG Simulator | Максимальний рівень персонажа: " + isMaxLevel;
        }

        // Допоміжний метод для очищення статичних лічильників (для ізоляції тестів)
        public static void ResetStaticCountersForTesting()
        {
            _createdCount = 0;
            _activeCount = 0;
        }

        // FIELDS

        private string _name = null!;
        private int _level;
        private int _health;
        private double _experience;
        private bool _isAlive;
        private CharacterClass _classType;

        // PROPERTIES

        public string Name
        {
            get => _name;
            set
            {
                if (string.IsNullOrWhiteSpace(value) || value.Length < 3 || value.Length > 15)
                    throw new ArgumentException("Ім’я має бути від 3 до 15 символів");
                _name = value;
            }
        }

        public int Level
        {
            get => _level;
            set
            {
                if (value < 1 || value > isMaxLevel)
                    throw new ArgumentException($"Рівень має бути від 1 до {isMaxLevel}");
                _level = value;
            }
        }

        public int Health
        {
            get => _health;
            set
            {
                if (value < 0 || value > 100)
                    throw new ArgumentException("Здоровʼя має бути від 0 до 100");
                _health = value;
                CheckDeath();
            }
        }

        public double Experience
        {
            get => _experience;
            set
            {
                if (value < 0)
                    throw new ArgumentException("Досвід не може бути відʼємним");
                _experience = value;
            }
        }

        public bool IsAlive => _isAlive;

        public CharacterClass ClassType => _classType;


        // CONSTRUCTORS
        // 1. Конструктор без параметрів
        public GameCharacter()
        {
            _name = "Unknown";
            _level = 1;
            _health = 100;
            _experience = 0;
            _classType = CharacterClass.Warrior;
            _isAlive = true;

            _createdCount++;
            _activeCount++;
        }

        // 2. Перевантажений конструктор
        public GameCharacter(string name, CharacterClass classType)
            : this()
        {
            Name = name;
            _classType = classType;
        }

        // 3. Повний конструктор
        public GameCharacter(string name, int level, int health, double experience, CharacterClass classType)
        {
            Name = name;
            Level = level;
            Health = health;
            Experience = experience;
            _classType = classType;
            _isAlive = true;

            _createdCount++;
            _activeCount++;
        }

        // METHODS
        public void TakeDamage(int dmg)
        {
            if (dmg < 0)
                throw new ArgumentException("Шкода не може бути від'ємною");

            if (!_isAlive)
                return;

            int newHealth = _health - dmg;

            if (newHealth <= 0)
            {
                _health = 0;
                _isAlive = false;
            }
            else
            {
                _health = newHealth;
            }
        }

        public void TakeDamage(int dmg, string source)
        {
            Console.WriteLine($"Отримано шкоду від {source}");
            TakeDamage(dmg);
        }


        public void Heal(int amount)
        {
            if (!_isAlive) return;
            Health = Math.Min(100, Health + amount);
        }

        public void GainExperience(double xp)
        {
            Experience += xp;
        }

        public void SetClass(CharacterClass classType)
        {
            _classType = classType;
        }

        private void CheckDeath()
        {
            _isAlive = _health > 0;
        }

        public void ShowStats()
        {
            Console.WriteLine("---- Характеристики персонажа ----");
            Console.WriteLine($"Імʼя: {Name}");
            Console.WriteLine($"Клас: {ClassType}");
            Console.WriteLine($"Рівень: {Level}");
            Console.WriteLine($"HP: {Health}");
            Console.WriteLine($"XP: {Experience}");
            Console.WriteLine($"Статус: {(IsAlive ? "Живий" : "Мертвий")}");
            Console.WriteLine("----------------------------------");
        }

        public static void DecreaseActiveCount()
        {
            if (_activeCount > 0)
                _activeCount--;
        }

        // PARSE / TRYPARSE

        public static GameCharacter Parse(string input)
        {
            if (string.IsNullOrWhiteSpace(input))
                throw new ArgumentException("Рядок порожній");

            string[] parts = input.Split(';');
            if (parts.Length != 5)
                throw new FormatException("Невірна кількість параметрів");

            string name = parts[0];

            if (!int.TryParse(parts[1], out int cls) ||
                !Enum.IsDefined(typeof(CharacterClass), cls))
                throw new ArgumentException("Некоректний клас персонажа");

            if (!int.TryParse(parts[2], out int level))
                throw new ArgumentException("Некоректний рівень");

            if (!int.TryParse(parts[3], out int health))
                throw new ArgumentException("Некоректне здоровʼя");

            // Використовуємо InvariantCulture для XP (дробове число)
            if (!double.TryParse(
                parts[4],
                System.Globalization.NumberStyles.Any,
                System.Globalization.CultureInfo.InvariantCulture,
                out double xp))
                throw new ArgumentException("Некоректний досвід");

            // створення через конструктор
            return new GameCharacter(name, level, health, xp, (CharacterClass)cls);
        }


        public static bool TryParse(string input, out GameCharacter character)
        {
            try
            {
                character = Parse(input);
                return true;
            }
            catch
            {
                character = null;
                return false;
            }
        }

        // ToString
        public override string ToString()
        {
            return $"{Name};{ClassType};{Level};{Health};{Experience}";
        }
    }
}