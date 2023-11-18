#define USE_MAT // Comment or remove this line if you don't want to use Mat
using System;
using System.Collections.Generic;
using System.Drawing.Imaging;
using System.Drawing;
using System.Linq;
using System.Runtime.CompilerServices;
using System.Runtime.InteropServices;
using System.Text;
using System.Threading.Tasks;
using System.Windows.Media.Media3D;
using System.Xml.Linq;
using VisionDesigner;

namespace WpfVisionTest.Global
{
    public static class ImageConverterTool
    {
        // Opencv Mat转算子CMvdImage
        /*public static void ConvertMat2MVDImage(Mat mat, CMvdImage cMvdImg)
        {
            // 参数合法性判断
            if (null == mat || null == cMvdImg)
            {
                throw new MvdException(MVD_MODULE_TYPE.MVD_MODUL_APP, MVD_ERROR_CODE.MVD_E_HANDLE);
            }
            // 像素格式判断
            if (MatType.CV_8UC1 != mat.Type() && MatType.CV_8UC3 != mat.Type())
            {
                throw new MvdException(MVD_MODULE_TYPE.MVD_MODUL_APP, MVD_ERROR_CODE.MVD_E_SUPPORT);
            }

            uint imgWidth = (uint)mat.Size().Width;// 图片的真实宽度
            uint imgHeight = (uint)mat.Size().Height;// 图片的真实高度
            byte[] bMvdImgData = null;

            // 根据传入的mat图像初始化MVDImage，并进行转换
            if (mat.Type() == MatType.CV_8UC1)
            {
                cMvdImg.InitImage(imgWidth, imgHeight, MVD_PIXEL_FORMAT.MVD_PIXEL_MONO_08);
                bMvdImgData = cMvdImg.GetImageData().stDataChannel[0].arrDataBytes;
                Marshal.Copy(mat.Ptr(0), bMvdImgData, 0, bMvdImgData.Length);
            }
            else if (mat.Type() == MatType.CV_8UC3)
            {
                cMvdImg.InitImage(imgWidth, imgHeight, MVD_PIXEL_FORMAT.MVD_PIXEL_RGB_RGB24_C3);
                bMvdImgData = cMvdImg.GetImageData().stDataChannel[0].arrDataBytes;
                Marshal.Copy(mat.Ptr(0), bMvdImgData, 0, bMvdImgData.Length);

                // Mat为BGRBGR...存储，MVDImage为RGBRGB...存储，需要调整
                byte bTemp;
                for (int i = 0; i < imgWidth * imgHeight; i++)
                {
                    bTemp = bMvdImgData[3 * i];
                    bMvdImgData[3 * i] = bMvdImgData[3 * i + 2];
                    bMvdImgData[3 * i + 2] = bTemp;
                }
            }
        }*/

        // 算子CMvdImage转Opencv Mat
        /*public static void ConvertMVDImage2Mat(CMvdImage mvdImage, Mat mat)
        {
            // 参数合法性判断
            if (null == mat || null == mvdImage)
            {
                throw new MvdException(MVD_MODULE_TYPE.MVD_MODUL_APP, MVD_ERROR_CODE.MVD_E_HANDLE);
            }

            // 像素格式判断
            if (mvdImage.PixelFormat != MVD_PIXEL_FORMAT.MVD_PIXEL_MONO_08 && mvdImage.PixelFormat != MVD_PIXEL_FORMAT.MVD_PIXEL_RGB_RGB24_C3)
            {
                throw new MvdException(MVD_MODULE_TYPE.MVD_MODUL_APP, MVD_ERROR_CODE.MVD_E_SUPPORT);
            }

            int imgWidth = (int)mvdImage.Width;
            int imgHeight = (int)mvdImage.Height;

            // 根据传入的MVDImage类型初始化Mat
            if (mvdImage.PixelFormat == MVD_PIXEL_FORMAT.MVD_PIXEL_MONO_08)
            {
                mat.Create(imgHeight, imgWidth, MatType.CV_8UC1);
                Marshal.Copy(mvdImage.GetImageData(0).arrDataBytes, 0, mat.Ptr(0), mvdImage.GetImageData(0).arrDataBytes.Length);
            }
            else if (mvdImage.PixelFormat == MVD_PIXEL_FORMAT.MVD_PIXEL_RGB_RGB24_C3)
            {
                mat.Create(imgHeight, imgWidth, MatType.CV_8UC3);
                // 先备份MVD图像数据，保证不改变源图像数据
                byte[] bMvdImgDataTemp = new byte[mvdImage.GetImageData(0).arrDataBytes.Length];
                Array.Copy(mvdImage.GetImageData(0).arrDataBytes, bMvdImgDataTemp, bMvdImgDataTemp.Length);

                // Mat为BGRBGR...存储，MVD为RGBRGB...存储，需要调整
                byte bTemp;
                for (int i = 0; i < imgWidth * imgHeight; i++)
                {
                    bTemp = bMvdImgDataTemp[3 * i];
                    bMvdImgDataTemp[3 * i] = bMvdImgDataTemp[3 * i + 2];
                    bMvdImgDataTemp[3 * i + 2] = bTemp;
                }
                // 将数据拷贝至Mat图像
                Marshal.Copy(bMvdImgDataTemp, 0, mat.Ptr(0), bMvdImgDataTemp.Length);
            }

        }*/

