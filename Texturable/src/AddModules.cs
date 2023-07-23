using System.Collections.Generic;
using System.Linq;
using System.Reflection;
using HarmonyLib;
using SFS;
using SFS.Parts;
using SFS.Parts.Modules;
using SFS.Variables;

namespace Texturable
{
    public static class AddModules
    {
        private static readonly List<string> partsToChange = new List<string> 
        {
            "Parachute",
            "Heat Shield",
            "Heat Shield Hollow",
            "Docking Port",
            "Landing Leg",
            "Landing Leg Big",
            "Strut",
            "Strut 2",
            "Solar Array 2",
            "Solar Array 3",
            "Probe",
            "Placeholder Battery",
            "Placeholder RTG",
            "RA LES",
            "R Engine",
            "RA Retro",
            "R Engine",
            "A Separator",
            "R Separator",
            "A Base",
            "A Bulkhead",
            "A Adapter"
        };
        
        public static List<CheckSkinModule> checkSkinModules = new List<CheckSkinModule>();
        
        private static readonly FieldInfo variableName = AccessTools.Field(typeof(ReferenceVariable<string>), "variableName");

        private static String_Reference CreateRef(string name = null, VariablesModule variables = null)
        {
            var stringRef = new String_Reference
            {
                referenceToVariables = variables
            };
            variableName.SetValue(stringRef, name);
            string _ = stringRef.Value;

            return stringRef;
        }
        public static void Execute()
        {
            for (var i = 0; i < Base.partsLoader.colorTextures.Count; i++)
            {
                string key = Base.partsLoader.colorTextures.Keys.ToList()[i];
                Base.partsLoader.colorTextures[key].pack_Redstone_Atlas = false;
            }
            foreach (string e in partsToChange)
            {
                bool flag = Base.partsLoader.parts.TryGetValue(e, out Part part);
                if (!flag) continue;
                var meshes = part.GetComponentsInChildren<PipeMesh>();
                if (meshes.Length == 0) continue;
                var skinModule = part.GetOrAddComponent<SkinModule>();
                skinModule.skinTag = "tank";
                    
                skinModule.meshModules = meshes;
                    
                if (!part.variablesModule.stringVariables.GetVariableNameList().Contains("color_tex"))
                {
                    part.variablesModule.stringVariables.SetValue("color_tex", "_", (true, true));
                }

                if (!part.variablesModule.stringVariables.GetVariableNameList().Contains("shape_tex"))
                {
                    part.variablesModule.stringVariables.SetValue("shape_tex", "_", (true, true));
                }

                if (!part.variablesModule.boolVariables.Has("disable_vanilla_shading"))
                {
                    part.variablesModule.boolVariables.SetValue("disable_vanilla_shading", false, (true, true));
                }
                    
                skinModule.colorTextureName = CreateRef("color_tex");
                skinModule.shapeTextureName = CreateRef("shape_tex");

                part.GetOrAddComponent<CheckSkinModule>();
            }
        }
        
    }
}