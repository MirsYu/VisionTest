using System;
using System.Collections.Generic;
using System.Drawing;
using System.Web;
using System.Windows;
using System.Windows.Controls;
using VisionDesigner;
using VisionDesigner.BlobFind;
using WpfVisionTest.Global;
using WpfVisionTest.Log4Net;
using WpfVisionTest.MvdXmlParse;

namespace WpfVisionTest.BlobFindTool
{
    /// <summary>
    /// _BlobFindTool.xaml 的交互逻辑
    /// </summary>
    public partial class _BlobFindTool : UserControl
    {
        /// <summary>
        /// Blob查找工具
        /// </summary>
        private static CBlobFindTool m_stBlobFindToolObj = null;
        /// <summary>
        /// xml转换部分
        /// </summary>
        private CMvdXmlParseTool m_stXmlParseToolObj = null;
        /// <summary>
        /// 参数字典
        /// </summary>
        private Dictionary<string, object> m_paramDictionary;
        /// <summary>
        /// 工具输入图像
        /// </summary>
        private CMvdImage m_InputImage = null;

        private MVDRenderActivex m_RenderActivex = null;

        private UserControlBlobFindToolViewModel _viewModel;

        public _BlobFindTool()
        {
            InitializeComponent();
            m_RenderActivex = _RenderControl.GetSubject();
            m_InputImage = new CMvdImage();
            _viewModel = new UserControlBlobFindToolViewModel();
            DataContext = _viewModel;
            InitTool();
            AddParam();

            using (Bitmap bitmap = new Bitmap(AppDomain.CurrentDomain.BaseDirectory + "BlobFindTool\\Image.bmp"))
            {
                ImageConverterTool.ConvertBitmap2MVDImage(bitmap, m_InputImage);
            }
            m_RenderActivex.LoadImageFromObject(m_InputImage);
            m_RenderActivex.Display(MVDRenderActivex.MVD_REFRESH_MODE.Sync);
 
        }

        public void InitTool()
        {
            try
            {
                m_stBlobFindToolObj = new CBlobFindTool();
                byte[] fileBytes = new byte[256];
                uint nConfigDataSize = 256;
                uint nConfigDataLen = 0;
                try
                {
                    m_stBlobFindToolObj.SaveConfiguration(fileBytes, nConfigDataSize, ref nConfigDataLen);
                }
                catch (MvdException ex)
                {
                    if (MVD_ERROR_CODE.MVD_E_NOENOUGH_BUF == ex.ErrorCode)
                    {
                        fileBytes = new byte[nConfigDataLen];
                        nConfigDataSize = nConfigDataLen;
                        m_stBlobFindToolObj.SaveConfiguration(fileBytes, nConfigDataSize, ref nConfigDataLen);
                    }
                    else
                    {
                        throw ex;
                    }
                }
                UpdateParamList(fileBytes, nConfigDataLen);

                LogManager.WriteDebug("Init finish.");
            }
            catch (MvdException ex)
            {
                LogManager.WriteError("Fail to initialize the tool. ErrorCode: 0x" + ex.ErrorCode.ToString("X"));
            }
            catch (System.Exception ex)
            {
                LogManager.WriteError("Fail with error " + ex.Message);
            }
        }

        public void BlobFind()
        {
            if (m_stBlobFindToolObj == null)
            {
                m_stBlobFindToolObj = new CBlobFindTool();
            }
            if (m_stBlobFindToolObj.InputImage == null)
            {
                m_stBlobFindToolObj.InputImage = m_InputImage;
            }
            m_stBlobFindToolObj.Run();

            CBlobFindResult blobResult = m_stBlobFindToolObj.Result;
            if (blobResult.BlobInfo.Count > 0)
            {

            }
            else
            {

            }
        }

        public void AddParam()
        {
            foreach (var kvp in m_paramDictionary)
            {
                _viewModel.AddToolRunValue(kvp.Key, kvp.Value);
            }
        }

        public void UpdateParmTool()
        {
            if (m_stBlobFindToolObj == null)
            {
                m_stBlobFindToolObj = new CBlobFindTool();
            }
            foreach (var item in _viewModel.BeginnerToolRunValues)
            {
                m_stBlobFindToolObj.SetRunParam(item._Name, item._Value);
            }
            foreach (var item in _viewModel.ExpertToolRunValues)
            {
                m_stBlobFindToolObj.SetRunParam(item._Name, item._Value);
            }
        }

        private void Button_Click(object sender, System.Windows.RoutedEventArgs e)
        {
            UpdateParmTool();
            string str = "";
            m_stBlobFindToolObj.GetRunParam("LowThreshold", ref str);
            BlobFind();
        }

        private void UpdateParamList(Byte[] bufXml, uint nXmlLen)
        {
            if (null == m_stXmlParseToolObj)
            {
                m_stXmlParseToolObj = new CMvdXmlParseTool(bufXml, nXmlLen);
            }
            else
            {
                m_stXmlParseToolObj.UpdateXmlBuf(bufXml, nXmlLen);
            }

            m_paramDictionary = new Dictionary<string, object>();

            for (int i = 0; i < m_stXmlParseToolObj.IntValueList.Count; ++i)
            {
                m_paramDictionary[m_stXmlParseToolObj.IntValueList[i].Name] = m_stXmlParseToolObj.IntValueList[i];
            }

            for (int i = 0; i < m_stXmlParseToolObj.EnumValueList.Count; ++i)
            {
                m_paramDictionary[m_stXmlParseToolObj.EnumValueList[i].Name] = m_stXmlParseToolObj.EnumValueList[i];
            }

            for (int i = 0; i < m_stXmlParseToolObj.FloatValueList.Count; ++i)
            {
                m_paramDictionary[m_stXmlParseToolObj.FloatValueList[i].Name] = m_stXmlParseToolObj.FloatValueList[i];
            }

            for (int i = 0; i < m_stXmlParseToolObj.BooleanValueList.Count; ++i)
            {
                m_paramDictionary[m_stXmlParseToolObj.BooleanValueList[i].Name] = m_stXmlParseToolObj.BooleanValueList[i];
            }
        }

    }
}
