using System;
using System.Collections.Generic;
using System.Text;

namespace rapidyank.Rapid
{
    /// <summary>
    /// Displayed event
    /// <para>Отображаемое событие</para>
    /// </summary>
    /// <remarks>Свойства имеют короткие имена для передачи в формате JSON</remarks>
    public class DispEvent
    {
        /// <summary>
        /// Конструктор
        /// </summary>
        public DispEvent()
        {
            Num = 0;
            Time = "";
            Obj = "";
            KP = "";
            Cnl = "";
            Text = "";
            Ack = "";
            Color = "";
            Sound = false;
        }

        /// <summary>
        /// Получить или установить порядковый номер
        /// </summary>
        public int Num { get; set; }
        /// <summary>
        /// Получить или установить отформатированную дату и время
        /// </summary>
        public string Time { get; set; }
        /// <summary>
        /// Получить или установить наименование объекта
        /// </summary>
        public string Obj { get; set; }
        /// <summary>
        /// Получить или установить наименование КП
        /// </summary>
        public string KP { get; set; }
        /// <summary>
        /// Получить или установить наименование входного канала
        /// </summary>
        public string Cnl { get; set; }
        /// <summary>
        /// Получить или установить текст события
        /// </summary>
        public string Text { get; set; }
        /// <summary>
        /// Получить или установить информацию о квитировании
        /// </summary>
        public string Ack { get; set; }
        /// <summary>
        /// Получить или установить цвет
        /// </summary>
        public string Color { get; set; }
        /// <summary>
        /// Получить или установить признак воспроизведения звука
        /// </summary>
        public bool Sound { get; set; }
    }
}