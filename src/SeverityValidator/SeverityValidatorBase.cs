using System;
using FluentValidation;
using FluentValidation.Results;

namespace SeverityValidator
{
	public interface ISeverityValidator<in T>
	{
		ValidationResult Validate(T instance);
		ValidationResult ValidateErrors(T instance);
		ValidationResult ValidateWarnings(T instance);
		ValidationResult ValidateInfo(T instance);
	}

	public abstract class SeverityValidatorBase<T> : ISeverityValidator<T>
	{
		private readonly Lazy<IFluentValidator<T>> lazyFluentValidator;

		protected SeverityValidatorBase(IFluentValidatorFactory<T> fluentValidatorFactory)
		{
			lazyFluentValidator = new Lazy<IFluentValidator<T>>(() =>
			{
				var fluentValidator = fluentValidatorFactory.CreateFluentValidator();

				fluentValidator.AddSeverityRules(Severity.Error, () => RulesForErrors(fluentValidator));
				fluentValidator.AddSeverityRules(Severity.Warning, () => RulesForWarnings(fluentValidator));
				fluentValidator.AddSeverityRules(Severity.Info, () => RulesForInfo(fluentValidator));

				return fluentValidator;
			});
		}

		public ValidationResult Validate(T instance) =>
			lazyFluentValidator.Value.Validate(instance);

		public ValidationResult ValidateErrors(T instance) => 
			lazyFluentValidator.Value.Validate(instance, ruleSet: Severity.Error.ToString());

		public ValidationResult ValidateWarnings(T instance) =>
			lazyFluentValidator.Value.Validate(instance, ruleSet: Severity.Warning.ToString());

		public ValidationResult ValidateInfo(T instance) =>
			lazyFluentValidator.Value.Validate(instance, ruleSet: Severity.Info.ToString());

		protected virtual void RulesForErrors(IFluentValidator<T> fluentValidator)
		{
		}

		protected virtual void RulesForWarnings(IFluentValidator<T> fluentValidator)
		{
		}

		protected virtual void RulesForInfo(IFluentValidator<T> fluentValidator)
		{
		}
	}
}