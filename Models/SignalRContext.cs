﻿using System;
using System.Linq;
using Microsoft.EntityFrameworkCore;

public class SignalRContext : DbContext
{
	public SignalRContext(DbContextOptions<SignalRContext> options) : base(options)
    {
	}

	public DbSet<SignalRItem> SignalRItemList { get; set; }
}