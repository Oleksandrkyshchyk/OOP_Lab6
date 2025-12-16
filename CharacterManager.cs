using System;
using System.Collections.Generic;
using System.Linq;

namespace OOP_Lab6
{
    public static class CharacterManager
    {
        // 1. ДОДАВАННЯ
        public static bool AddCharacterFromString(List<GameCharacter> list, string data)
        {
            if (GameCharacter.TryParse(data, out GameCharacter character))
            {
                list.Add(character);
                return true;
            }
            return false;
        }

        // 2. ПОШУК (List<T>.FindAll)
        public static List<GameCharacter> SearchByName(List<GameCharacter> list, string name)
        {
            return list.FindAll(c => c.Name == name);
        }

        public static List<GameCharacter> SearchByClass(List<GameCharacter> list, CharacterClass classType)
        {
            return list.FindAll(c => c.ClassType == classType);
        }

        // 3. ВИДАЛЕННЯ (List<T>.RemoveAll та List<T>.RemoveAt)

        public static int RemoveCharactersByClass(List<GameCharacter> list, CharacterClass classType)
        {
            int removedCount = list.RemoveAll(c => c.ClassType == classType);

            // Коректно оновлюємо статичний лічильник ActiveCount
            for (int i = 0; i < removedCount; i++)
            {
                GameCharacter.DecreaseActiveCount();
            }

            return removedCount;
        }

        public static bool RemoveCharacterByIndex(List<GameCharacter> list, int index)
        {
            if (index >= 0 && index < list.Count)
            {
                list.RemoveAt(index);
                GameCharacter.DecreaseActiveCount();
                return true;
            }
            return false;
        }
    }
}