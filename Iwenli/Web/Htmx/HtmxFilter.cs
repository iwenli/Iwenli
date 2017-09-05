using System;
using System.IO;
using System.Web;

namespace Iwenli.Web.Htmx
{
    public class HtmxFilter : Stream
    {
        private Stream m_sink;
        private string m_filePath = string.Empty;
        FileStream m_fs;

        /// <summary>
        /// 构造函数
        /// </summary>
        /// <param name="sink"></param>
        /// <param name="url"></param>
        /// <param name="page"></param>
        public HtmxFilter(Stream sink, Url url, Page page)
        {
            m_sink = sink;

            //非编辑模式
            switch (url.RequestViewType)
            {
                case RequestViewType.Put:
                    {
                        switch (page.SaveWay)
                        {
                            case PageSaveType.Null:
                                break;
                            case PageSaveType.Save:
                                {
                                    //保存静态文件
                                    if (!(new FileInfo(url.SavePath)).Directory.Exists)
                                    {
                                        (new FileInfo(url.SavePath)).Directory.Create();
                                    }
                                    m_fs = new FileStream(url.SavePath, FileMode.OpenOrCreate, FileAccess.Write);
                                }
                                break;
                            case PageSaveType.Cache:
                                {

                                    #region 默认缓存模式

                                    //设置页的可缓存性
                                    HttpCacheability _cacheability = HttpCacheability.Server;
                                    if (!System.Enum.TryParse<HttpCacheability>(page.Location, out _cacheability))
                                    {
                                         _cacheability = HttpCacheability.Server;
                                    }
                                    HttpContext.Current.Response.Cache.SetCacheability(HttpCacheability.Server);
                                    //是否应忽略由使缓存无效的客户端发送的 HTTP Cache-Control 标头
                                    HttpContext.Current.Response.Cache.SetValidUntilExpires(true);

                                    //设置页缓存的到期时间
                                    HttpContext.Current.Response.Cache.SetExpires(DateTime.Now.AddSeconds(page.UpdateTime));
                                    HttpContext.Current.Response.Cache.SetMaxAge(new TimeSpan((long)((long)page.UpdateTime * 10000000)));

                                    //根据 HTTP 标头值进行缓存
                                    if (!string.IsNullOrEmpty(page.Header))
                                    {
                                        HttpContext.Current.Response.Cache.VaryByHeaders[page.Header] = true;
                                    }
                                    //使用参数对页的各个版本进行缓存
                                    if (!string.IsNullOrEmpty(page.Param))
                                    {
                                        HttpContext.Current.Response.Cache.VaryByParams[page.Param] = true;
                                    }
                                    //使用自定义字符串对页的各个版本进行缓存
                                    if (!string.IsNullOrEmpty(page.Custom))
                                    {
                                        HttpContext.Current.Response.Cache.SetVaryByCustom(page.Custom);
                                    }

                                    #endregion

                                }
                                break;
                            case PageSaveType.None:
                                break;
                            default:
                                break;
                        }
                    }
                    break;
                case RequestViewType.Edit:
                    break;
                case RequestViewType.Update:
                    {
                        //模块中处理
                    }
                    break;
                case RequestViewType.Delete:
                    {
                        //模块中处理
                    }
                    break;
                case RequestViewType.Save:
                    {
                        //保存静态文件
                        if (!(new FileInfo(url.SavePath)).Directory.Exists)
                        {
                            (new FileInfo(url.SavePath)).Directory.Create();
                        } 
                        m_fs = new FileStream(url.SavePath, FileMode.OpenOrCreate, FileAccess.Write);
                    }
                    break;
                case RequestViewType.Null:
                    {
                        //模块中处理
                    }
                    break;
                case RequestViewType.Cache:
                    {
                        //模块中处理
                    }
                    break;
                default:
                    break;
            }   
        }

        protected override void Dispose(bool disposing)
        {
            if (m_fs != null)
            {
                m_fs.Dispose();
            }
            base.Dispose(disposing);
        }


        #region MyRegion

        /// <summary>
        /// 当前流是否支持读取
        /// </summary>
        public override bool CanRead
        {
            get { return true; }
        }
        /// <summary>
        /// 当前流是否支持查找功能
        /// </summary>
        public override bool CanSeek
        {
            get { return true; }
        }
        /// <summary>
        /// 当前流是否支持写入功能
        /// </summary>
        public override bool CanWrite
        {
            get { return true; }
        }
        /// <summary>
        /// 当在派生类中重写时，将清除该流的所有缓冲区，并使得所有缓冲数据被写入到基础设备。
        /// </summary>
        public override void Flush()
        {
            this.m_sink.Flush();
        }

        /// <summary>
        /// 用字节表示的流长度
        /// </summary>
        public override long Length
        {
            get { return 0; }
        }

        /// <summary>
        /// 获取或设置当前流中的位置
        /// </summary>
        public override long Position
        {
            get
            {
                try
                {
                    return m_sink.Position;
                }
                catch (Exception ex)
                {
                    return 0;
                }
            }

            set { m_sink.Position = value; }
        }

        /// <summary>
        /// 当在派生类中重写时，从当前流读取字节序列，并将此流中的位置提升读取的字节数。
        /// </summary>
        /// <param name="buffer"></param>
        /// <param name="offset"></param>
        /// <param name="count"></param>
        /// <returns></returns>
        public override int Read(byte[] buffer, int offset, int count)
        {
            return m_sink.Read(buffer, offset, count);
        }

        /// <summary>
        ///  当在派生类中重写时，设置当前流中的位置。
        /// </summary>
        /// <param name="offset"></param>
        /// <param name="origin"></param>
        /// <returns></returns>
        public override long Seek(long offset, SeekOrigin origin)
        {
            return this.m_sink.Seek(offset, origin);
        }

        /// <summary>
        /// 当在派生类中重写时，设置当前流的长度。
        /// </summary>
        /// <param name="value"></param>
        public override void SetLength(long value)
        {
            this.m_sink.SetLength(value);
        }

        #endregion


        /// <summary>
        /// 当在派生类中重写时，向当前流中写入字节序列，并将此流中的当前位置提升写入的字节数。
        /// </summary>
        /// <param name="buffer"></param>
        /// <param name="offset"></param>
        /// <param name="count"></param>
        public override void Write(byte[] buffer, int offset, int count)
        {
            if (m_fs != null && HttpContext.Current.Response.StatusCode == 200)
            {
                try
                {
                    //将数据写入静态文件.
                    m_fs.Write(buffer, 0, count);
                    //System.Threading.Thread.Sleep(100000);
                }
                catch (Exception ex)
                {
                    this.LogError("静态化文件失败：[" + m_filePath + "]" + ex.Message, ex);
                }
            }
            this.m_sink.Write(buffer, 0, count);
        }
    }
}
