using System;

namespace TotalDecoupling.DataAccessLayer.Entities;

public class Person
{
    public Guid Id { get; set; }

    public string FirstName { get; set; }

    public string LastName { get; set; }

    public DateTime CreateDate { get; set; }
}
