namespace com.janoserdelyi.Validation;

public class Result<T>
{
	public bool IsSuccess { get; }
	public bool IsFailure => !IsSuccess;
	public int ErrorCode { get; }
	public T? Value { get; }
	public string? ErrorMessage { get; }
	public string? DetailedErrorMessage { get; } // for stack traces and the like. non-public-facing error details

	protected internal Result (
		bool isSuccess,
		int errorCode,
		string? errorMessage = null,
		string? detailedErrorMessage = null
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
		DetailedErrorMessage = detailedErrorMessage;
	}

	protected internal Result (
		T value,
		bool isSuccess,
		int errorCode,
		string? errorMessage = null,
		string? detailedErrorMessage = null
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
		DetailedErrorMessage = detailedErrorMessage;
	}

	public static Result<TValue> Success<TValue> (
		TValue value
	) {
		return new Result<TValue> (value, true, (int)Error.None);
	}

	public static Result<TValue> Failure<TValue> (
		int error,
		string? errorMessage = null,
		string? detailedErrorMessage = null
	) {
		return new Result<TValue> (false, error, errorMessage, detailedErrorMessage);
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
