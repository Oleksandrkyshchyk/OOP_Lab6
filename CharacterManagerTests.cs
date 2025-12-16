using Microsoft.VisualStudio.TestTools.UnitTesting;
using OOP_Lab6;
using System.Collections.Generic;
using System.Linq;

namespace OOP_Lab6_Test
{
    [TestClass]
    [DoNotParallelize]
    public class CharacterManagerTests
    {
        // SETUP / CLEANUP

        [TestInitialize]
        public void Setup()
        {
            GameCharacter.ResetStaticCountersForTesting();
        }

        // ДОПОМІЖНІ МЕТОДИ (SetupManagerList, CleanupManagerList)

        private List<GameCharacter> SetupManagerList() 
        {
            // Створюємо список, щоб ActiveCount коректно збільшувався
            List<GameCharacter> list = new List<GameCharacter>
            {
                new GameCharacter("Warrior1", CharacterClass.Warrior),
                new GameCharacter("Mage1", CharacterClass.Mage),
                new GameCharacter("Warrior2", CharacterClass.Warrior)
            };
            return list;
        }

        private void CleanupManagerList(List<GameCharacter> list) 
        {
            // Зменшуємо ActiveCount для всіх об'єктів, створених у SetupTestList
            int count = list.Count;
            list.Clear();
            for (int i = 0; i < count; i++)
            {
                GameCharacter.DecreaseActiveCount();
            }
        }

        // Нові тести для CharacterManager (Логіка колекцій)

        [TestMethod]
        public void AddCharacterFromString_ValidData_ShouldReturnTrueAndIncreaseCount()
        {
            // Arrange
            List<GameCharacter> list = new List<GameCharacter>();
            string validData = "TestHero;1;50;100;10.5";
            int initialActiveCount = GameCharacter.ActiveCount;

            // Act
            bool result = CharacterManager.AddCharacterFromString(list, validData);

            // Assert
            Assert.IsTrue(result);
            Assert.AreEqual(1, list.Count);
            Assert.AreEqual(initialActiveCount + 1, GameCharacter.ActiveCount);

            // Cleanup
            CharacterManager.RemoveCharacterByIndex(list, 0);
        }

        [TestMethod]
        public void AddCharacterFromString_InvalidData_ShouldReturnFalse_BadName()
        {
            // Arrange
            List<GameCharacter> list = new List<GameCharacter>();
            int initialActiveCount = GameCharacter.ActiveCount;
            string invalidData = "ad;0;1;100;0"; // Некоректне ім'я

            // Act
            bool result = CharacterManager.AddCharacterFromString(list, invalidData);

            // Assert
            Assert.IsFalse(result);
            Assert.AreEqual(0, list.Count);
            Assert.AreEqual(initialActiveCount, GameCharacter.ActiveCount);
        }

        [TestMethod]
        public void AddCharacterFromString_InvalidData_ShouldReturnFalse_InvalidClass()
        {
            // Arrange
            List<GameCharacter> list = new List<GameCharacter>();
            int initialActiveCount = GameCharacter.ActiveCount;
            string invalidData = "Hero;99;1;100;0"; // Некоректний клас (за межами enum)

            // Act
            bool result = CharacterManager.AddCharacterFromString(list, invalidData);

            // Assert
            Assert.IsFalse(result);
            Assert.AreEqual(0, list.Count);
            Assert.AreEqual(initialActiveCount, GameCharacter.ActiveCount);
        }

        [TestMethod]
        public void AddCharacterFromString_InvalidData_ShouldReturnFalse_WrongPartCount()
        {
            // Arrange
            List<GameCharacter> list = new List<GameCharacter>();
            int initialActiveCount = GameCharacter.ActiveCount;
            string invalidData = "Hero;0;1;100"; // Некоректна кількість параметрів

            // Act
            bool result = CharacterManager.AddCharacterFromString(list, invalidData);

            // Assert
            Assert.IsFalse(result);
            Assert.AreEqual(0, list.Count);
            Assert.AreEqual(initialActiveCount, GameCharacter.ActiveCount);
        }

        // ТЕСТУВАННЯ ПОШУКУ

