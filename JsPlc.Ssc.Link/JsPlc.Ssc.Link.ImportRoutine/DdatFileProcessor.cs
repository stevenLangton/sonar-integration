using JsPlc.Ssc.Link.Core.Interfaces;
using JsPlc.Ssc.Link.ImportRoutine.Interfaces;
using System;
using System.Collections.Generic;
using System.Configuration;
using System.Data;
using System.IO;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace JsPlc.Ssc.Link.ImportRoutine
{
	public class DdatFileProcessor : IFileProcessor
	{
		Dictionary<string, ColleagueModel> _fileData = new Dictionary<string, ColleagueModel>();

		DirectoryInfo _importFilesLocation = new DirectoryInfo(Path.Combine(AppDomain.CurrentDomain.BaseDirectory, ConfigurationManager.AppSettings["ImportFilesLocation"]));
		DirectoryInfo _processedFilesLocation = new DirectoryInfo(Path.Combine(AppDomain.CurrentDomain.BaseDirectory, ConfigurationManager.AppSettings["ProcessedFilesLocation"]));

		const string _ddatSeparator = "\x01";
		const char _datSeparator = ',';
		const char _packCharacter = '"';

		private ILogger _logger;

		public DdatFileProcessor(ILogger logger)
		{
			this._logger = logger;
		}

		public IReadOnlyCollection<ColleagueModel> ProcessedData
		{
			get
			{
				return _fileData.Values.ToList().AsReadOnly();
			}
		}

		public void MoveFilesToProcessedFolder()
		{
			_fileData.Clear();

			var processedFilesToMove = _importFilesLocation.GetFiles("*.ddat");

			foreach (var processedFileToMove in processedFilesToMove)
			{
				try
				{
					var originalFileName = processedFileToMove.Name;
					var newFileName = Path.Combine(_processedFilesLocation.FullName, string.Format("{0}_{1}.processed", processedFileToMove.Name, DateTime.Now.Ticks));
					processedFileToMove.MoveTo(newFileName);
					_logger.InfoFormat("File {0} moved", originalFileName);
				}
				catch (Exception ex)
				{
					_logger.Error(ex);
				}
			}

			if (processedFilesToMove.Any() == false)
			{
				_logger.Info("Files to move not found");
			}
		}

		public void ProcessFiles()
		{
			ProcessStep1();
			ProcessStep2();
		}

		private void ProcessStep1()
		{
			var foundAbInitioFiles = _importFilesLocation.GetFiles("Link_AbInitio_*.ddat", SearchOption.TopDirectoryOnly).FirstOrDefault();

			if (foundAbInitioFiles != null)
			{
				_logger.InfoFormat("Loading new {0} file", foundAbInitioFiles.Name);
				using (StreamReader sr = new StreamReader(foundAbInitioFiles.FullName))
				{
					int lineNumber = 0;
					while (sr.Peek() >= 0)
					{
						var line = sr.ReadLine();

						try
						{
							var lineTokens = line.Split(Convert.ToChar(_ddatSeparator));

							var newColleague = new ColleagueModel
							{
								ColleagueId = lineTokens[0],
								FirstName = lineTokens[1],
								LastName = lineTokens[3],
								KnownAsName = lineTokens[2],
								Grade = lineTokens[9],
								ManagerId = lineTokens[10],
								Division = lineTokens[23],
								Department = lineTokens[18]
							};

							//insert data
							_fileData.Add(newColleague.ColleagueId, newColleague);
						}
						catch (Exception ex)
						{
							string errorMessage = string.Format("Can't parse line {0} in the file {1}", lineNumber, foundAbInitioFiles.FullName);
							_logger.Error(errorMessage, ex);
						}

						lineNumber++;
					}
				}
				_logger.InfoFormat("Loaded new {0} file", foundAbInitioFiles.Name);
			}
			else
			{
				_logger.Info("Nothing to load - AbInitio");
			}
		}

		private void ProcessStep2()
		{
			var foundFIMFiles = _importFilesLocation.GetFiles("Link_FIM_*.ddat", SearchOption.TopDirectoryOnly).FirstOrDefault();

			if (foundFIMFiles != null)
			{
				_logger.InfoFormat("Loading new {0} file", foundFIMFiles.Name);
				using (StreamReader sr = new StreamReader(foundFIMFiles.FullName))
				{
					int lineNumber = 0;
					while (sr.Peek() >= 0)
					{
						var line = sr.ReadLine();

						try
						{
							var lineTokens = line.Split(_datSeparator);

							var newColleague = new ColleagueModel
							{
								ColleagueId = unpackString(lineTokens[1], _packCharacter),
								EmailAddress = unpackString(lineTokens[0], _packCharacter)
							};

							if (_fileData.ContainsKey(newColleague.ColleagueId))
							{
								var existingColleague = _fileData[newColleague.ColleagueId];
								//update data
								existingColleague.EmailAddress = newColleague.EmailAddress;
							}

						}
						catch (Exception ex)
						{
							string errorMessage = string.Format("Can't parse line {0} in the file {1}", lineNumber, foundFIMFiles.FullName);
							_logger.Error(errorMessage, ex);
						}

						lineNumber++;
					}
				}
				_logger.InfoFormat("Loaded new {0} file", foundFIMFiles.Name);
			}
			else
			{
				_logger.Info("Nothing to load - FIM");
			}
		}

		private static unsafe string unpackString(string inputString, char packChar)
		{
			int len = inputString.Length;
			char* newChars = stackalloc char[len];
			char* currentChar = newChars;

			if (inputString[0] == packChar && inputString[len - 1] == packChar)
			{
				//from second to pre-last character
				for (int i = 1; i < len - 1; ++i)
				{
					char c = inputString[i];
					if(c == packChar)
					{
						string formattedError = string.Format("String contains Pack Character; String: {0}; Pack Character: {1}; Position: {2}"
							, inputString
							, packChar
							, i);
						throw new ArgumentOutOfRangeException(formattedError);
					}

					*currentChar++ = c;
				}
			}			
			
			return new string(newChars, 0, (int)(currentChar - newChars));
		}

	}
}
