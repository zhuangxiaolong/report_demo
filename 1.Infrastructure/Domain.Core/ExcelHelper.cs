using System;
using System.Collections.Generic;
using System.Data;
using System.IO;
using System.Reflection;
using NPOI.HSSF.UserModel;
using NPOI.SS.UserModel;
using NPOI.XSSF.UserModel;

namespace Domain.Core
{
   
    public class ExcelHelper : IDisposable
    {
        private IWorkbook workbook = null;
        private FileStream fs = null;
        private bool disposed;

        public ExcelHelper()
        {
            disposed = false;
        }
         public void ExportOneSheepExcel2003<T>(List<T> list, string filePath, int startRow = 1, int startColumn = 1)
        {
            int a = startRow - 1;  //接收传来的开始行，得到开始行下标
            int b = startColumn - 1;//接收传来的开始列，得到开始列下标
            //创建工作本
            HSSFWorkbook book = new HSSFWorkbook();
            //创建sheet
            ISheet sheet = book.CreateSheet("Sheet1");
            //获取类的映射
            Type t = typeof(T);
            //获取类属性
            PropertyInfo[] ps = t.GetProperties();
            #region 如果要创建列头
            /*
            创建列头(行)
            IRow headRow = sheet.CreateRow(a);
            for (int i = 0; i < ps.Length; i++)
            {
                row.CreateCell(b).SetCellValue(ps[i].Name);
                b++;
            }
            a++;
            */
            #endregion   
            //创建身体
            for (int j = 0; j < list.Count; j++)
            {
                IRow row = sheet.CreateRow(a);//创建身体的行
                //列下标回归
                b = startColumn - 1;
                //列
                for (int k = 0; k < ps.Length; k++)
                {
                    row.CreateCell(b).SetCellValue(ps[k].GetValue(list[j])!=null? ps[k].GetValue(list[j]).ToString():null);
                    b++;
                }
                a++;//开始下一行
            }
            //判断有没有这个文件
            if (File.Exists(filePath))
            {
                File.Delete(filePath);
            }
            //写入文件//没有就创建，有就打开
            using (FileStream fs = new FileStream(filePath, FileMode.Create, FileAccess.Write))
            {
                book.Write(fs);
            }
        }
         public void ExportOneSheepExcel2007<T>(List<T> list, string filePath, int startRow = 1, int startColumn = 1)
         {
             int a = startRow - 1;  //接收传来的开始行，得到开始行下标
             int b = startColumn - 1;//接收传来的开始列，得到开始列下标
             //创建工作本
             XSSFWorkbook book = new XSSFWorkbook();
             //创建sheet
             ISheet sheet = book.CreateSheet("Sheet1");
             //获取类的映射
             Type t = typeof(T);
             //获取类属性
             PropertyInfo[] ps = t.GetProperties();
             #region 如果要创建列头
             /*
             创建列头(行)
             IRow headRow = sheet.CreateRow(a);
             for (int i = 0; i < ps.Length; i++)
             {
                 row.CreateCell(b).SetCellValue(ps[i].Name);
                 b++;
             }
             a++;
             */
             #endregion   
             //创建身体
             for (int j = 0; j < list.Count; j++)
             {
                 IRow row = sheet.CreateRow(a);//创建身体的行
                 //列下标回归
                 b = startColumn - 1;
                 //列
                 for (int k = 0; k < ps.Length; k++)
                 {
                     row.CreateCell(b).SetCellValue(ps[k].GetValue(list[j])!=null? ps[k].GetValue(list[j]).ToString():null);
                     b++;
                 }
                 a++;//开始下一行
             }
             //判断有没有这个文件
             if (File.Exists(filePath))
             {
                 File.Delete(filePath);
             }
             //写入文件//没有就创建，有就打开
             using (FileStream fs = new FileStream(filePath, FileMode.Create, FileAccess.Write))
             {
                 book.Write(fs);
             }
         }
    }
}