namespace com.janoserdelyi.Validation;

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