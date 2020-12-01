using Aspenlaub.Net.GitHub.CSharp.Cudotosi.Interfaces;

namespace Aspenlaub.Net.GitHub.CSharp.Cudotosi.Test.Helpers {
    public class FakeFolderDialog : IFolderDialog {
        public string FolderToReturn { get; set; }


        public string PromptForFolder(string folder) {
            return FolderToReturn;
        }
    }
}
