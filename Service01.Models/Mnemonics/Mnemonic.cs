namespace Service01.Models.Mnemonics
{
	public class Mnemonic
	{
		public const string UNKNOWN_ERROR = "Unknown error";
		public const string TIMEOUT_BROKER = "Timeout from broker";
		public const string ANSWER_EMPTY = "Answer is empty";
		public const string INVALID_FORMAT = "Invalid format";

		public const int UNKNOWN_CODE = 0;

		public const string HEALTH_CHECK_RESULT_HEALTHY = "Все системы в норме";
		public const string HEALTH_CHECK_RESULT_BROKER_PATH_EMPTY = "Не задан путь директории (BrokerPath) для брокера";
		public const string HEALTH_CHECK_RESULT_BROKER_PATH_INVALID = "Не корректный путь директории (BrokerPath) для брокера";
	}
}
