using Engine.Models;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Engine.Actions
{
    // abstract so it can't be instantiated on its own
    public abstract class BaseAction
    {
        protected readonly GameItem _itemInUse;

        // tell the UI to display a string message
        public event EventHandler<string> OnActionPerformed;

        protected BaseAction(GameItem itemInUse)
        {
            _itemInUse = itemInUse;
        }

        protected void ReportResult(string result)
        {
            OnActionPerformed?.Invoke(this, result);
        }
    }
}
