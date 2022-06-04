using MoreMountains.NiceVibrations;

public class FallHapticHandler : AbstractFallFXHandler
{
    protected override void Play()
    {
        MMVibrationManager.Haptic(HapticTypes.MediumImpact);
    }
}
