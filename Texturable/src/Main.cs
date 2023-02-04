using HarmonyLib;
using ModLoader;

namespace Texturable
{
    public class Main : Mod
    {

        public override string ModNameID => "SFSTexturable";
        public override string DisplayName => "Texturable";
        public override string Author => "NeptuneSky";
        public override string MinimumGameVersionNecessary => "1.5.9.8";
        public override string ModVersion => "v1.0.0";
        public override string Description => "A simple mod that makes more parts textureable.";

        // This initializes the patcher. This is required if you use any Harmony patches.
        private static Harmony patcher;

        public override void Load()
        {
            // This tells the loader what to run when your mod is loaded.
        }

        public override void Early_Load()
        {
            // This method runs before anything from the game is loaded. This is where you should apply your patches, as shown below.

            // The patcher uses an ID formatted like a web domain.
            patcher = new Harmony("Neptune.Texturable.Mod");

            // This pulls your Harmony patches from everywhere in the namespace and applies them.
            patcher.PatchAll();
        }
    }
}