﻿namespace TotalDecoupling.BusinessLayer.Models;

public enum FailureReason
{
    None,
    ItemNotFound,
    Forbidden,
    DatabaseError,
    ClientError,
    GenericError
}
