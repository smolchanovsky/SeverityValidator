using System;
using System.Linq;
using System.Linq.Expressions;
using System.Threading;
using System.Threading.Tasks;
using FluentValidation;
using FluentValidation.Internal;
using FluentValidation.Results;

namespace SeverityValidator
{
	public interface IFluentValidator<T> : IValidator<T>
	{
		void AddSeverityRules(Severity severity, Action action);
		IRuleBuilderInitial<T, TProperty> ErrorRuleFor<TProperty>(Expression<Func<T, TProperty>> expression);
		IRuleBuilderInitial<T, TProperty> WarningRuleFor<TProperty>(Expression<Func<T, TProperty>> expression);
		IRuleBuilderInitial<T, TProperty> InfoRuleFor<TProperty>(Expression<Func<T, TProperty>> expression);
	}

	public class FluentValidator<T> : AbstractValidator<T>, IFluentValidator<T>
	{
		private const string commonRuleSetName = "Common";

		public new ValidationResult Validate(T instance) =>
			this.Validate(instance, ruleSet: commonRuleSetName);

		public new Task<ValidationResult> ValidateAsync(T instance, CancellationToken cancellation = new CancellationToken()) =>
			this.ValidateAsync(instance, cancellation, ruleSet: commonRuleSetName);

		public IRuleBuilderInitial<T, TProperty> ErrorRuleFor<TProperty>(Expression<Func<T, TProperty>> expression) =>
			GetRuleBuilder(expression, CascadeMode.StopOnFirstFailure);

		public IRuleBuilderInitial<T, TProperty> WarningRuleFor<TProperty>(Expression<Func<T, TProperty>> expression) =>
			GetRuleBuilder(expression, CascadeMode.Continue);

		public IRuleBuilderInitial<T, TProperty> InfoRuleFor<TProperty>(Expression<Func<T, TProperty>> expression) =>
			GetRuleBuilder(expression, CascadeMode.Continue);

		private IRuleBuilderInitial<T, TProperty> GetRuleBuilder<TProperty>(Expression<Func<T, TProperty>> expression, CascadeMode cascadeMode)
		{
			if (expression is null)
				throw new ArgumentNullException(nameof(expression), "Cannot pass null to RuleFor");

			var rule = PropertyRule.Create(expression, () => cascadeMode);
			AddRule(rule);

			return new RuleBuilder<T, TProperty>(rule, this);
		}

		public void AddSeverityRules(Severity severity, Action action)
		{
			RuleSet(string.Join(",", severity.ToString(), commonRuleSetName), action);

			foreach (PropertyRule rule in this)
			{
				if (rule.RuleSets.Any(x => string.Equals(x, severity.ToString())))
					rule.CurrentValidator.Options.Severity = severity;
			}
		}
	}
}