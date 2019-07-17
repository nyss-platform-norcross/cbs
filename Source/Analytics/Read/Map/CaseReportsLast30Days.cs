using System;
using Dolittle.ReadModels;
using Concepts;
using Concepts.HealthRisk;
using System.Collections.Generic;
using MongoDB.Bson.Serialization.Attributes;
using MongoDB.Bson.Serialization.Options;

namespace Read.Map
{
    public class CaseReportsLast30Days : IReadModel
    {
        public Day Id { get; set; }
        
        [BsonDictionaryOptions(DictionaryRepresentation.ArrayOfDocuments)]
        public IDictionary<HealthRiskNumber, IList<CaseReportForMap>> CaseReportsPerHealthRisk { get; set; } 
    }
}
