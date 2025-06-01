using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Dungeon.Resources.Compiler
{
    internal class ResourceCompilerConfiguration
    {
        /// <summary>
        /// Логировать работу с ресурсами
        /// </summary>
        public bool IsLogging { get; set; }

        /// <summary>
        /// Путь до проекта
        /// </summary>
        public string PathProject { get; set; }

        /// <summary>
        /// Пусть к бинарникам
        /// </summary>
        public string PathBin { get; set; }

        /// <summary>
        /// Путь к папке данных в <see cref="PathBin"/>
        /// </summary>
        public string PathData { get; set; }

        /// <summary>
        /// Путь к репозиторию
        /// </summary>
        public string PathRepository { get; set; }
    }
}
