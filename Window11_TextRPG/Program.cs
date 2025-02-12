namespace Window11_TextRPG
{
    internal class Program
    {
        static void Main(string[] args)
        {
            Console.WriteLine("Hello, World!");
            GameManager.Instance.ChangeScene(SceneState.PlayerManager);
        }
    }
}
