using Service01.Models.Mnemonics;
using Service01.Models.Models;
using System.Net;

namespace Service01.Services
{
	public class ValidateService: IValidateService
	{
		public void Validate(ResponseModel model)
		{
			if (model is null)
			{
				throw new ArgumentNullException(nameof(model));
			}
			else if (model.StatusCode == Mnemonic.UNKNOWN_CODE)
			{
				if (string.IsNullOrEmpty(model.Item))
				{
					model.Success = false;
					model.StatusCode = (int)HttpStatusCode.NoContent;
					model.ErrorText = Mnemonic.ANSWER_EMPTY;
				}
				else
				{
					var newLine = Environment.NewLine;
					int firstLineIndex = model.Item.IndexOf(newLine);
					string firstLine = string.Empty;

					if (firstLineIndex != -1)
					{
						firstLine = model.Item.Substring(0, firstLineIndex);
					}

					bool isStatusCodeInt = int.TryParse(firstLine, out int statusCode);
					model.Item = model.Item.Remove(0, firstLineIndex + newLine.Length);

					if (!string.IsNullOrEmpty(model.Item) && isStatusCodeInt)
					{
						model.Success = true;
						model.StatusCode = statusCode;
						model.ErrorText = string.Empty;
					}
					else
					{
						model.Success = false;
						model.StatusCode = (int)HttpStatusCode.NotImplemented;
						model.ErrorText = Mnemonic.INVALID_FORMAT;
					}
				}
			}
		}
	}
}