        // Halcon HObject转算子CMvdImage
        /*public static void ConvertHalcon2MVDImage(HObject cHalconImg, CMvdImage cMvdImg)
        {
            // 参数合法性判断
            if (null == cHalconImg || null == cMvdImg)
            {
                throw new MvdException(MVD_MODULE_TYPE.MVD_MODUL_APP, MVD_ERROR_CODE.MVD_E_PARAMETER_ILLEGAL);
            }

            // 获取通道数量
            HTuple ChannelNum;
            HOperatorSet.CountChannels(cHalconImg, out ChannelNum);
            if (1 != ChannelNum.I && 3 != ChannelNum.I)
            {
                throw new MvdException(MVD_MODULE_TYPE.MVD_MODUL_APP, MVD_ERROR_CODE.MVD_E_SUPPORT);
            }

            HTuple ImageType;
            HTuple ImageWidth;
            HTuple ImageHeight;
            // 获取图像信息
            if (1 == ChannelNum.I) // 灰度图
            {
                HTuple ImagePoint;
                HOperatorSet.GetImagePointer1(cHalconImg, out ImagePoint, out ImageType, out ImageWidth, out ImageHeight);
                cMvdImg.InitImage(Convert.ToUInt32(ImageWidth.I), Convert.ToUInt32(ImageHeight.I), MVD_PIXEL_FORMAT.MVD_PIXEL_MONO_08);
                Marshal.Copy(ImagePoint.IP, cMvdImg.GetImageData().stDataChannel[0].arrDataBytes, 0, ImageWidth.I * ImageHeight.I);
            }
            else if (3 == ChannelNum)
            {
                HTuple ImagePointR;
                HTuple ImagePointG;
                HTuple ImagePointB;
                HOperatorSet.GetImagePointer3(cHalconImg, out ImagePointR, out ImagePointG, out ImagePointB, out ImageType, out ImageWidth, out ImageHeight);
                cMvdImg.InitImage(Convert.ToUInt32(ImageWidth.I), Convert.ToUInt32(ImageHeight.I), MVD_PIXEL_FORMAT.MVD_PIXEL_RGB_RGB24_C3);

                // 将IntPtr转为byte数组
                int nImageWidth = ImageWidth.I;
                int nImageHeight = ImageHeight.I;
                int nChannelDataLen = nImageWidth * nImageHeight;
                byte[] bImageBufR = new byte[nChannelDataLen];
                byte[] bImageBufG = new byte[nChannelDataLen];
                byte[] bImageBufB = new byte[nChannelDataLen];
                Marshal.Copy(ImagePointR.IP, bImageBufR, 0, nChannelDataLen);
                Marshal.Copy(ImagePointG.IP, bImageBufG, 0, nChannelDataLen);
                Marshal.Copy(ImagePointB.IP, bImageBufB, 0, nChannelDataLen);

                byte[] bMvdImgData = cMvdImg.GetImageData().stDataChannel[0].arrDataBytes;
                // 将图像数据拷贝至算子图像组件
                for (int i = 0; i < nImageHeight; i++)
                {
                    for (int j = 0; j < nImageWidth; j++)
                    {
                        bMvdImgData[i * nImageWidth * 3 + j * 3 + 0] = bImageBufR[i * nImageWidth + j];
                        bMvdImgData[i * nImageWidth * 3 + j * 3 + 1] = bImageBufG[i * nImageWidth + j];
                        bMvdImgData[i * nImageWidth * 3 + j * 3 + 2] = bImageBufB[i * nImageWidth + j];
                    }
                }
            }
        }*/

