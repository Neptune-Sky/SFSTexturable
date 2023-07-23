using System.Collections.Generic;
using System.Linq;
using HarmonyLib;
using ModLoader;
using SFS;
using UnityEngine;
using ModLoader.Helpers;
using ModLoader.IO;

namespace Texturable
{
    public class Main : Mod
    {

        public override string ModNameID => "SFSTexturable";
        public override string DisplayName => "Texturable";
        public override string Author => "NeptuneSky";
        public override string MinimumGameVersionNecessary => "1.5.9.8";
        public override string ModVersion => "v0.2.1-beta";
        public override string Description => "A simple mod that makes more parts textureable.";

        // This initializes the patcher. This is required if you use any Harmony patches.
        private static Harmony patcher;

        public override void Load()
        {
            // This tells the loader what to run when your mod is loaded.
            AddModules.Execute();
        }

        public override void Early_Load()
        {
            patcher = new Harmony("Neptune.Texturable.Mod");
        }

        private bool Command(string str)
        {
            var splitCommand = str.Split(' ').ToList();
            string command = splitCommand[0];
            var args = splitCommand.Count < 2 ? new List<string>() : splitCommand.GetRange(1, str.Length - 1);

            if (command != "skin") return false;
            if (args.Count < 1)
            {
                Debug.Log("Not enough args! You must provide the skin to change to.");
                return true;
            }
            AddModules.checkSkinModules.ForEach(e =>
            {
                e.skinModule.SetTexture(0, Base.partsLoader.colorTextures[args[0]].colorTex);
            });

            return false;
        }
    }
}