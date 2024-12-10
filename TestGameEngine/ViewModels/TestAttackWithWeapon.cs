using Engine.Actions;
using Engine.Factories;
using Engine.Models;

namespace TestGameEngine;

[TestClass]
public class TestAttackWithWeapon
{
    [TestMethod]
    public void Test_Constructor_GoodParameters()
    {
        GameItem stick = ItemFactory.CreateGameItem(1001);

        AttackWithWeapon attackWithWeapon = new AttackWithWeapon(stick, 1, 5);

        Assert.IsNotNull(attackWithWeapon);
    }

    [TestMethod]
    [ExpectedException(typeof(ArgumentException))]
    public void Test_Constructor_ItemIsNotAWeapon() 
    {
        GameItem healthPotion = ItemFactory.CreateGameItem(2001);

        // health potion is not a weapon, so the constructor should throw an exception
        AttackWithWeapon attackWithWeapon = new AttackWithWeapon(healthPotion, 1, 5);

    }

    [TestMethod]
    [ExpectedException(typeof(ArgumentException))]
    public void Test_Constructor_MinDamageLessThanZero()
    {
        GameItem stick = ItemFactory.CreateGameItem(1001);

        // the min damage is less than 0, so the constructor should throw an exception
        AttackWithWeapon attackWithWeapon = new AttackWithWeapon(stick, -1, 5);
    }

    [TestMethod]
    [ExpectedException(typeof(ArgumentException))]
    public void Test_Constructor_MaxDamageLessThanMinDamage()
    {
        GameItem stick = ItemFactory.CreateGameItem(1001);

        // the max damage is less than min damage, so the constructor should throw an exception
        AttackWithWeapon attackWithWeapon = new AttackWithWeapon(stick, 2, 1);
    }
}
