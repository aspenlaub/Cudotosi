using System;
using Aspenlaub.Net.GitHub.CSharp.Cudotosi.Entities;
using Aspenlaub.Net.GitHub.CSharp.Cudotosi.Interfaces;

namespace Aspenlaub.Net.GitHub.CSharp.Cudotosi.Components;

public class JpgFileNameChanger : IJpgFileNameChanger {
    public string ChangeFileName(string xxlFileName, BootstrapSizes size, bool preview) {
        if (string.IsNullOrEmpty(xxlFileName)) { return xxlFileName; }

        if (!xxlFileName.Contains("_XXL")) {
            throw new ArgumentException(nameof(xxlFileName));
        }

        return xxlFileName.Replace("_XXL", "_" + (preview ? "vs_" : "") + Enum.GetName(typeof(BootstrapSizes), size)?.ToUpper());
    }
}