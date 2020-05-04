using System;
using System.Linq;
using System.Linq.Expressions;
using FluentValidation;
using FluentValidation.Internal;

namespace SeverityValidator
{
	public interface IFluentValidator<T> : IValidator<T>
	{
		void AddSeverityRules(Severity severity, Action action);
		IRuleBuilderInitial<T, TProperty> RuleFor<TProperty>(Expression<Func<T, TProperty>> expression);
	}

	public class FluentValidator<T> : AbstractValidator<T>, IFluentValidator<T>
	{
		public void AddSeverityRules(Severity severity, Action action)
		{
			RuleSet(severity.ToString(), action);

			foreach (PropertyRule rule in this)
			{
				if (rule.RuleSets.Any(x => string.Equals(x, severity.ToString())))
					rule.CurrentValidator.Options.Severity = severity;
			}
		}
	}
}