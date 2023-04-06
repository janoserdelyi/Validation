namespace com.janoserdelyi.Validation;

public enum Error
{
	Default = 0, // just a uninitialized default error value, basically
	None = 1,
	Empty = 2,
	InvalidFormat = 4,
	TooShort = 8,
	TooLong = 16,
	NotAllowed = 32
}