        // 算子CMvdImage转Halcon HObject
        /*public static void ConvertMVDImage2Halcon(CMvdImage cMvdImg, HObject cHalconImg)
        {
            // 参数合法性判断
            if (null == cHalconImg || null == cMvdImg)
            {
                throw new MvdException(MVD_MODULE_TYPE.MVD_MODUL_APP, MVD_ERROR_CODE.MVD_E_PARAMETER_ILLEGAL);
            }

            // 像素格式判断
            MVD_PIXEL_FORMAT enMvdImagePixel = cMvdImg.PixelFormat;
            if (MVD_PIXEL_FORMAT.MVD_PIXEL_MONO_08 != enMvdImagePixel && MVD_PIXEL_FORMAT.MVD_PIXEL_RGB_RGB24_C3 != enMvdImagePixel)
            {
                throw new MvdException(MVD_MODULE_TYPE.MVD_MODUL_APP, MVD_ERROR_CODE.MVD_E_SUPPORT);
            }

            GCHandle hImageData = new GCHandle();
            GCHandle hImageDataR = new GCHandle();
            GCHandle hImageDataG = new GCHandle();
            GCHandle hImageDataB = new GCHandle();
            try
            {
                int nImageWidth = Convert.ToInt32(cMvdImg.Width);
                int nImageHeight = Convert.ToInt32(cMvdImg.Height);
                // 根据传入的图像初始化Halcon
                if (MVD_PIXEL_FORMAT.MVD_PIXEL_MONO_08 == enMvdImagePixel)
                {
                    // 引用MvdImg数据来创建halcon图像;halcon内部会深拷贝
                    hImageData = GCHandle.Alloc(cMvdImg.GetImageData().stDataChannel[0].arrDataBytes, GCHandleType.Pinned);
                    HOperatorSet.GenImage1(out cHalconImg, "byte", nImageWidth, nImageHeight, hImageData.AddrOfPinnedObject());
                }
                else if (MVD_PIXEL_FORMAT.MVD_PIXEL_RGB_RGB24_C3 == enMvdImagePixel)
                {
                    // MVDImage图像是一个通道RGBRGBRGB...存放；需要另外开辟块内存
                    int nChannelDataLen = nImageWidth * nImageHeight;
                    byte[] bImageBufR = new byte[nChannelDataLen];
                    byte[] bImageBufG = new byte[nChannelDataLen];
                    byte[] bImageBufB = new byte[nChannelDataLen];
                    byte[] bMvdImageData = cMvdImg.GetImageData().stDataChannel[0].arrDataBytes;
                    // 引用MVDImage数据进行填充
                    for (int i = 0; i < nImageHeight; i++)
                    {
                        for (int j = 0; j < nImageWidth; j++)
                        {
                            bImageBufR[i * nImageWidth + j] = bMvdImageData[i * nImageWidth * 3 + j * 3 + 0];
                            bImageBufG[i * nImageWidth + j] = bMvdImageData[i * nImageWidth * 3 + j * 3 + 1];
                            bImageBufB[i * nImageWidth + j] = bMvdImageData[i * nImageWidth * 3 + j * 3 + 2];
                        }
                    }
                    // 创建halcon图像
                    hImageDataR = GCHandle.Alloc(bImageBufR, GCHandleType.Pinned);
                    hImageDataG = GCHandle.Alloc(bImageBufG, GCHandleType.Pinned);
                    hImageDataB = GCHandle.Alloc(bImageBufB, GCHandleType.Pinned);
                    HOperatorSet.GenImage3(out cHalconImg, "byte", nImageWidth, nImageHeight, hImageDataR.AddrOfPinnedObject(), hImageDataG.AddrOfPinnedObject(), hImageDataB.AddrOfPinnedObject());
                }
            }
            finally
            {
                if (hImageData.IsAllocated)
                {
                    hImageData.Free();
                }
                if (hImageDataR.IsAllocated)
                {
                    hImageDataR.Free();
                }
                if (hImageDataG.IsAllocated)
                {
                    hImageDataG.Free();
                }
                if (hImageDataB.IsAllocated)
                {
                    hImageDataB.Free();
                }
            }
        }*/

