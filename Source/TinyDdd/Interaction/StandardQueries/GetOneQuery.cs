﻿using System;
using System.Linq.Expressions;
using SwissKnife;

namespace TinyDdd.Interaction.StandardQueries
{
    public class GetOneQuery<T> : IQuery<Option<T>> where T : class
    {
        public Expression<Func<T, bool>> Criteria { get; set; }
    }
}