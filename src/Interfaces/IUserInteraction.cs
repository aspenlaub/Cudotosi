using System.Windows;

namespace Aspenlaub.Net.GitHub.CSharp.Cudotosi.Interfaces;

public interface IUserInteraction {
    string PromptForFolder(string folder);
    MessageBoxResult ShowMessageBox(string text, MessageBoxButton button, MessageBoxImage icon);
}