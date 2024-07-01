﻿using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using GSqlQuery.SQLServer.Benchmark.Data.Tables;

namespace GSqlQuery.SQLServer.Benchmark.Data.Transform
{
    internal class Actors : TransformTo<Actor>
    {
        public Actors() : base(4)
        {
        }

        public override Actor CreateEntity(IEnumerable<PropertyValue> propertyValues)
        {
            long actorId = default;
            string firstName = default;
            string lastName = default;
            DateTime lastUpdate = default;


            foreach (PropertyValue item in propertyValues)
            {
                switch (item.Property.PropertyInfo.Name)
                {
                    case nameof(Actor.ActorId):
                        actorId = (long)item.Value;
                        break;
                    case nameof(Actor.FirstName):
                        firstName = (string)item.Value;
                        break;
                    case nameof(Actor.LastName):
                        lastName = (string)item.Value;
                        break;
                    case nameof(Actor.LastUpdate):
                        lastUpdate = (DateTime)item.Value;
                        break;
                    default:
                        break;
                }
            }

            return new Actor(actorId, firstName, lastName, lastUpdate);
        }
    }
}