        [TestMethod]
        public void SearchByName_ShouldReturnMatchingCharacters()
        {
            // Arrange
            List<GameCharacter> list = SetupManagerList();

            // Act
            List<GameCharacter> found = CharacterManager.SearchByName(list, "Warrior1");

            // Assert
            Assert.AreEqual(1, found.Count);
            Assert.AreEqual("Warrior1", found[0].Name);

            // Cleanup
            CleanupManagerList(list);
        }

        [TestMethod]
        public void SearchByName_ShouldReturnEmptyListIfNotFound()
        {
            // Arrange
            List<GameCharacter> list = SetupManagerList();

            // Act
            List<GameCharacter> found = CharacterManager.SearchByName(list, "Healer");

            // Assert
            Assert.AreEqual(0, found.Count);

            // Cleanup
            CleanupManagerList(list);
        }

        [TestMethod]
        public void SearchByClass_ShouldReturnMultipleMatches()
        {
            // Arrange
            List<GameCharacter> list = SetupManagerList();

            // Act
            List<GameCharacter> found = CharacterManager.SearchByClass(list, CharacterClass.Warrior);

            // Assert
            Assert.AreEqual(2, found.Count);
            Assert.IsTrue(found.All(c => c.ClassType == CharacterClass.Warrior));

            // Cleanup
            CleanupManagerList(list);
        }

        [TestMethod]
        public void SearchByClass_ShouldReturnEmptyListIfNoMatches()
        {
            // Arrange
            List<GameCharacter> list = SetupManagerList();

            // Act
            List<GameCharacter> found = CharacterManager.SearchByClass(list, CharacterClass.Assassin);

            // Assert
            Assert.AreEqual(0, found.Count);

            // Cleanup
            CleanupManagerList(list);
        }

        // ТЕСТУВАННЯ ВИДАЛЕННЯ

        [TestMethod]
        public void RemoveCharactersByClass_ShouldRemoveAllMatchesAndDecreaseActiveCount()
        {
            // Arrange
            List<GameCharacter> list = SetupManagerList();
            int initialActiveCount = GameCharacter.ActiveCount;

            // Act
            int removed = CharacterManager.RemoveCharactersByClass(list, CharacterClass.Warrior);

            // Assert
            Assert.AreEqual(2, removed);
            Assert.AreEqual(1, list.Count);
            Assert.AreEqual(initialActiveCount - 2, GameCharacter.ActiveCount);
            Assert.AreEqual(CharacterClass.Mage, list[0].ClassType);

            // Cleanup (видаляємо останній об'єкт)
            CharacterManager.RemoveCharacterByIndex(list, 0);
        }

        [TestMethod]
        public void RemoveCharactersByClass_ShouldReturnZeroIfNoMatches()
        {
            // Arrange
            List<GameCharacter> list = SetupManagerList();
            int initialActiveCount = GameCharacter.ActiveCount;

            // Act
            int removed = CharacterManager.RemoveCharactersByClass(list, CharacterClass.Assassin);

            // Assert
            Assert.AreEqual(0, removed);
            Assert.AreEqual(3, list.Count);
            Assert.AreEqual(initialActiveCount, GameCharacter.ActiveCount);

            // Cleanup
            CleanupManagerList(list);
        }


        [TestMethod]
        public void RemoveCharacterByIndex_ShouldRemoveOneItemAndDecreaseActiveCount()
        {
            // Arrange
            List<GameCharacter> list = SetupManagerList();
            int initialActiveCount = GameCharacter.ActiveCount;

            // Act
            bool success = CharacterManager.RemoveCharacterByIndex(list, 1);

            // Assert
            Assert.IsTrue(success);
            Assert.AreEqual(2, list.Count);
            Assert.AreEqual(initialActiveCount - 1, GameCharacter.ActiveCount);
            Assert.AreEqual("Warrior2", list[1].Name);

            // Cleanup (видаляємо решту)
            CleanupManagerList(list);
        }

        [TestMethod]
        public void RemoveCharacterByIndex_InvalidIndex_ShouldReturnFalseAndNotChangeCount()
        {
            // Arrange
            List<GameCharacter> list = SetupManagerList();
            int initialActiveCount = GameCharacter.ActiveCount;

            // Act
            bool success = CharacterManager.RemoveCharacterByIndex(list, 99);

            // Assert
            Assert.IsFalse(success);
            Assert.AreEqual(3, list.Count);
            Assert.AreEqual(initialActiveCount, GameCharacter.ActiveCount);

            // Cleanup
            CleanupManagerList(list);
        }
    }
}