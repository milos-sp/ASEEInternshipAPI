﻿using System.ComponentModel.DataAnnotations;
using System.Runtime.InteropServices;

namespace ProductAPI.Models
{
    public enum TransactionKind
    {
        dep, wdw, pmt, fee, inc, rev, adj, lnd, lnr, fcx, aop, acl, spl, sal
    }
}
