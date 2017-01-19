using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Windows.Controls;

namespace DoYourHomework.Pages
{
    public interface IPage
    {
        int PageIndex { get; }
        event EventHandler OnCompleted;
        void OnLoadCompleted();
    }
}
