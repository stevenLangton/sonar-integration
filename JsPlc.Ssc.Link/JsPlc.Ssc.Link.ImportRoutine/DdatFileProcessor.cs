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
		
		const string _separator = "\x01";
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

		public void ProcessAbInitioFile()
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
							var lineTokens = line.Split(Convert.ToChar(_separator));

							var newColleague = new ColleagueModel
							{
								ColleagueId = lineTokens[1],
								EmailAddress = lineTokens[0]
							};

							if (_fileData.ContainsKey(newColleague.ColleagueId))
							{
								var existingColleague = _fileData[newColleague.ColleagueId];
								//update data
								existingColleague.EmailAddress = newColleague.EmailAddress;
							}
							else
							{
								//insert data
								_fileData.Add(newColleague.ColleagueId, newColleague);
							}
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

		public void ProcessFIMFile()
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

							var lineTokens = line.Split(Convert.ToChar(_separator));

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

							if (_fileData.ContainsKey(newColleague.ColleagueId))
							{
								var existingColleague = _fileData[newColleague.ColleagueId];
								//update data
								existingColleague.FirstName = newColleague.FirstName;
								existingColleague.LastName = newColleague.LastName;
								existingColleague.KnownAsName = newColleague.KnownAsName;
								existingColleague.Grade = newColleague.Grade;
								existingColleague.ManagerId = newColleague.ManagerId;
								existingColleague.Division = newColleague.Division;
								existingColleague.Department = newColleague.Department;

							}
							else
							{
								//insert data
								_fileData.Add(newColleague.ColleagueId, newColleague);
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

		public void MoveFilesToProcessedFolder()
		{
			_fileData.Clear();

			var processedFilesToMove = _importFilesLocation.GetFiles("*.ddat");
			
			foreach(var processedFileToMove in processedFilesToMove)
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

			if(processedFilesToMove.Any() == false)
			{
				_logger.Info("Files to move not found");
			}
		}
	}
}
