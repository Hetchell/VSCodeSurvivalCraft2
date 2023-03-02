using Game;
using GameEntitySystem;
using TemplatesDatabase;

namespace CreatorWandModAPI
{
    public class CreatorWandModAPIEditPaletteDialog : EditPaletteDialog
    {
        private readonly ButtonWidget okButton;

        public CreatorWandModAPIEditPaletteDialog(WorldPalette palette)
            : base(palette)
        {
            okButton = Children.Find<ButtonWidget>("EditPaletteDialog.OK");
        }

        public override void Update()
        {
            base.Update();
            if (okButton.IsClicked)
            {
                ((Subsystem)GameManager.Project.FindSubsystem<SubsystemPalette>()).Load((ValuesDictionary)null);
            }
        }
    }
}