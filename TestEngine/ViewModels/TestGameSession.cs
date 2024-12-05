using Engine.ViewModels;
using Microsoft.VisualStudio.TestTools.UnitTesting;
using System;

namespace TestEngine.ViewModels
{
    [TestClass] // how we declare a unit test class
    public class TestGameSession
    {
        [TestMethod] // how we declare a unit test function
        public void TestCreateGameSession()
        {
            GameSession gameSession = new GameSession();

            // Assert means we expect this condition to be true, make sure that it is true
            Assert.IsNotNull(gameSession.CurrentPlayer);
            Assert.AreEqual("Town square", gameSession.CurrentPlayer.Name); // check that the starting location's name is Town Square
        }

        [TestMethod]
        public void TestPlayerMovesHomeAndIsHealedOnKilled()
        {
            GameSession gameSession = new GameSession();

            gameSession.CurrentPlayer.TakeDamage(999);

            Assert.AreEqual("Home",  gameSession.CurrentLocation.Name);
            Assert.AreEqual(gameSession.CurrentPlayer.Level * 10, gameSession.CurrentPlayer.Health);
        }
    }
}
