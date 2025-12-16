using Microsoft.VisualStudio.TestTools.UnitTesting;
using OOP_Lab6;
using System;
using System.Collections.Generic; // Залишаємо, бо потрібен для TryParse/Parse/ToString
using System.Linq; // Залишаємо, бо може бути потрібен для деяких тестів, якщо вони використовують LINQ

namespace OOP_Lab6_Test
{
    [TestClass]
    [DoNotParallelize]
    public class GameCharacterTests
    {
        // SETUP / CLEANUP

        [TestInitialize]
        public void Setup()
        {
            GameCharacter.ResetStaticCountersForTesting();
        }

        [TestCleanup]
        public void Cleanup()
        {
            // Тут не потрібно робити нічого, оскільки об'єкти з тестів
            // (включаючи їхні DecreaseActiveCount) були викликані всередині тестів.
        }

        // CONSTRUCTORS

        [TestMethod]
        public void DefaultConstructor_ShouldCreateAliveCharacter()
        {
            // Arrange
            GameCharacter c;

            // Act
            c = new GameCharacter();

            // Assert
            Assert.IsNotNull(c);
            Assert.IsTrue(c.IsAlive);
            Assert.AreEqual(1, c.Level);
            Assert.AreEqual(100, c.Health);

            // Cleanup
            GameCharacter.DecreaseActiveCount();
        }

        [TestMethod]
        public void ParameterizedConstructor_ShouldAssignNameAndClass()
        {
            // Act
            GameCharacter c = new GameCharacter("Hero", CharacterClass.Mage);

            // Assert
            Assert.AreEqual("Hero", c.Name);
            Assert.AreEqual(CharacterClass.Mage, c.ClassType);

            // Cleanup
            GameCharacter.DecreaseActiveCount();
        }

        [TestMethod]
        public void FullConstructor_ShouldAssignAllProperties()
        {
            // Act
            GameCharacter c = new GameCharacter("Tank", 50, 75, 100.5, CharacterClass.Warrior);

            // Assert
            Assert.AreEqual("Tank", c.Name);
            Assert.AreEqual(50, c.Level);
            Assert.AreEqual(75, c.Health);
            Assert.AreEqual(100.5, c.Experience);
            Assert.AreEqual(CharacterClass.Warrior, c.ClassType);
            Assert.IsTrue(c.IsAlive);

            // Cleanup
            GameCharacter.DecreaseActiveCount();
        }

        // NAME PROPERTY

        [TestMethod]
        [DataRow("")]
        [DataRow("ab")]
        [DataRow("averyveryveryverylongname")]
        public void Name_SetIncorrect_ShouldThrowException(string name)
        {
            // Arrange
            GameCharacter character = new GameCharacter();

            // Act/Assert
            try
            {
                character.Name = name;
                Assert.Fail("Очікувався ArgumentException");
            }
            catch (ArgumentException)
            {
                Assert.IsTrue(true);
            }
            // Cleanup
            GameCharacter.DecreaseActiveCount();
        }

        [TestMethod]
        public void Name_SetCorrect_ShouldAssign()
        {
            // Arrange
            GameCharacter character = new GameCharacter();
            string expected = "Hero";

            // Act
            character.Name = expected;

            // Assert
            Assert.AreEqual(expected, character.Name);

            // Cleanup
            GameCharacter.DecreaseActiveCount();
        }

        [TestMethod]
        public void Name_SetMinLength_ShouldAssign()
        {
            // Arrange
            GameCharacter character = new GameCharacter();

            // Act
            character.Name = "Abc";

            // Assert
            Assert.AreEqual("Abc", character.Name);

            // Cleanup
            GameCharacter.DecreaseActiveCount();
        }

        [TestMethod]
        public void Name_SetMaxLength_ShouldAssign()
        {
            // Arrange
            GameCharacter character = new GameCharacter();

            // Act
            character.Name = "123456789012345";

            // Assert
            Assert.AreEqual("123456789012345", character.Name);

            // Cleanup
            GameCharacter.DecreaseActiveCount();
        }

        // LEVEL PROPERTY

        [TestMethod]
        [DataRow(0)]
        [DataRow(-1)]
        [DataRow(101)]
        public void Level_SetIncorrect_ShouldThrowException(int level)
        {
            // Arrange
            GameCharacter character = new GameCharacter();

            // Act/Assert
            try
            {
                character.Level = level;
                Assert.Fail("Очікувався ArgumentException");
            }
            catch (ArgumentException)
            {
                Assert.IsTrue(true);
            }
            // Cleanup
            GameCharacter.DecreaseActiveCount();
        }

