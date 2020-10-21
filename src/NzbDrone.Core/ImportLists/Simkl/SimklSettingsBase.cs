using System;
using System.Text.RegularExpressions;
using FluentValidation;
using NzbDrone.Common.Extensions;
using NzbDrone.Core.Annotations;
using NzbDrone.Core.ThingiProvider;
using NzbDrone.Core.Validation;

namespace NzbDrone.Core.ImportLists.Simkl
{
    public class SimklSettingsBaseValidator<TSettings> : AbstractValidator<TSettings>
    where TSettings : SimklSettingsBase<TSettings>
    {
        public SimklSettingsBaseValidator()
        {
            RuleFor(c => c.Link).ValidRootUrl();
            RuleFor(c => c.AccessToken).NotEmpty();
            RuleFor(c => c.RefreshToken).NotEmpty();
            RuleFor(c => c.Expires).NotEmpty();

            // Loose validation @TODO
            RuleFor(c => c.Rating)
                .Matches(@"^\d+\-\d+$", RegexOptions.IgnoreCase)
                .When(c => c.Rating.IsNotNullOrWhiteSpace())
                .WithMessage("Not a valid rating");

            // Any valid certification
            RuleFor(c => c.Certification)
                .Matches(@"^\bNR\b|\bG\b|\bPG\b|\bPG\-13\b|\bR\b|\bNC\-17\b$", RegexOptions.IgnoreCase)
                .When(c => c.Certification.IsNotNullOrWhiteSpace())
                .WithMessage("Not a valid cerification");

            // Loose validation @TODO
            RuleFor(c => c.Years)
                .Matches(@"^\d+(\-\d+)?$", RegexOptions.IgnoreCase)
                .When(c => c.Years.IsNotNullOrWhiteSpace())
                .WithMessage("Not a valid year or range of years");

            // Limit not smaller than 1 and not larger than 100
            RuleFor(c => c.Limit)
                .GreaterThan(0)
                .WithMessage("Must be integer greater than 0");
        }
    }

    public class SimklSettingsBase<TSettings> : IProviderConfig
        where TSettings : SimklSettingsBase<TSettings>
    {
        protected virtual AbstractValidator<TSettings> Validator => new SimklSettingsBaseValidator<TSettings>();

        public SimklSettingsBase()
        {
            SignIn = "startOAuth";
            Rating = "0-100";
            Certification = "NR,G,PG,PG-13,R,NC-17";
            Genres = "";
            Years = "";
            Limit = 100;
        }

        public string Link => "https://api.Simkl.tv";
        public virtual string Scope => "";

        [FieldDefinition(0, Label = "Access Token", Type = FieldType.Textbox, Hidden = HiddenType.Hidden)]
        public string AccessToken { get; set; }

        [FieldDefinition(0, Label = "Refresh Token", Type = FieldType.Textbox, Hidden = HiddenType.Hidden)]
        public string RefreshToken { get; set; }

        [FieldDefinition(0, Label = "Expires", Type = FieldType.Textbox, Hidden = HiddenType.Hidden)]
        public DateTime Expires { get; set; }

        [FieldDefinition(0, Label = "Auth User", Type = FieldType.Textbox, Hidden = HiddenType.Hidden)]
        public string AuthUser { get; set; }

        [FieldDefinition(1, Label = "Rating", HelpText = "Filter movies by rating range (0-100)")]
        public string Rating { get; set; }

        [FieldDefinition(2, Label = "Certification", HelpText = "Filter movies by a certification (NR,G,PG,PG-13,R,NC-17), (Comma Separated)")]
        public string Certification { get; set; }

        [FieldDefinition(3, Label = "Genres", HelpText = "Filter movies by Simkl Genre Slug (Comma Separated)")]
        public string Genres { get; set; }

        [FieldDefinition(4, Label = "Years", HelpText = "Filter movies by year or year range")]
        public string Years { get; set; }

        [FieldDefinition(5, Label = "Limit", HelpText = "Limit the number of movies to get")]
        public int Limit { get; set; }

        [FieldDefinition(6, Label = "Additional Parameters", HelpText = "Additional Simkl API parameters", Advanced = true)]
        public string SimklAdditionalParameters { get; set; }

        [FieldDefinition(99, Label = "Authenticate with Simkl", Type = FieldType.OAuth)]
        public string SignIn { get; set; }

        public NzbDroneValidationResult Validate()
        {
            return new NzbDroneValidationResult(Validator.Validate((TSettings)this));
        }
    }
}
