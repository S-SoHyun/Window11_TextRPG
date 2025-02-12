namespace Window11_TextRPG
{
    internal class Program
    {
        static void Main(string[] args)
        {
            GameManager.Instance.ChangeScene(SceneState.PlayerManager);
        }
    }
}
