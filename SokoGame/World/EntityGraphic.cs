namespace SokoGame.World;

public readonly record struct EntityGraphic(
    EntityGraphic.GraphicMode Mode,
    char Character,
    ImageIndex ImageIndex,
    int LayerIndex)
{
    public enum GraphicMode
    {
        Skip = 0,
        Character = 1,
        Image = 2
    }

    public static EntityGraphic CreateCharacter(char character, int layerIndex)
    {
        return new EntityGraphic
        {
            Mode = GraphicMode.Character,
            Character = character,
            LayerIndex = layerIndex
        };
    }

    public static EntityGraphic CreateImage(ImageIndex imageIndex, int layerIndex)
    {
        return new EntityGraphic
        {
            Mode = GraphicMode.Image,
            ImageIndex = imageIndex,
            LayerIndex = layerIndex
        };
    }
}