        // 引用相机裸数据初始化算子CMvdImage
        /*public static void GetImvdImgFromCamerData(UInt32 nWidth, UInt32 nHeight, Byte[] byteGrapData, MyCamera.MvGvspPixelType enPixelType, CMvdImage cMvdImage)
        {
            if (0 == nWidth || 0 == nHeight || null == byteGrapData || null == cMvdImage)
            {
                throw new MvdException(MVD_MODULE_TYPE.MVD_MODUL_APP, MVD_ERROR_CODE.MVD_E_PARAMETER_ILLEGAL);
            }
            if ((MyCamera.MvGvspPixelType.PixelType_Gvsp_Mono8 != enPixelType) && (MyCamera.MvGvspPixelType.PixelType_Gvsp_RGB8_Packed != enPixelType))
            {
                throw new MvdException(MVD_MODULE_TYPE.MVD_MODUL_APP, MVD_ERROR_CODE.MVD_E_SUPPORT);
            }
            MVD_IMAGE_DATA_INFO imagedate = new MVD_IMAGE_DATA_INFO();

            // 根据出图宽高初始化图像
            if (MyCamera.MvGvspPixelType.PixelType_Gvsp_Mono8 == enPixelType)
            {
                imagedate.stDataChannel[0].arrDataBytes = byteGrapData;
                imagedate.stDataChannel[0].nLen = nWidth * nHeight;
                imagedate.stDataChannel[0].nSize = nWidth * nHeight;
                imagedate.stDataChannel[0].nRowStep = nWidth;
                cMvdImage.InitImage(nWidth, nHeight, MVD_PIXEL_FORMAT.MVD_PIXEL_MONO_08, imagedate);
            }
            else if (MyCamera.MvGvspPixelType.PixelType_Gvsp_RGB8_Packed == enPixelType)
            {
                imagedate.stDataChannel[0].arrDataBytes = byteGrapData;
                imagedate.stDataChannel[0].nLen = nWidth * nHeight * 3;
                imagedate.stDataChannel[0].nSize = nWidth * nHeight * 3;
                imagedate.stDataChannel[0].nRowStep = nWidth * 3;
                cMvdImage.InitImage(nWidth, nHeight, MVD_PIXEL_FORMAT.MVD_PIXEL_RGB_RGB24_C3, imagedate);
            }
        }*/

