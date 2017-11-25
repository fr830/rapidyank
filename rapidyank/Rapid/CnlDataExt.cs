using System;
using System.Collections.Generic;
using System.Text;

namespace rapidyank.Rapid
{
    /// <summary>
    /// Расширенные данные входого канала
    /// </summary>
    public class CnlDataExt
    {
        /// <summary>
        /// Конструктор
        /// </summary>
        public CnlDataExt(int cnlNum)
        {
            CnlNum = cnlNum;
            Val = 0.0;
            Stat = 0;
            Text = "";
            TextWithUnit = "";
            Color = "";
        }

        /// <summary>
        /// Получить номер входного канала
        /// </summary>
        public int CnlNum { get; private set; }
        /// <summary>
        /// Получить или установить значение
        /// </summary>
        public double Val { get; set; }
        /// <summary>
        /// Получить или установить статус
        /// </summary>
        public int Stat { get; set; }
        /// <summary>
        /// Получить или установить отображаемый текст
        /// </summary>
        public string Text { get; set; }
        /// <summary>
        /// Получить или установить отображаемый текст с размерностью
        /// </summary>
        public string TextWithUnit { get; set; }
        /// <summary>
        /// Получить или установить цвет
        /// </summary>
        public string Color { get; set; }
    }
}
