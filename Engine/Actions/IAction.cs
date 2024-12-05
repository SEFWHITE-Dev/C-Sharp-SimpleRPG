using Engine.Models;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Engine.Actions
{
    // an interface is a data-type, we can store any object that implements an interface into a
    // variable/property/param that uses the interface as its datatype

    // an interface is a contract, or set of requirements that says
    // 'all Action classes must have these properties, events, and functions'
    public interface IAction
    {
        event EventHandler<string> OnActionPerformed; // event to report the action's results
        void Execute(LivingEntity actor, LivingEntity target);
    }
}
