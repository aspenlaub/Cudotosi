using System;
using System.Windows;
using Aspenlaub.Net.GitHub.CSharp.Cudotosi.Interfaces;

namespace Aspenlaub.Net.GitHub.CSharp.Cudotosi.Helpers {
    public class FakeUserInteraction : IUserInteraction {
        public string FolderToReturn { get; set; }

        public string PromptForFolder(string folder) {
            return FolderToReturn;
        }

        public MessageBoxResult ShowMessageBox(string text, MessageBoxButton button, MessageBoxImage icon) {
            switch (button) {
                case MessageBoxButton.YesNoCancel:
                case MessageBoxButton.YesNo:
                    return MessageBoxResult.No;
                case MessageBoxButton.OK:
                case MessageBoxButton.OKCancel:
                    return MessageBoxResult.OK;
                default:
                    throw new NotImplementedException($"Cannot handle message box button type {Enum.GetName(typeof(MessageBoxButton), button)}");
            }
        }
    }
}
