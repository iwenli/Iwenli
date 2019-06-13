using FSLib.App.SimpleUpdater;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Iwenli.CodeGenerate
{
	class Update
	{
		/// <summary>
		/// 是否启用更新
		/// </summary>
		public static bool Enable => true;

		/// <summary>
		/// 检测并更新
		/// </summary>
		public static async Task<Version> CheckUpdateTask()
		{
			Version newVersion = null;
			if (Enable)
			{
				//任务模式检测更新
				var updater = Updater.CreateUpdaterInstance(@"http://oa.txooo.com/soft/code_generate/{0}", "update_c.xml");
				try
				{
					newVersion = await updater.CheckUpdateTask();
				}
				catch (Exception ex)
				{
					Iwenli.LogHelper.GetLogger("app_update").Error("升级程序异常", ex);
				}
			}
			return newVersion;
		}
	}
}
