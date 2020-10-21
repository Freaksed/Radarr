using FluentValidation;
using NzbDrone.Core.Annotations;

namespace NzbDrone.Core.ImportLists.Simkl.List
{
    public class SimklListSettingsValidator : SimklSettingsBaseValidator<SimklListSettings>
    {
        public SimklListSettingsValidator()
        : base()
        {
        }
    }

    public class SimklListSettings : SimklSettingsBase<SimklListSettings>
    {
        protected override AbstractValidator<SimklListSettings> Validator => new SimklListSettingsValidator();

        public SimklListSettings()
        {
        }

        [FieldDefinition(1, Label = "Username", Privacy = PrivacyLevel.UserName, HelpText = "Username for the List to import from")]
        public string Username { get; set; }

        [FieldDefinition(2, Label = "List Name", HelpText = "List name for import, list must be public or you must have access to the list")]
        public string Listname { get; set; }
    }
}