        [TestMethod]
        public void Level_SetCorrect_ShouldAssign()
        {
            // Arrange
            GameCharacter character = new GameCharacter();

            // Act
            character.Level = 10;

            // Assert
            Assert.AreEqual(10, character.Level);

            // Cleanup
            GameCharacter.DecreaseActiveCount();
        }

        [TestMethod]
        public void Level_SetMinBoundary_ShouldAssign()
        {
            // Arrange
            GameCharacter character = new GameCharacter();

            // Act
            character.Level = 1;

            // Assert
            Assert.AreEqual(1, character.Level);

            // Cleanup
            GameCharacter.DecreaseActiveCount();
        }

        [TestMethod]
        public void Level_SetMaxBoundary_ShouldAssign()
        {
            // Arrange
            GameCharacter character = new GameCharacter();

            // Act
            character.Level = GameCharacter.isMaxLevel;

            // Assert
            Assert.AreEqual(GameCharacter.isMaxLevel, character.Level);

            // Cleanup
            GameCharacter.DecreaseActiveCount();
        }

        // HEALTH PROPERTY

        [TestMethod]
        [DataRow(-1)]
        [DataRow(101)]
        public void Health_SetIncorrect_ShouldThrowException(int health)
        {
            // Arrange
            GameCharacter character = new GameCharacter();

            // Act/Assert
            try
            {
                character.Health = health;
                Assert.Fail("Очікувався ArgumentException");
            }
            catch (ArgumentException)
            {
                Assert.IsTrue(true);
            }
            // Cleanup
            GameCharacter.DecreaseActiveCount();
        }

        [TestMethod]
        public void Health_SetZero_ShouldKillCharacter()
        {
            // Arrange
            GameCharacter character = new GameCharacter();

            // Act
            character.Health = 0;

            // Assert
            Assert.IsFalse(character.IsAlive);

            // Cleanup
            GameCharacter.DecreaseActiveCount();
        }

        [TestMethod]
        public void Health_SetPositive_ShouldKeepAlive()
        {
            // Arrange
            GameCharacter character = new GameCharacter();

            // Act
            character.Health = 50;

            // Assert
            Assert.IsTrue(character.IsAlive);

            // Cleanup
            GameCharacter.DecreaseActiveCount();
        }

        [TestMethod]
        public void Health_SetMaxBoundary_ShouldKeepAlive()
        {
            // Arrange
            GameCharacter character = new GameCharacter();

            // Act
            character.Health = 100;

            // Assert
            Assert.IsTrue(character.IsAlive);

            // Cleanup
            GameCharacter.DecreaseActiveCount();
        }

        // EXPERIENCE PROPERTY

        [TestMethod]
        public void Experience_SetNegative_ShouldThrowException()
        {
            // Arrange
            GameCharacter character = new GameCharacter();

            // Act/Assert
            try
            {
                character.Experience = -10;
                Assert.Fail("Очікувався ArgumentException");
            }
            catch (ArgumentException)
            {
                Assert.IsTrue(true);
            }
            // Cleanup
            GameCharacter.DecreaseActiveCount();
        }

        [TestMethod]
        public void Experience_SetCorrect_ShouldAssign()
        {
            // Arrange
            GameCharacter character = new GameCharacter();

            // Act
            character.Experience = 25;

            // Assert
            Assert.AreEqual(25, character.Experience);

            // Cleanup
            GameCharacter.DecreaseActiveCount();
        }

        // TAKE DAMAGE
        [TestMethod]
        public void TakeDamage_ShouldReduceHealth()
        {
            // Arrange
            GameCharacter character = new GameCharacter();

            // Act
            character.TakeDamage(20);

            // Assert
            Assert.AreEqual(80, character.Health);

            // Cleanup
            GameCharacter.DecreaseActiveCount();
        }

        [TestMethod]
        public void TakeDamage_ShouldKillCharacter_WhenHealthZero()
        {
            // Arrange
            GameCharacter character = new GameCharacter();

            // Act
            character.TakeDamage(200);

            // Assert
            Assert.IsFalse(character.IsAlive);
            Assert.AreEqual(0, character.Health);

            // Cleanup
            GameCharacter.DecreaseActiveCount();
        }

        [TestMethod]
        public void TakeDamage_Negative_ShouldThrowException()
        {
            // Arrange
            GameCharacter character = new GameCharacter();

            // Act/Assert
            try
            {
                character.TakeDamage(-5);
                Assert.Fail("Очікувався ArgumentException");
            }
            catch (ArgumentException)
            {
                Assert.IsTrue(true);
            }
            // Cleanup
            GameCharacter.DecreaseActiveCount();
        }

