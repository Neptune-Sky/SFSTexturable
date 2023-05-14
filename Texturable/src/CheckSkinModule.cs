using System;
using System.Collections.Generic;
using SFS;
using SFS.Parts.Modules;
using SFS.Variables;
using UnityEngine;

namespace Texturable
{
    [Serializable]
    public class ColorSaver
    {
        public Mode mode;
        public Color color;
    }
    public class CheckSkinModule : MonoBehaviour
    {
        public SkinModule skinModule;
        private VariablesModule variables;

        private bool toggle;
        private readonly List<ColorSaver> defaultColorsList = new();

        private void Start()
        {   
            AddModules.checkSkinModules.Add(this);
            if (!TryGetComponent(out skinModule)) return;
            variables = GetComponent<VariablesModule>();
            var colorTexture = "";
            var shapeTexture = "";
            try
            {
                colorTexture = variables.stringVariables.GetValue("color_tex");
                shapeTexture = variables.stringVariables.GetValue("shape_tex");
            }
            catch (Exception)
            {
                // ignored
            }

            if (!string.IsNullOrEmpty(colorTexture))
            {
                skinModule.SetTexture(0, Base.partsLoader.colorTextures[colorTexture].colorTex);
            }

            if (!string.IsNullOrEmpty(shapeTexture))
            {
                skinModule.SetTexture(1, Base.partsLoader.shapeTextures[shapeTexture].shapeTex);
            }
            skinModule.meshModules.ForEach(e =>
            {
                defaultColorsList.Add(new ColorSaver
                {
                    mode = e.colors.mode,
                    color = e.colors.color.colorBasic,
                });
            });
            if (!variables.boolVariables.GetValue("disable_vanilla_shading")) return;
            
            skinModule.meshModules.ForEach(e =>
            {
                e.colors.mode = Mode.Single;
                e.colors.color.colorBasic = Color.white;
                e.GenerateMesh();
            });
            toggle = true;
            
        }

        private void OnDestroy()
        {
            AddModules.checkSkinModules.Remove(this);
        }

        private void Update()
        {
            if (variables.boolVariables.GetValue("disable_vanilla_shading") && !toggle)
            {
                skinModule.meshModules.ForEach(e =>
                {
                    e.colors.mode = Mode.Single;
                    e.colors.color.colorBasic = Color.white;
                    e.GenerateMesh();
                });
                toggle = true;
            }

            if (!variables.boolVariables.GetValue("disable_vanilla_shading") && toggle)
            {
                for (var i = 0; i < skinModule.meshModules.Length; i++)
                {
                    PipeMesh mesh = skinModule.meshModules[i];
                    mesh.colors.color.colorBasic = defaultColorsList[i].color;
                    mesh.colors.mode = defaultColorsList[i].mode;
                    mesh.GenerateMesh();
                }
                toggle = false;
            }
            string colorTexture = variables.stringVariables.GetValue("color_tex");
            string shapeTexture = variables.stringVariables.GetValue("shape_tex");
            
            if (skinModule.colorTextureName == null && colorTexture != "" && colorTexture != "R Stripes")
            {
                skinModule.SetTexture(0, Base.partsLoader.colorTextures[colorTexture].colorTex);
            }
            else if (skinModule.colorTextureName.Value != colorTexture)
            {
                variables.stringVariables.SetValue("color_tex", skinModule.colorTextureName.Value, (true, true));
            }

            if (skinModule.shapeTextureName == null && shapeTexture != "")
            {
                skinModule.SetTexture(1, Base.partsLoader.shapeTextures[shapeTexture].shapeTex);
            }
            else if (skinModule.shapeTextureName.Value != shapeTexture)
            {
                variables.stringVariables.SetValue("shape_tex", skinModule.shapeTextureName.Value, (true, true));
            }
        }
    }
}