#nullable enable



namespace Mythonia.Game
{
    public class MTextureManager
    {
        public Main MGame { get; init; }

        private readonly List<MTexture> _textures = new();

        public MTexture this[string name]
        {
            get => _textures.Find(texture => texture.Name == name) ?? throw new Exception($"Texture \"{name}\" is not found");
        }

        public MTextureManager(Main game)
        {
            MGame = game;
        }

        public void AddNewTexture(string name, ICollection<(string Name, Rectangle Range)>? subtextures = null)
        {
            MTexture texture = new(MGame.Content, name, subtextures);
            _textures.Add(texture);
        }
        public void AddTileTexture(string name, Rectangle gridSize)
        {
            MTexture texture = new MTexture(MGame.Content, name).SecAsTile(gridSize);
            _textures.Add(texture);
        }

    }
}
