﻿using System;
using System.Collections.Generic;
using System.Text;

namespace OnlineShopCore.Data.Interfaces
{
    public interface ISortable
    {
        int SortOrder { set; get; }
    }
}