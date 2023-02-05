using SFS.Parts;
using SFS.Parts.Modules;
using System.Collections.Generic;
using System.Linq;
using System.Reflection;
using HarmonyLib;
using SFS;
using SFS.Variables;
using UnityEngine;

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
            "Placeholder RTG"
        };
        
        public static List<CheckSkinModule> checkSkinModules = new List<CheckSkinModule>();
        
        private static readonly FieldInfo variableName = AccessTools.Field(typeof(ReferenceVariable<string>), "variableName");

        private static String_Reference CreateRef(string name = null, SFS.Variables.VariablesModule variables = null)
        {
            var stringRef = new SFS.Variables.String_Reference()
            {
                referenceToVariables = variables
            };
            variableName.SetValue(stringRef, name);
            var _ = stringRef.Value;

            return stringRef;
        }
        public static void Execute()
        {
            for (int i = 0; i < Base.partsLoader.colorTextures.Count; i++)
            {
                string key = Base.partsLoader.colorTextures.Keys.ToList()[i];
                Base.partsLoader.colorTextures[key].pack_Redstone_Atlas = false;
            }
            foreach (string e in partsToChange)
            {
                bool flag = Base.partsLoader.parts.TryGetValue(e, out Part part);
                if (flag)
                {
                    PipeMesh[] meshes = part.GetComponentsInChildren<PipeMesh>();
                    if (meshes.Length == 0) continue;
                    SkinModule skinModule = part.GetOrAddComponent<SkinModule>();
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
                    
                    skinModule.colorTextureName = CreateRef("color_tex");
                    Debug.Log(skinModule.colorTextureName.Value);
                    skinModule.shapeTextureName = CreateRef("shape_tex");

                    part.GetOrAddComponent<CheckSkinModule>();
                }
            }
        }
        
    }
}