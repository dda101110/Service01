using Microsoft.Extensions.Logging;
using Microsoft.Extensions.Options;
using Newtonsoft.Json;
using Service01.Models.Mnemonics;
using Service01.Models.Models;
using System.Net;
using System.Security.Cryptography;
using System.Text;

namespace Service01.Services.BrokerService
{
	public class FSBrokerService : IBrokerService
	{
		private static HashSet<string> _files = new HashSet<string>();
		private SemaphoreSlim _semaphore { get; set; } = new SemaphoreSlim(1);
		private BufferOptionModel _bufferOption { get; set; }
		private IValidateService _validateService { get; set; }
		private FileSystemWatcher _watcher { get; set; }
		private string? _filePath { get; set; }
		private string _key { get; set; }
		private string _filenameRequest { get=> $"{_filePath}.req"; }
		private string _filenameResponse { get=> $"{_filePath}.resp"; }
		private ILogger<FSBrokerService> _logger { get; set; }
		private object _locker = new object();

		public FSBrokerService(IOptions<BufferOptionModel> bufferOption, IValidateService validateService, ILogger<FSBrokerService> logger)
		{
			_logger = logger;
			_validateService = validateService;
			_bufferOption = bufferOption.Value;
			_semaphore.WaitAsync();

			_watcher = new FileSystemWatcher(_bufferOption.BrokerPath);
			_watcher.NotifyFilter = NotifyFilters.FileName;
			_watcher.Created += async (sender, e) => await OnFileCreatedAsync(e);
		}
		private async Task OnFileCreatedAsync(FileSystemEventArgs e)
		{
			_logger.LogInformation($"OnFileCreated Name: {e.Name}");
			_semaphore?.Release();
		}
		public async Task<RateResponseModel> GetRateAsync(RateRequestModel request)
		{
			var result = new RateResponseModel()
			{
				Item = null,
				Success = false,
				ErrorText = Mnemonic.UNKNOWN_ERROR,
			};

			_filePath = GetFilename(request);
			_key = Path.GetFileName(_filePath);

			_watcher.Filter = Path.GetFileName(_filenameResponse);
			_watcher.EnableRaisingEvents = true;

			await SaveRequestAsync(request);

			var waitTask = _semaphore.WaitAsync();
			var timeoutTask = Task.Delay(TimeSpan.FromSeconds(_bufferOption.Timeout));

			var completedTask = await Task.WhenAny(waitTask, timeoutTask);

			if (completedTask == waitTask)
			{
				result.Item = await ReadResponseAsync();
			}
			else
			{
				result.ErrorText = Mnemonic.TIMEOUT_BROKER;
				result.StatusCode = (int)HttpStatusCode.RequestTimeout;
			}

			EnsureClean();

			_validateService.Validate(result);

			_logger.LogInformation($"Data from FileBroker: {JsonConvert.SerializeObject(result)}");

			return result;
		}
		public void EnsureClean()
		{
			_semaphore?.Release();
			
			bool fileExists = false;
			lock (_files)
			{
				fileExists = _files.TryGetValue(_key, out var val);

				if (fileExists)
				{
					_files.Remove(_key);
				}
			}

			if (File.Exists(_filenameRequest) && fileExists)
			{
				File.Delete(_filenameRequest);

				_logger.LogInformation($"File {_filenameRequest} deleted");
			} else
			{
				_logger.LogWarning($"File {_filenameRequest} absent");
			}

			if (File.Exists(_filenameResponse) && fileExists)
			{
				File.Delete(_filenameResponse);

				_logger.LogInformation($"File {_filenameResponse} deleted");
			} else
			{
				_logger.LogWarning($"File {_filenameResponse} absent");
			}
		}

		private async Task SaveRequestAsync(RateRequestModel request)
		{
			string content = $"{request.HttpMethod} {request.HttpPath}";
			bool fileExists = false;

			lock (_files)
			{
				fileExists = _files.TryGetValue(_key, out var val);

				if (!fileExists)
				{
					_files.Add(_key);
				}
			}

			if (!File.Exists(_filenameRequest) && !fileExists)
			{
				await File.WriteAllTextAsync(_filenameRequest, content);

				_logger.LogInformation($"FileRequest {_filenameRequest} created");
			} else
			{
				_logger.LogWarning($"FileRequest exists {_filenameRequest}");
			}
		}
		private async Task<string> ReadResponseAsync()
		{
			string result = await File.ReadAllTextAsync(_filenameResponse);

			return result;
		}
		private string GetFilename(RateRequestModel request)
		{
			using var md5 = MD5.Create();
			byte[] data = Encoding.UTF8.GetBytes(request.HttpMethod + request.HttpPath);
			byte[] md5hash = md5.ComputeHash(data);
			string filename = BitConverter.ToString(md5hash).Replace("-", "");

			var result = Path.Combine(_bufferOption.BrokerPath, filename);

			return result;
		}
	}
}
