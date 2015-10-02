using System;
using System.ComponentModel;
using System.IO;
using System.Text;
using WpfTestTask.Enums;
using WpfTestTask.Models;

namespace WpfTestTask.Services
{
	public class FileService
	{
		private const string FileName = "list.csv";

		public BindingList<RowModel> ReadData()
		{
			var list = new BindingList<RowModel>();

			string text = File.ReadAllText(FileName);

			string[] lines = text.Split('\n');

			foreach (var line in lines)
			{
				if (string.IsNullOrEmpty(line.Trim()))
				{
					continue;
				}

				var values = line.Split(',');

				var id = int.Parse(values[0].Trim().Trim('\r'));
				var description = values[1].Trim().Trim('\r');
				var state = (State)Enum.Parse(typeof(State), values[2].Trim().Trim('\r'));
				var isCompleted = bool.Parse(values[3].Trim().Trim('\r'));

				var model = new RowModel
				{
					Id = id,
					Description = description,
					State = state,
					IsCompleted = isCompleted,
				};

				list.Add(model);
			}

			return list;
		}

		public void WriteData(BindingList<RowModel> list)
		{
			var sb = new StringBuilder();

			foreach (var model in list)
			{
				var str = string.Format("{0},{1},{2},{3}", model.Id, model.Description, model.State, model.IsCompleted);

				sb.AppendLine(str);
			}

			File.WriteAllText(FileName, sb.ToString());
		}
	}
}