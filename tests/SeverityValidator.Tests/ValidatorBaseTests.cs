using System;
using System.Collections;
using FluentAssertions;
using FluentAssertions.Equivalency;
using FluentValidation;
using FluentValidation.Results;
using NUnit.Framework;
using SeverityValidator.Extensions;

namespace SeverityValidator.Tests
{
	[TestFixture]
	public class ValidatorBaseTests
	{
		private static Func<EquivalencyAssertionOptions<ValidationFailure>, EquivalencyAssertionOptions<ValidationFailure>> EqualityOptions =>
			opts => opts.Including(x => x.PropertyName).Including(x => x.Severity);

		private ISeverityValidator<TestEntity> validator;

		[SetUp]
		public void SetUp()
		{
			var fluentValidatorFactory = new FluentValidatorFactory<TestEntity>();
			validator = new TestValidator(fluentValidatorFactory);
		}

		[Test, TestCaseSource(typeof(TestSource), nameof(TestSource.ValidEntities))]
		public void Validate_ValidEntity_EmptyErrorsList(TestEntity entity, ValidationResult expectedValidationResult)
		{
			var result = validator.Validate(entity);

			result.IsValid.Should().BeTrue();
			result.Errors.Should().BeEquivalentTo(expectedValidationResult.Errors, EqualityOptions);
		}

		[Test, TestCaseSource(typeof(TestSource), nameof(TestSource.EntitiesWithErrors))]
		public void Validate_EntityWithErrors_ErrorsListWithSeverityError(TestEntity entity, ValidationResult expectedValidationResult)
		{
			var result = validator.Validate(entity);

			result.IsValid.Should().BeFalse();
			result.Errors.Should().BeEquivalentTo(expectedValidationResult.Errors, EqualityOptions);
		}

		[Test, TestCaseSource(typeof(TestSource), nameof(TestSource.EntitiesWithWarnings))]
		public void Validate_EntityWithWarnings_ErrorsListWithSeverityWarning(TestEntity entity, ValidationResult expectedValidationResult)
		{
			var result = validator.Validate(entity);

			result.IsValid.Should().BeFalse();
			result.Errors.Should().BeEquivalentTo(expectedValidationResult.Errors, EqualityOptions);
		}

		[Test, TestCaseSource(typeof(TestSource), nameof(TestSource.EntitiesWithInfo))]
		public void Validate_EntityWithInfo_ErrorsListWithSeverityInfo(TestEntity entity, ValidationResult expectedValidationResult)
		{
			var result = validator.Validate(entity);

			result.IsValid.Should().BeFalse();
			result.Errors.Should().BeEquivalentTo(expectedValidationResult.Errors, EqualityOptions);
		}

		[Test, TestCaseSource(typeof(TestSource), nameof(TestSource.EntitiesWithAllErrorTypes))]
		public void Validate_EntityWithAllErrorTypes_ErrorsList(TestEntity entity, ValidationResult expectedValidationResult)
		{
			var result = validator.Validate(entity);

			result.IsValid.Should().BeFalse();
			result.Errors.Should().BeEquivalentTo(expectedValidationResult.Errors, EqualityOptions);
		}

		[Test, TestCaseSource(typeof(TestSource), nameof(TestSource.ValidEntities))]
		public void ValidateErrors_ValidEntity_EmptyErrorsList(TestEntity entity, ValidationResult expectedValidationResult)
		{
			var result = validator.ValidateErrors(entity);

			result.IsValid.Should().BeTrue();
			result.Errors.Should().BeEquivalentTo(expectedValidationResult.Errors, EqualityOptions);
		}

		[Test, TestCaseSource(typeof(TestSource), nameof(TestSource.ValidEntities))]
		public void ValidateWarnings_ValidEntity_EmptyErrorsList(TestEntity entity, ValidationResult expectedValidationResult)
		{
			var result = validator.ValidateWarnings(entity);

			result.IsValid.Should().BeTrue();
			result.Errors.Should().BeEquivalentTo(expectedValidationResult.Errors, EqualityOptions);
		}

