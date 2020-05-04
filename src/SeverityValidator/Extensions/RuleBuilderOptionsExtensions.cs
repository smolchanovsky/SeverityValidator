using System;
using FluentValidation;
using SeverityValidator.Helpers;

namespace SeverityValidator.Extensions
{
	public static class RuleBuilderOptionsExtensions
	{
		public static IRuleBuilderOptions<T, TProperty> WithMessageAndJsonObject<T, TProperty>(this IRuleBuilderOptions<T, TProperty> ruleBuilderOptions) =>
			ruleBuilderOptions.WithMessage(x => x.ToJson());

		public static IRuleBuilderOptions<T, TProperty> WithMessageAndJsonObject<T, TProperty>(this IRuleBuilderOptions<T, TProperty> ruleBuilderOptions, 
			string message) =>
			ruleBuilderOptions.WithMessage(x => $"{message}\n{x.ToJson()}");

		public static IRuleBuilderOptions<T, TProperty> WithMessageAndJsonObject<T, TProperty>(this IRuleBuilderOptions<T, TProperty> ruleBuilderOptions, 
			Func<T, string> messageFunc) =>
			ruleBuilderOptions.WithMessage(x => $"{messageFunc(x)}\n{x.ToJson()}");
	}
}