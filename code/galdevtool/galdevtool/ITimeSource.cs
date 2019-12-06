using System;
using System.Collections.Generic;
using System.Text;

namespace galdevtool
{
    public interface ITimeSource
    {
        DateTime Now();
    }
}
