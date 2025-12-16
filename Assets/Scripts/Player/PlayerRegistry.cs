public static class PlayerRegistry
{
    public static Player Player { get; set; }
    
    public static void RegisterPlayer(Player player)
    {
        Player = player;
    }
    
    public static void UnregisterPlayer(Player player)
    {
        if (Player == player)
            Player = null;
    }
}