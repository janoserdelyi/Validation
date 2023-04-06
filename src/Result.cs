namespace com.janoserdelyi.Validation;

public class Result<T>
{
	public bool IsSuccess { get; }
	public bool IsFailure => !IsSuccess;
	public int ErrorCode { get; }
	public T? Value { get; }
	public string? ErrorMessage { get; } // optional?

	protected internal Result (
		bool isSuccess,
		int errorCode,
		string? errorMessage = null
	) {
		if (isSuccess && errorCode != (int)Error.None) {
			throw new InvalidOperationException ();
		}

		if (!isSuccess && errorCode == (int)Error.None) {
			throw new InvalidOperationException ();
		}

		IsSuccess = isSuccess;
		ErrorCode = errorCode;
		ErrorMessage = errorMessage;
	}

	protected internal Result (
		T value,
		bool isSuccess,
		int errorCode,
		string? errorMessage = null
	) {
		if (isSuccess && errorCode != (int)Error.None) {
			throw new InvalidOperationException ();
		}

		if (!isSuccess && errorCode == (int)Error.None) {
			throw new InvalidOperationException ();
		}

		Value = value;
		IsSuccess = isSuccess;
		ErrorCode = errorCode;
		ErrorMessage = errorMessage;
	}

	public static Result<TValue> Success<TValue> (
		TValue value
	) {
		return new Result<TValue> (value, true, (int)Error.None);
	}

	public static Result<TValue> Failure<TValue> (
		int error,
		string? errorMessage = null
	) {
		return new Result<TValue> (false, error, errorMessage);
	}

	public static Result<TValue> Evaluate<TValue> (
		TValue? value
	) {
		if (value != null) {
			return Success (value);
		}

		return Failure<TValue> ((int)Error.Empty, "No value provided");
	}
}

// i should roll these in to Result
public static class ResultExtensions
{
	public static Result<T> Ensure<T> (
		this Result<T> result,
		Func<T?, bool> predicate,
		int error,
		string? errorMessage = null
	) {
		if (result.IsFailure == true) {
			return result;
		}

		if (predicate (result.Value) == true) {
			return result;
		}

		// return a new result with the error since this was not successful
		return Result<T>.Failure<T> (error, errorMessage);
	}

	// we need to map from one thing to another for final outputs
	public static Result<TOut> Map<TIn, TOut> (
		this Result<TIn> result,
		Func<TIn?, TOut> mappingFunc
	) {
		if (result.IsSuccess) {
			return Result<TIn>.Success (mappingFunc (result.Value));
		}

		return Result<TOut>.Failure<TOut> (result.ErrorCode, result.ErrorMessage);
	}
}