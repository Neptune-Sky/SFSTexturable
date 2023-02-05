using System;
using SFS;
using SFS.Parts.Modules;
using SFS.Variables;
using UnityEngine;

namespace Texturable
{
    public class CheckSkinModule : MonoBehaviour
    {
        public SkinModule skinModule;
        private VariablesModule variables;

        void Start()
        {   
            AddModules.checkSkinModules.Add(this);
            if (TryGetComponent(out skinModule))
            {
                variables = GetComponent<VariablesModule>();
                string colorTexture = "";
                string shapeTexture = "";
                try
                {
                    colorTexture = variables.stringVariables.GetValue("color_tex");
                    shapeTexture = variables.stringVariables.GetValue("shape_tex");
                }
                catch (Exception) {}
                if (!string.IsNullOrEmpty(colorTexture))
                {
                    skinModule.SetTexture(0, Base.partsLoader.colorTextures[colorTexture].colorTex);
                }

                if (!string.IsNullOrEmpty(shapeTexture))
                {
                    skinModule.SetTexture(1, Base.partsLoader.shapeTextures[shapeTexture].shapeTex);
                }
                
            }
        }

        private void OnDestroy()
        {
            AddModules.checkSkinModules.Remove(this);
        }

        private void Update()
        {
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