		[Test, TestCaseSource(typeof(TestSource), nameof(TestSource.ValidEntities))]
		public void ValidateInfo_ValidEntity_EmptyErrorsList(TestEntity entity, ValidationResult expectedValidationResult)
		{
			var result = validator.ValidateInfo(entity);

			result.IsValid.Should().BeTrue();
			result.Errors.Should().BeEquivalentTo(expectedValidationResult.Errors, EqualityOptions);
		}

		[Test, TestCaseSource(typeof(TestSource), nameof(TestSource.EntitiesWithAllErrorTypes))]
		public void ValidateErrors_EntityWithAllErrorTypes_ErrorsListWithSeverityError(TestEntity entity, ValidationResult expectedValidationResult)
		{
			var result = validator.ValidateErrors(entity);

			result.IsValid.Should().BeFalse();
			result.Errors.Should().BeEquivalentTo(expectedValidationResult.Errors.WithSeverityError(), EqualityOptions);
		}

		[Test, TestCaseSource(typeof(TestSource), nameof(TestSource.EntitiesWithAllErrorTypes))]
		public void ValidateWarnings_EntityWithAllErrorTypes_ErrorsListWithSeverityWarning(TestEntity entity, ValidationResult expectedValidationResult)
		{
			var result = validator.ValidateWarnings(entity);

			result.IsValid.Should().BeFalse();
			result.Errors.Should().BeEquivalentTo(expectedValidationResult.Errors.WithSeverityWarning(), EqualityOptions);
		}

		[Test, TestCaseSource(typeof(TestSource), nameof(TestSource.EntitiesWithAllErrorTypes))]
		public void ValidateInfo_EntityWithAllErrorTypes_ErrorsListWithSeverityInfo(TestEntity entity, ValidationResult expectedValidationResult)
		{
			var result = validator.ValidateInfo(entity);

			result.IsValid.Should().BeFalse();
			result.Errors.Should().BeEquivalentTo(expectedValidationResult.Errors.WithSeverityInfo(), EqualityOptions);
		}

		public class TestSource
		{
			public static IEnumerable ValidEntities = new[]
			{
				new TestCaseData(new TestEntity
					{
						Id = 1,
						Name = "Name",
						Description = "Description",
						FullDescription = "FullDescription",
						OptionField1 = "Option1",
						OptionField2 = "Option2"
					},
					new ValidationResult(Array.Empty<ValidationFailure>()))
			};

			public static IEnumerable EntitiesWithErrors = new[]
			{
				new TestCaseData(new TestEntity
					{
						Id = null,
						Name = "Name",
						Description = "Description",
						FullDescription = "FullDescription",
						OptionField1 = "Option1",
						OptionField2 = "Option2",
					},
					new ValidationResult(new[]
					{
						new ValidationFailure(nameof(TestEntity.Id), "") { Severity = Severity.Error }
					})),
				new TestCaseData(new TestEntity
					{
						Id = 1,
						Name = null,
						Description = "Description",
						FullDescription = "FullDescription",
						OptionField1 = "Option1",
						OptionField2 = "Option2",
					},
					new ValidationResult(new[]
					{
						new ValidationFailure(nameof(TestEntity.Name), "") { Severity = Severity.Error },
					})),
				new TestCaseData(new TestEntity
					{
						Id = null,
						Name = null,
						Description = "Description",
						FullDescription = "FullDescription",
						OptionField1 = "Option1",
						OptionField2 = "Option2",
					},
					new ValidationResult(new[]
					{
						new ValidationFailure(nameof(TestEntity.Id), "") { Severity = Severity.Error },
						new ValidationFailure(nameof(TestEntity.Name), "") { Severity = Severity.Error },
					})),
			};

