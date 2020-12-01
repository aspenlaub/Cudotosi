using System.Threading.Tasks;
using Aspenlaub.Net.GitHub.CSharp.Cudotosi.Commands;
using Aspenlaub.Net.GitHub.CSharp.Cudotosi.Handlers;
using Aspenlaub.Net.GitHub.CSharp.Cudotosi.Interfaces;
using Aspenlaub.Net.GitHub.CSharp.VishizhukelNet.Application;
using Aspenlaub.Net.GitHub.CSharp.VishizhukelNet.Interfaces;

namespace Aspenlaub.Net.GitHub.CSharp.Cudotosi.Application {
    public class CudotosiApplication : ApplicationBase<IGuiAndApplicationSynchronizer<ICudotosiApplicationModel>, ICudotosiApplicationModel> {
        public ICudotosiHandlers Handlers { get; private set; }
        public ICudotosiCommands Commands { get; private set; }

        public CudotosiApplication(IButtonNameToCommandMapper buttonNameToCommandMapper,
                IGuiAndApplicationSynchronizer<ICudotosiApplicationModel> guiAndApplicationSynchronizer,
                ICudotosiApplicationModel model)
            : base(buttonNameToCommandMapper, guiAndApplicationSynchronizer, model) {
        }

        protected override async Task EnableOrDisableButtonsAsync() {
            Model.Save.Enabled = await Commands.SaveCommand.ShouldBeEnabledAsync();
        }

        public override void RegisterTypes() {
            Handlers = new CudotosiHandlers();
            Commands = new CudotosiCommands {
                SaveCommand = new SaveCommand(Model)
            };
        }
    }
}