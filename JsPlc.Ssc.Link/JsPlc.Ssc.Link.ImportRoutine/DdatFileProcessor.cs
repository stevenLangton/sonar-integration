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
		DirectoryInfo _unexpectedFilesLocation = new DirectoryInfo(Path.Combine(AppDomain.CurrentDomain.BaseDirectory, ConfigurationManager.AppSettings["UnexpectedFilesLocation"]));

		const string _ddatSeparator = "\x01";
		const char _datSeparator = ',';
		const char _packCharacter = '"';
		const string _abInitioFileSearchPattern = "Link_AbInitio_{0}_*.ddat";
		const string _fimFileSearchPattern = "Link_FIM_{0}_*.ddat";
		const string _fileNameDateFormat = "ddMMyyyy";

		private ILogger _logger;

		private bool _filesProcessed = false;

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
			if (_filesProcessed)
			{
				_fileData.Clear();

				var foundAbInitioFile = findAbInitioFile();
				var foundFimFile = findFimFile();

				var processedFilesToMove = new FileInfo[] { foundAbInitioFile, foundFimFile };

				foreach (var processedFileToMove in processedFilesToMove)
				{
					try
					{
						var originalFileName = processedFileToMove.Name;
						var newFileName = Path.Combine(_processedFilesLocation.FullName, string.Format("{0}_{1}.processed", processedFileToMove.Name, DateTime.Now.Ticks));
						processedFileToMove.MoveTo(newFileName);
						_logger.InfoFormat("Processed file {0} moved to Processed Files folder", originalFileName);
					}
					catch (Exception ex)
					{
						_logger.Error(ex);
					}
				}

				if (processedFilesToMove.Any() == false)
				{
					_logger.Info("Processed files to move not found");
				}
			}
		}

		public void MoveUnexpectedFilesToUnexpectedFolder()
		{
			if (_filesProcessed)
			{
				var processedFilesToMove = _importFilesLocation.GetFiles("*.ddat");

				foreach (var processedFileToMove in processedFilesToMove)
				{
					try
					{
						var originalFileName = processedFileToMove.Name;
						var newFileName = Path.Combine(_unexpectedFilesLocation.FullName, string.Format("{0}_{1}.unexpected", processedFileToMove.Name, DateTime.Now.Ticks));
						processedFileToMove.MoveTo(newFileName);
						_logger.InfoFormat("Unexpected file {0} moved to Unexpected Files folder", originalFileName);
					}
					catch (Exception ex)
					{
						_logger.Error(ex);
					}
				}

				if (processedFilesToMove.Any() == false)
				{
					_logger.Info("Unexpected files to move not found");
				}
			}
		}

		public void ProcessFiles()
		{
			_logger.Info("START process files");
			_filesProcessed = false;
			if (bothFilesPresent())
			{
				processStep1AbInitio();
				processStep2FIM();
				_filesProcessed = true;
			}
			else
			{
				_logger.Info("One of the files is missing, waiting for 2 files to be present.");
			}
			_logger.Info("END processed files");
		}

		#region Private Methods

		private bool bothFilesPresent()
		{
			bool result = false;

			if (findAbInitioFile() != null && findFimFile() != null)
			{
				result = true;
				_logger.Info("Both files are present, ready to process.");
			}

			return result;
		}

		private void processStep1AbInitio()
		{
			var foundAbInitioFile = findAbInitioFile();

			if (foundAbInitioFile != null)
			{
				_logger.InfoFormat("Processing new AbInitio file: {0}", foundAbInitioFile.Name);
				using (StreamReader sr = new StreamReader(foundAbInitioFile.FullName))
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
							string errorMessage = string.Format("Can't parse AbInitio file {1} line {0}", lineNumber, foundAbInitioFile.FullName);
							_logger.Error(errorMessage, ex);
						}

						lineNumber++;
					}
				}
				_logger.InfoFormat("Processed new AbInitio file: {0}", foundAbInitioFile.Name);
			}
			else
			{
				_logger.Info("AbInitio file not found");
			}
		}

		private void processStep2FIM()
		{
			var foundFIMFile = findFimFile();

			if (foundFIMFile != null)
			{
				_logger.InfoFormat("Processing new FIM file: {0}", foundFIMFile.Name);
				using (StreamReader sr = new StreamReader(foundFIMFile.FullName))
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
							string errorMessage = string.Format("Can't parse FIM file {1} line {0}", lineNumber, foundFIMFile.FullName);
							_logger.Error(errorMessage, ex);
						}

						lineNumber++;
					}
				}
				_logger.InfoFormat("Processed new FIM file {0}", foundFIMFile.Name);
			}
			else
			{
				_logger.Info("FIM file not found");
			}
		}

		private FileInfo findFimFile()
		{
			string formattedFileName = string.Format(_fimFileSearchPattern, DateTime.Today.ToString(_fileNameDateFormat));

			var result = _importFilesLocation.GetFiles(formattedFileName, SearchOption.TopDirectoryOnly).FirstOrDefault();

			if(result == null)
			{
				_logger.InfoFormat("FIM file not found. Search pattern: {0}", formattedFileName);
			}

			return result;
		}

		private FileInfo findAbInitioFile()
		{
			string formattedFileName = string.Format(_abInitioFileSearchPattern, DateTime.Today.ToString(_fileNameDateFormat));
			
			var result = _importFilesLocation.GetFiles(formattedFileName, SearchOption.TopDirectoryOnly).FirstOrDefault();

			if(result == null)
			{
				_logger.InfoFormat("AbInitio file not found. Search pattern: {0}", formattedFileName);
			}

			return result;
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

		#endregion
	}
}
