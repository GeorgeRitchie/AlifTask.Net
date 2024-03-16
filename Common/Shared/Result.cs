namespace AlifTask.Common.Shared
{
	public class Result
	{
		public bool IsSuccess { get; set; }
		public List<Error> Errors { get; private set; }

		public static Result Success() => new() { IsSuccess = true, Errors = new() };
		public static Result<T> Success<T>(T value) => new Result<T>() { IsSuccess = true, Value = value, Errors = new() };
		public static Result Failure(List<Error>? errors = null) => new() { IsSuccess = false, Errors = errors ?? new() };
		public static Result<T> Failure<T>(T? value = default, List<Error>? errors = null) => new Result<T>() { IsSuccess = false, Value = value, Errors = errors ?? new() };

		public virtual Result ToFailure(List<Error> errors)
		{
			return new Result()
			{
				IsSuccess = false,
				Errors = new List<Error>(Errors.Concat(errors))
			};
		}
	}

	public class Result<T> : Result
	{
		public T? Value { get; set; }

		public static implicit operator T?(Result<T> value) => value.Value;

		public override Result<T> ToFailure(List<Error> errors)
		{
			return Failure(Value, new List<Error>(Errors.Concat(errors)));
		}
	}
}
