using System.Collections.Generic;
using System.Linq;
using FluentValidation;
using FluentValidation.Results;

namespace SeverityValidator.Extensions
{
	public static class ValidationResultExtensions
	{
		public static ValidationFailure[] WithSeverityError(this IEnumerable<ValidationFailure> errors) =>
			errors.GetErrorsBySeverity(Severity.Error);

		public static ValidationFailure[] WithSeverityWarning(this IEnumerable<ValidationFailure> errors) =>
			errors.GetErrorsBySeverity(Severity.Warning);

		public static ValidationFailure[] WithSeverityInfo(this IEnumerable<ValidationFailure> errors) =>
			errors.GetErrorsBySeverity(Severity.Info);

		private static ValidationFailure[] GetErrorsBySeverity(this IEnumerable<ValidationFailure> source, Severity severity) => 
			source.Where(x => x.Severity == severity).ToArray();
	}
}