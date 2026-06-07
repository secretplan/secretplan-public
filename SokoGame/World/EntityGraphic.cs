namespace SokoGame.World;

public readonly record struct EntityGraphic(
    EntityGraphic.GraphicMode Mode,
    char Character,
    ImagePageIndex ImagePageIndex,
    int LayerIndex,
    string? Color,
    string? SecondaryColor,
    EntityContinuousAnimation Animation)
{
    public enum GraphicMode
    {
        Skip = 0,
        Character = 1,
        Sprite = 2,

        /// <summary>
        ///     Intentionally un-draws whatever is at this location
        /// </summary>
        Clear = 3
    }

    public static EntityGraphic CreateCharacter(char character, int layerIndex, string color)
    {
        return new EntityGraphic
        {
            Mode = GraphicMode.Character,
            Character = character,
            LayerIndex = layerIndex,
            Color = color,
        };
    }

    public static EntityGraphic CreateImage(ImagePageIndex imagePageIndex, int layerIndex, string color, string? secondaryColor = null)
    {
        return new EntityGraphic
        {
            Mode = GraphicMode.Sprite,
            ImagePageIndex = imagePageIndex,
            LayerIndex = layerIndex,
            Color = color,
            SecondaryColor = secondaryColor
        };
    }
}