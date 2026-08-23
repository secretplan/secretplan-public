namespace SecretPlanGodot.Serialization;

[AttributeUsage(AttributeTargets.Property | AttributeTargets.Field)]
public class SettingsRangeAttribute : Attribute
{
    public float Max { get; set; } = 0;
    public float Min { get; set; } = 1f;
    public float Step { get; set; } = 0.01f;
    public bool RoundToWholeNumbers { get; set; } = false;

    public SliderValues GetSliderValues()
    {
        return new()
        {
            Max = Max,
            Min = Min,
            Step = Step,
            RoundToWholeNumbers = RoundToWholeNumbers
        };
    }
}