        // C# Bitmap转算子CMvdImage
        public static void ConvertBitmap2MVDImage(Bitmap cBitmapImg, CMvdImage cMvdImg)
        {
            // 参数合法性判断
            if (null == cBitmapImg || null == cMvdImg)
            {
                throw new MvdException(MVD_MODULE_TYPE.MVD_MODUL_APP, MVD_ERROR_CODE.MVD_E_PARAMETER_ILLEGAL);
            }

            // 判断像素格式
            if (PixelFormat.Format8bppIndexed != cBitmapImg.PixelFormat && PixelFormat.Format24bppRgb != cBitmapImg.PixelFormat)
            {
                throw new MvdException(MVD_MODULE_TYPE.MVD_MODUL_APP, MVD_ERROR_CODE.MVD_E_SUPPORT);
            }

            Int32 nImageWidth = cBitmapImg.Width;
            Int32 nImageHeight = cBitmapImg.Height;
            Int32 nChannelNum = 0;
            BitmapData bitmapData = null;

            try
            {
                // 获取图像信息
                if (PixelFormat.Format8bppIndexed == cBitmapImg.PixelFormat) // 灰度图
                {
                    bitmapData = cBitmapImg.LockBits(new Rectangle(0, 0, nImageWidth, nImageHeight)
                                                                    , ImageLockMode.ReadOnly
                                                                    , PixelFormat.Format8bppIndexed);
                    cMvdImg.InitImage(Convert.ToUInt32(nImageWidth), Convert.ToUInt32(nImageHeight), MVD_PIXEL_FORMAT.MVD_PIXEL_MONO_08);
                    nChannelNum = 1;
                }
                else if (PixelFormat.Format24bppRgb == cBitmapImg.PixelFormat) // 彩色图
                {
                    bitmapData = cBitmapImg.LockBits(new Rectangle(0, 0, nImageWidth, nImageHeight)
                                                                , ImageLockMode.ReadOnly
                                                                , PixelFormat.Format24bppRgb);
                    cMvdImg.InitImage(Convert.ToUInt32(nImageWidth), Convert.ToUInt32(nImageHeight), MVD_PIXEL_FORMAT.MVD_PIXEL_RGB_RGB24_C3);
                    nChannelNum = 3;
                }

                // 考虑图像是否4字节对齐，bitmap要求4字节对齐，而mvdimage不要求对齐
                if (0 == nImageWidth % 4) // 4字节对齐时，直接拷贝
                {
                    Marshal.Copy(bitmapData.Scan0, cMvdImg.GetImageData().stDataChannel[0].arrDataBytes, 0, nImageWidth * nImageHeight * nChannelNum);
                }
                else // 按步长逐行拷贝
                {
                    // 每行实际占用字节数
                    Int32 nRowPixelByteNum = nImageWidth * nChannelNum + 4 - (nImageWidth * nChannelNum % 4);
                    // 每行首字节首地址
                    IntPtr bitmapDataRowPos = IntPtr.Zero;
                    for (int i = 0; i < nImageHeight; i++)
                    {
                        // 获取每行第一个像素值的首地址
                        bitmapDataRowPos = new IntPtr(bitmapData.Scan0.ToInt64() + nRowPixelByteNum * i);
                        Marshal.Copy(bitmapDataRowPos, cMvdImg.GetImageData().stDataChannel[0].arrDataBytes, i * nImageWidth * nChannelNum, nImageWidth * nChannelNum);
                    }
                }

                // bitmap彩色图按BGR存储，而MVDimg按RGB存储，改变存储顺序
                // 交换R和B
                if (PixelFormat.Format24bppRgb == cBitmapImg.PixelFormat)
                {
                    byte bTemp;
                    byte[] bMvdImgData = cMvdImg.GetImageData().stDataChannel[0].arrDataBytes;
                    for (int i = 0; i < nImageWidth * nImageHeight; i++)
                    {
                        bTemp = bMvdImgData[3 * i];
                        bMvdImgData[3 * i] = bMvdImgData[3 * i + 2];
                        bMvdImgData[3 * i + 2] = bTemp;
                    }
                }
            }
            finally
            {
                cBitmapImg.UnlockBits(bitmapData);
            }
        }

