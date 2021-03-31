using CIS.HR.Models;
using FluentValidation;
using CIS.HR.ViewModels;
using CIS.HR.Validators;
using System.ComponentModel.DataAnnotations;
using System;
using CIS.HR.Models.DTO;

namespace CIS.HR
{
    namespace ViewModels
    {
        public class PositionViewModel
        {
            public PositionViewModel()
            { }

            public PositionViewModel( PositionDTO positionDto )
            {
                PositionId = positionDto.PositionId;
                PositionDescriptionId = positionDto.PositionDescriptionId;
                ClassificationId = positionDto.ClassificationId;
                Code = positionDto.Code;
                Title = positionDto.Title;
                LastUpdated = positionDto.LastUpdated.ToShortDateString();
                Classification = positionDto.Classification;
            }

            public int PositionId { get; set; }
            public int PositionDescriptionId { get; set; }
            public int ClassificationId { get; set; }
            public string Code { get; set; }
            public string Title { get; set; }
            public string LastUpdated { get; set; }
            public string Classification { get; set; }
        }

        [FluentValidation.Attributes.Validator(typeof(CreatePositionViewModelValidator))]
        public class CreatePositionViewModel
        {
            public string Code { get; set; }
            public string Title { get; set; }
            public DateTime? DateEffective { get; set; }
        }
    }

    namespace Validators
    {
        public class CreatePositionViewModelValidator : AbstractValidator<CreatePositionViewModel>
        {
            public CreatePositionViewModelValidator()
            {
                RuleFor(x => x.Code)
                    .NotNull()
                    .Length(1, 3)
                    .WithMessage("'Code' must be between 1 and 3 characters.");
                RuleFor(x => x.Title)
                    .NotNull();
                RuleFor(x => x.DateEffective)
                    .NotNull();
                RuleFor(x => x.DateEffective)
                    .NotEmpty()
                    .OverridePropertyName("Effective Date");
            }
        }
    }
}