        [TestMethod]
        public void TakeDamage_WhenDead_ShouldNotChangeHealth()
        {
            // Arrange
            GameCharacter character = new GameCharacter();
            character.TakeDamage(200);
            int healthAfterDeath = character.Health;

            // Act
            character.TakeDamage(10);

            // Assert
            Assert.AreEqual(healthAfterDeath, character.Health);
            Assert.AreEqual(0, character.Health);

            // Cleanup
            GameCharacter.DecreaseActiveCount();
        }

        [TestMethod]
        public void TakeDamageOverload_ShouldReduceHealth()
        {
            // Arrange
            GameCharacter character = new GameCharacter();
            int initialHealth = character.Health;
            int damage = 30;

            // Act
            character.TakeDamage(damage, "Goblin");

            // Assert
            Assert.AreEqual(initialHealth - damage, character.Health);

            // Cleanup
            GameCharacter.DecreaseActiveCount();
        }

        // HEAL

        [TestMethod]
        public void Heal_ShouldIncreaseHealth()
        {
            // Arrange
            GameCharacter character = new GameCharacter();
            character.TakeDamage(50);

            // Act
            character.Heal(20);

            // Assert
            Assert.AreEqual(70, character.Health);

            // Cleanup
            GameCharacter.DecreaseActiveCount();
        }

        [TestMethod]
        public void Heal_ShouldNotExceedMaxHealth()
        {
            // Arrange
            GameCharacter character = new GameCharacter();

            // Act
            character.Heal(50);

            // Assert
            Assert.AreEqual(100, character.Health);

            // Cleanup
            GameCharacter.DecreaseActiveCount();
        }

        [TestMethod]
        public void Heal_FullHealth_ShouldDoNothing()
        {
            // Arrange
            GameCharacter character = new GameCharacter();
            int initialHealth = character.Health;

            // Act
            character.Heal(10);

            // Assert
            Assert.AreEqual(initialHealth, character.Health);
            Assert.AreEqual(100, character.Health);

            // Cleanup
            GameCharacter.DecreaseActiveCount();
        }

        [TestMethod]
        public void Heal_WhenDead_ShouldDoNothing()
        {
            // Arrange
            GameCharacter character = new GameCharacter();
            character.TakeDamage(200);

            // Act
            character.Heal(50);

            // Assert
            Assert.AreEqual(0, character.Health);

            // Cleanup
            GameCharacter.DecreaseActiveCount();
        }

        // GAIN EXPERIENCE

        [TestMethod]
        public void GainExperience_ShouldIncreaseXP()
        {
            // Arrange
            GameCharacter character = new GameCharacter();

            // Act
            character.GainExperience(40.5);

            // Assert
            Assert.AreEqual(40.5, character.Experience);

            // Cleanup
            GameCharacter.DecreaseActiveCount();
        }

        // SET CLASS

        [TestMethod]
        public void SetClass_ShouldChangeClass()
        {
            // Arrange
            GameCharacter character = new GameCharacter();

            // Act
            character.SetClass(CharacterClass.Healer);

            // Assert
            Assert.AreEqual(CharacterClass.Healer, character.ClassType);

            // Cleanup
            GameCharacter.DecreaseActiveCount();
        }

        // STATIC MEMBERS

        [TestMethod]
        public void GetGameInfo_ShouldReturnString()
        {
            // Arrange/Act
            string info = GameCharacter.GetGameInfo();

            // Assert
            Assert.IsFalse(string.IsNullOrEmpty(info));
            Assert.IsTrue(info.Contains("RPG Simulator"));
            Assert.IsTrue(info.Contains(GameCharacter.isMaxLevel.ToString()));
        }

        [TestMethod]
        public void CreatedCount_ShouldIncrease()
        {
            // Arrange
            int before = GameCharacter.CreatedCount;

            // Act: Створюємо новий об'єкт
            GameCharacter c = new GameCharacter();

            // Assert
            Assert.AreEqual(before + 1, GameCharacter.CreatedCount);

            // Cleanup: зменшуємо лічильник для об'єкта 'c'
            GameCharacter.DecreaseActiveCount();
        }

        [TestMethod]
        public void ActiveCount_ShouldIncrease_OnCreation()
        {
            // Arrange
            int before = GameCharacter.ActiveCount;

            // Act
            GameCharacter c = new GameCharacter();

            // Assert
            Assert.AreEqual(before + 1, GameCharacter.ActiveCount);

            // Cleanup: зменшуємо лічильник для об'єкта 'c'
            GameCharacter.DecreaseActiveCount();
        }

        [TestMethod]
        public void DecreaseActiveCount_ShouldDecrease()
        {
            // Arrange
            GameCharacter c = new GameCharacter();
            int expected = 0;

            // Act
            GameCharacter.DecreaseActiveCount();

            // Assert
            Assert.AreEqual(expected, GameCharacter.ActiveCount);
        }

