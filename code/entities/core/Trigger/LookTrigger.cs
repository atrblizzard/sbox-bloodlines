using Editor;
using Sandbox;

namespace Vampire.entities.core.Trigger
{
    [Solid]
    [HammerEntity]
    [Library("trigger_look")]
    public partial class LookTrigger : BaseTrigger
    {
        [Property(Title = "Look Time")]
        public float LookTime { get; set; }

        [Property(Title = "Field of View")]
        public float FieldOfView { get; set; }
    }
}