			public static IEnumerable EntitiesWithWarnings = new[]
			{
				new TestCaseData(new TestEntity
					{
						Id = 1,
						Name = "Name",
						Description = null,
						FullDescription = "FullDescription",
						OptionField1 = "Option1",
						OptionField2 = "Option2",
					},
					new ValidationResult(new[]
					{
						new ValidationFailure(nameof(TestEntity.Description), "") { Severity = Severity.Warning }
					})),
				new TestCaseData(new TestEntity
					{
						Id = 1,
						Name = "Name",
						Description = "Description",
						FullDescription = null,
						OptionField1 = "Option1",
						OptionField2 = "Option2",
					},
					new ValidationResult(new[]
					{
						new ValidationFailure(nameof(TestEntity.FullDescription), "") { Severity = Severity.Warning },
					})),
				new TestCaseData(new TestEntity
					{
						Id = 1,
						Name = "Name",
						Description = null,
						FullDescription = null,
						OptionField1 = "Option1",
						OptionField2 = "Option2",
					},
					new ValidationResult(new[]
					{
						new ValidationFailure(nameof(TestEntity.Description), "") { Severity = Severity.Warning },
						new ValidationFailure(nameof(TestEntity.FullDescription), "") { Severity = Severity.Warning },
					})),
			};

			public static IEnumerable EntitiesWithInfo = new[]
			{
				new TestCaseData(new TestEntity
					{
						Id = 1,
						Name = "Name",
						Description = "Description",
						FullDescription = "FullDescription",
						OptionField1 = null,
						OptionField2 = "Option1",
					},
					new ValidationResult(new[]
					{
						new ValidationFailure(nameof(TestEntity.OptionField1), "") { Severity = Severity.Info }
					})),
				new TestCaseData(new TestEntity
					{
						Id = 1,
						Name = "Name",
						Description = "Description",
						FullDescription = "FullDescription",
						OptionField1 = "Option1",
						OptionField2 = null,
					},
					new ValidationResult(new[]
					{
						new ValidationFailure(nameof(TestEntity.OptionField2), "") { Severity = Severity.Info }
					})),
				new TestCaseData(new TestEntity
					{
						Id = 1,
						Name = "Name",
						Description = "Description",
						FullDescription = "FullDescription",
						OptionField1 = null,
						OptionField2 = null,
					},
					new ValidationResult(new[]
					{
						new ValidationFailure(nameof(TestEntity.OptionField1), "") { Severity = Severity.Info },
						new ValidationFailure(nameof(TestEntity.OptionField2), "") { Severity = Severity.Info }
					})),
			};

			public static IEnumerable EntitiesWithAllErrorTypes = new[]
			{
				new TestCaseData(new TestEntity(),
					new ValidationResult(new[]
					{
						new ValidationFailure(nameof(TestEntity.Id), "") { Severity = Severity.Error },
						new ValidationFailure(nameof(TestEntity.Name), "") { Severity = Severity.Error },
						new ValidationFailure(nameof(TestEntity.Description), "") { Severity = Severity.Warning },
						new ValidationFailure(nameof(TestEntity.FullDescription), "") { Severity = Severity.Warning },
						new ValidationFailure(nameof(TestEntity.OptionField1), "") { Severity = Severity.Info },
						new ValidationFailure(nameof(TestEntity.OptionField2), "") { Severity = Severity.Info }
					})),
			};
		}

		public class TestValidator : SeverityValidatorBase<TestEntity>
		{
			public TestValidator(IFluentValidatorFactory<TestEntity> fluentValidatorFactory) : base(fluentValidatorFactory)
			{
			}

			protected override void RulesForErrors(IFluentValidator<TestEntity> fluentValidator)
			{
				fluentValidator.ErrorRuleFor(x => x.Id).Cascade(CascadeMode.Continue).NotNull();
				fluentValidator.ErrorRuleFor(x => x.Name).Cascade(CascadeMode.Continue).NotNull();
			}

			protected override void RulesForWarnings(IFluentValidator<TestEntity> fluentValidator)
			{
				fluentValidator.WarningRuleFor(x => x.Description).NotNull();
				fluentValidator.WarningRuleFor(x => x.FullDescription).NotNull();
			}

			protected override void RulesForInfo(IFluentValidator<TestEntity> fluentValidator)
			{
				fluentValidator.InfoRuleFor(x => x.OptionField1).NotNull();
				fluentValidator.InfoRuleFor(x => x.OptionField2).NotNull();
			}
		}

		public class TestEntity
		{
			public int? Id { get; set; }
			public string Name { get; set; }
			public string Description { get; set; }
			public string FullDescription { get; set; }
			public string OptionField1 { get; set; }
			public string OptionField2 { get; set; }
		}
	}
}