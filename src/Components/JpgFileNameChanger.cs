using System;
using Aspenlaub.Net.GitHub.CSharp.Cudotosi.Entities;
using Aspenlaub.Net.GitHub.CSharp.Cudotosi.Interfaces;

namespace Aspenlaub.Net.GitHub.CSharp.Cudotosi.Components;

public class JpgFileNameChanger : IJpgFileNameChanger {
    public string ChangeFileName(string xlFileName, BootstrapSizes size, bool preview) {
        if (string.IsNullOrEmpty(xlFileName)) { return xlFileName; }

        if (!xlFileName.Contains("_XL")) {
            throw new ArgumentException(nameof(xlFileName));
        }

        return xlFileName.Replace("_XL", "_" + (preview ? "vs_" : "") + Enum.GetName(typeof(BootstrapSizes), size)?.ToUpper());
    }
}