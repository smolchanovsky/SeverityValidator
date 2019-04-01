namespace SeverityValidator
{
	public interface IFluentValidatorFactory<T>
	{
		IFluentValidator<T> CreateFluentValidator();
	}

	public class FluentValidatorFactory<T> : IFluentValidatorFactory<T>
	{
		public IFluentValidator<T> CreateFluentValidator()
		{
			return new FluentValidator<T>();
		}
	}
}