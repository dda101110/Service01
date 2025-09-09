using Service01.Models.Mnemonics;
using Service01.Models.Models;
using Service01.Services;
using System.Net;
using Xunit;

namespace Service01.Tests
{
	public class ValidateServiceTests
	{
		[Fact]
		public void InvalidDataInEmptyData()
		{
			ValidateService validateService = new ValidateService();

			ResponseModel model = new ResponseModel();
			validateService.Validate(model);

			Assert.False(model.Success, "model.Success should be False");
		}

		[Fact]
		public void ValidSuccessInValidResponseModel()
		{
			ValidateService validateService = new ValidateService();

			ResponseModel model = new ResponseModel()
			{
				Item = "200\r\nRESPONSE",
			};
			validateService.Validate(model);

			Assert.True(model.Success, "model.Success should be True");
		}

		[Fact]
		public void ValidStatusCodeInValidResponseModel()
		{
			ValidateService validateService = new ValidateService();

			ResponseModel model = new ResponseModel()
			{
				Item = "200\r\nRESPONSE",
			};
			validateService.Validate(model);

			Assert.Equal((int)HttpStatusCode.OK, model.StatusCode);
		}

		[Fact]
		public void HasErrorInInvalidResponseModel()
		{
			ValidateService validateService = new ValidateService();

			ResponseModel model = new ResponseModel()
			{
				Item = "200x\r\nRESPONSE",
			};
			validateService.Validate(model);

			Assert.Equal(Mnemonic.INVALID_FORMAT, model.ErrorText);
		}
	}
}
