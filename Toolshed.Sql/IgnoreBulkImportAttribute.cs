using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Toolshed
{
    /// <summary>
    /// Indicates that a BULK INSERT should not add this column when auto-adding column mapping. Used with Toolshed bulk insert helper methods
    /// </summary>
    [AttributeUsage(AttributeTargets.Property)]
    public class IgnoreBulkImportAttribute : Attribute
    {

    }
}
