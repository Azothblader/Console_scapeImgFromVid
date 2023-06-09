using MediaToolkit;
using MediaToolkit.Model;
using MediaToolkit.Options;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Console_scapeImgFromVid
{
	class Program
	{
		static void Main(string[] args)
		{
			int extract_sec = 2;

			var _inputFile = @"C:\Users\4042\Desktop\[Genshin Impact] 強風オールバック - きらら Kirara The Queen of Delivery.mp4";
		 
			var _outputDir = @"C:\Users\4042\Desktop\imgz";


			var inputFile = new MediaFile { Filename = _inputFile  };
			//var outputFile = new MediaFile { Filename = _outputFile };

			//get length
			var objInfo = GetVideoInfo(_inputFile); 
			TimeSpan duration = new TimeSpan(objInfo.Item3);
			double totalSec = duration.TotalSeconds; 


			using (var engine = new Engine())
			{
				engine.GetMetadata(inputFile);

				// Saves the frame located on the 15th second of the video.
				var options = new ConversionOptions { Seek = TimeSpan.FromSeconds(15) };
				//engine.GetThumbnail(inputFile, outputFile, options); 
				var i = 0;
				 
				while (totalSec> i )
				{
					var n_outputFile = string.Format("{0}\\image-{1:0000}.jpeg", _outputDir, i);
					/*var thumbTask = new FfTaskSaveThumbnail(_tempFile, outputFile, TimeSpan.FromSeconds(i));
					_ = await mediaToolkitService.ExecuteAsync(thumbTask);
					i++;*/
					var outputFile = new MediaFile { Filename = n_outputFile };
					options = new ConversionOptions { Seek = TimeSpan.FromSeconds(i )   };
					try
					{
						engine.GetThumbnail(inputFile, outputFile, options);
					}
					catch {
						break;
					}
					i += extract_sec;
				}
			}
		}

		public static Tuple<int, int, long> GetVideoInfo(string fileName)
		{
			var inputFile = new MediaToolkit.Model.MediaFile { Filename = fileName };
			using (var engine = new Engine())
			{
				engine.GetMetadata(inputFile);
			}

			// FrameSize is returned as '1280x768' string.
			var size = inputFile.Metadata.VideoData.FrameSize.Split(new[] { 'x' }).Select(o => int.Parse(o)).ToArray();

			return new Tuple<int, int, long>(size[0], size[1], inputFile.Metadata.Duration.Ticks);
		}
	}
}
