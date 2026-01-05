public enum EndingType { Good, Bad, SecretEscapeUsed }

public static class EndingContext
{
    public static EndingType NextEnding = EndingType.Good;
}

// When you trigger the ending do the following:
// EndingContext.NextEnding = EndingType.Bad;
// SceneManager.LoadScene(GameScene.Ending);
