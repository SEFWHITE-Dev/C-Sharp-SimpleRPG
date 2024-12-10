


namespace TestGameEngine.ViewModels;

[TestClass]
public class TestGameSession
{
    [TestMethod]
    public void TestCreateGameSession()
    {
        GameSession gameSession = new GameSession();

        Assert.IsNotNull(gameSession.CurrentPlayer); // Assert means, we expect this condition to be true
        Assert.AreEqual("Town Square", gameSession.CurrentLocation.Name);
    }

    [TestMethod]
    public void TestPlayerMovesHomeAndIsCompletelyHealedOnKilled()
    {
        GameSession gameSession = new GameSession();

        gameSession.CurrentPlayer.TakeDamage(999);

        Assert.AreEqual("Home", gameSession.CurrentLocation.Name);
        Assert.AreEqual(gameSession.CurrentPlayer.Level * 10, gameSession.CurrentPlayer.CurrentHealth);
    }
}
