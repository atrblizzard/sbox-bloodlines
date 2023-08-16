using Sandbox;

namespace Vampire.entities.core.Trigger
{
    public partial class BaseTrigger : Sandbox.BaseTrigger
    {
        [Property(Title = "Start Disabled")]
        public bool StartDisabled { get; set; } = false;
    }
}
