﻿using System;
using System.Collections.Generic;
using System.Text;

namespace Core.Abstractions
{
    public interface IDataTypeMapper
    {
        string Map(string sourceType);
    }
}