        // 算子CMvdImage转C# Bitmap
        public static void ConvertMVDImage2Bitmap(CMvdImage cMvdImg, ref Bitmap cBitmapImg)
        {
            // 参数合法性判断
            if (null == cMvdImg)
            {
                throw new MvdException(MVD_MODULE_TYPE.MVD_MODUL_APP, MVD_ERROR_CODE.MVD_E_PARAMETER_ILLEGAL);
            }

            // 判断像素格式
            if (MVD_PIXEL_FORMAT.MVD_PIXEL_MONO_08 != cMvdImg.PixelFormat && MVD_PIXEL_FORMAT.MVD_PIXEL_RGB_RGB24_C3 != cMvdImg.PixelFormat)
            {
                throw new MvdException(MVD_MODULE_TYPE.MVD_MODUL_APP, MVD_ERROR_CODE.MVD_E_SUPPORT);
            }

            Int32 nImageWidth = Convert.ToInt32(cMvdImg.Width);
            Int32 nImageHeight = Convert.ToInt32(cMvdImg.Height);
            Int32 nChannelNum = 0;
            BitmapData bitmapData = null;
            byte[] bBitmapDataTemp = null;
            try
            {
                // 获取图像信息
                if (MVD_PIXEL_FORMAT.MVD_PIXEL_MONO_08 == cMvdImg.PixelFormat) // 灰度图
                {
                    cBitmapImg = new Bitmap(nImageWidth, nImageHeight, PixelFormat.Format8bppIndexed);

                    // 灰度图需指定调色板
                    ColorPalette colorPalette = cBitmapImg.Palette;
                    for (int j = 0; j < 256; j++)
                    {
                        colorPalette.Entries[j] = Color.FromArgb(j, j, j);
                    }
                    cBitmapImg.Palette = colorPalette;

                    bitmapData = cBitmapImg.LockBits(new Rectangle(0, 0, nImageWidth, nImageHeight)
                                                                    , ImageLockMode.WriteOnly
                                                                    , PixelFormat.Format8bppIndexed);

                    // 灰度图不做深拷贝
                    bBitmapDataTemp = cMvdImg.GetImageData().stDataChannel[0].arrDataBytes;
                    nChannelNum = 1;
                }
                else if (MVD_PIXEL_FORMAT.MVD_PIXEL_RGB_RGB24_C3 == cMvdImg.PixelFormat) // 彩色图
                {
                    cBitmapImg = new Bitmap(nImageWidth, nImageHeight, PixelFormat.Format24bppRgb);
                    bitmapData = cBitmapImg.LockBits(new Rectangle(0, 0, nImageWidth, nImageHeight)
                                                                , ImageLockMode.WriteOnly
                                                                , PixelFormat.Format24bppRgb);
                    // 彩色图做深拷贝
                    bBitmapDataTemp = new byte[cMvdImg.GetImageData().stDataChannel[0].arrDataBytes.Length];
                    Array.Copy(cMvdImg.GetImageData().stDataChannel[0].arrDataBytes, bBitmapDataTemp, bBitmapDataTemp.Length);
                    nChannelNum = 3;
                }

                // bitmap彩色图按BGR存储，而MVDimg按RGB存储，改变存储顺序
                // 交换R和B
                if (MVD_PIXEL_FORMAT.MVD_PIXEL_RGB_RGB24_C3 == cMvdImg.PixelFormat)
                {
                    byte bTemp;
                    for (int i = 0; i < nImageWidth * nImageHeight; i++)
                    {
                        bTemp = bBitmapDataTemp[3 * i];
                        bBitmapDataTemp[3 * i] = bBitmapDataTemp[3 * i + 2];
                        bBitmapDataTemp[3 * i + 2] = bTemp;
                    }
                }

                // 考虑图像是否4字节对齐，bitmap要求4字节对齐，而mvdimage不要求对齐
                if (0 == nImageWidth % 4) // 4字节对齐时，直接拷贝
                {
                    Marshal.Copy(bBitmapDataTemp, 0, bitmapData.Scan0, nImageWidth * nImageHeight * nChannelNum);
                }
                else // 按步长逐行拷贝
                {
                    // 每行实际占用字节数
                    Int32 nRowPixelByteNum = nImageWidth * nChannelNum + 4 - (nImageWidth * nChannelNum % 4);
                    // 每行首字节首地址
                    IntPtr bitmapDataRowPos = IntPtr.Zero;
                    for (int i = 0; i < nImageHeight; i++)
                    {
                        // 获取每行第一个像素值的首地址
                        bitmapDataRowPos = new IntPtr(bitmapData.Scan0.ToInt64() + nRowPixelByteNum * i);
                        Marshal.Copy(bBitmapDataTemp, i * nImageWidth * nChannelNum, bitmapDataRowPos, nImageWidth * nChannelNum);
                    }
                }

                cBitmapImg.UnlockBits(bitmapData);
            }
            catch (MvdException ex)
            {
                if (null != cBitmapImg)
                {
                    cBitmapImg.UnlockBits(bitmapData);
                    cBitmapImg.Dispose();
                    cBitmapImg = null;
                }
                throw ex;
            }
            catch (System.Exception ex)
            {
                if (null != cBitmapImg)
                {
                    cBitmapImg.UnlockBits(bitmapData);
                    cBitmapImg.Dispose();
                    cBitmapImg = null;
                }
                throw ex;
            }
        }


    }
}