        [TestMethod]
        public void DecreaseActiveCount_ShouldNotGoBelowZero()
        {
            // Arrange
            int zeroCount = GameCharacter.ActiveCount;

            // Act
            GameCharacter.DecreaseActiveCount();

            // Assert
            Assert.AreEqual(zeroCount, GameCharacter.ActiveCount);
            Assert.AreEqual(0, GameCharacter.ActiveCount);
        }

        // PARSE

        [TestMethod]
        public void Parse_CorrectString_ShouldCreateCharacter()
        {
            // Arrange
            string s = "Hero;0;1;100;0";

            // Act
            GameCharacter c = GameCharacter.Parse(s);

            // Assert
            Assert.AreEqual("Hero", c.Name);
            Assert.AreEqual(CharacterClass.Warrior, c.ClassType);
            Assert.AreEqual(1, c.Level);
            Assert.AreEqual(100, c.Health);
            Assert.AreEqual(0, c.Experience);

            // Cleanup
            GameCharacter.DecreaseActiveCount();
        }

        [TestMethod]
        public void Parse_NullOrEmpty_ShouldThrowException()
        {
            try
            {
                GameCharacter.Parse(null);
                Assert.Fail("Очікувався ArgumentException для null");
            }
            catch (ArgumentException)
            {
                Assert.IsTrue(true);
            }

            try
            {
                GameCharacter.Parse("");
                Assert.Fail("Очікувався ArgumentException для пустого рядка");
            }
            catch (ArgumentException)
            {
                Assert.IsTrue(true);
            }
        }

        [TestMethod]
        public void Parse_InvalidFormat_ShouldThrowException_WrongPartCount()
        {
            try
            {
                GameCharacter.Parse("bad;data");
                Assert.Fail("Очікувався FormatException");
            }
            catch (FormatException)
            {
                Assert.IsTrue(true);
            }
        }

        [TestMethod]
        public void Parse_InvalidFormat_ShouldThrowException_InvalidClass()
        {
            string s = "Hero;99;1;100;0";
            try
            {
                GameCharacter.Parse(s);
                Assert.Fail("Очікувався ArgumentException");
            }
            catch (ArgumentException)
            {
                Assert.IsTrue(true);
            }
        }

        [TestMethod]
        public void Parse_InvalidFormat_ShouldThrowException_NonNumericLevel()
        {
            string s = "Hero;0;one;100;0";
            try
            {
                GameCharacter.Parse(s);
                Assert.Fail("Очікувався ArgumentException");
            }
            catch (ArgumentException)
            {
                Assert.IsTrue(true);
            }
        }

        [TestMethod]
        public void Parse_InvalidFormat_ShouldThrowException_NonNumericHealth()
        {
            string s = "Hero;0;1;full;0";
            try
            {
                GameCharacter.Parse(s);
                Assert.Fail("Очікувався ArgumentException");
            }
            catch (ArgumentException)
            {
                Assert.IsTrue(true);
            }
        }

        [TestMethod]
        public void Parse_InvalidFormat_ShouldThrowException_NonNumericXP()
        {
            string s = "Hero;0;1;100;none";
            try
            {
                GameCharacter.Parse(s);
                Assert.Fail("Очікувався ArgumentException");
            }
            catch (ArgumentException)
            {
                Assert.IsTrue(true);
            }
        }

        // TRYPARSE

        [TestMethod]
        public void TryParse_Invalid_ShouldReturnFalse()
        {
            GameCharacter c;
            bool result = GameCharacter.TryParse("bad data", out c);

            Assert.IsFalse(result);
            Assert.IsNull(c);
        }

        [TestMethod]
        public void TryParse_Valid_ShouldReturnTrue()
        {
            // Arrange
            GameCharacter c;

            // Act
            bool result = GameCharacter.TryParse("Hero;0;1;100;0", out c);

            // Assert
            Assert.IsTrue(result);
            Assert.IsNotNull(c);

            // Cleanup
            if (result)
            {
                GameCharacter.DecreaseActiveCount();
            }
        }

        // TOSTRING

        [TestMethod]
        public void ToString_ShouldReturnFormattedString()
        {
            // Arrange
            GameCharacter character = new GameCharacter();
            string expected = $"{character.Name};{character.ClassType};{character.Level};{character.Health};{character.Experience}";

            // Act
            string actual = character.ToString();

            // Assert
            Assert.AreEqual(expected, actual);

            // Cleanup
            GameCharacter.DecreaseActiveCount();
        }
    }
}