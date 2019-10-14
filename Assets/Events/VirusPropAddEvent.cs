
namespace Events
{
    public struct VirusPropAddEvent
    {
        public VirusPropEnum PropEnum;
        public float Duration;

        public VirusPropAddEvent(float duration, VirusPropEnum propEnum)
        {
            Duration = duration;
            PropEnum = propEnum;
        }
    }
}
