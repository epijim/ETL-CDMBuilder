﻿using System;
using System.Collections.Generic;
using System.Data;
using org.ohdsi.cdm.framework.entities.Builder;
using org.ohdsi.cdm.framework.entities.Omop;
using org.ohdsi.cdm.framework.shared.Extensions;

namespace org.ohdsi.cdm.framework.core.Definitions
{
   public class PersonDefinition : EntityDefinition
   {
      public string PersonSourceValue { get; set; }
      public string Gender { get; set; }
      public string YearOfBirth { get; set; }
      public string MonthOfBirth { get; set; }
      public string DayOfBirth { get; set; }
      public string Location { get; set; }
      public string Country { get; set; }

      public string CareSiteId { get; set; }

      public string Race { get; set; }
      public string RaceConceptId { get; set; }
      public string Ethnicity { get; set; }
      public string EthnicityConceptId { get; set; }
      public string LocationId { get; set; }
      public string ObservationPeriodGap { get; set; }

      // CDM v5 props
      public string TimeOfBirth { get; set; }
      public string GenderSourceConceptId { get; set; }
      public string RaceSourceConceptId { get; set; }
      public string EthnicitySourceConceptId { get; set; }
      public string PeriodTypeConceptId { get; set; }

      public override IEnumerable<IEntity> GetConcepts(Concept concept, IDataRecord reader, KeyMasterOffset keyOffset)
      {
         var locationSourceValue = reader.GetString(Location);
         int? locationId = null;
         if (!string.IsNullOrEmpty(Location))
         {
            locationId = Vocabulary.LookupLocation(locationSourceValue);
         }

         var observationPeriodGap = 32;
         if(!string.IsNullOrEmpty(ObservationPeriodGap))
         {
            observationPeriodGap = int.Parse(ObservationPeriodGap);
         }

         var genderSource = reader.GetString(Gender);
         var genderConceptId = Vocabulary.LookupGender(genderSource);
         
         var personId = reader.GetLong(PersonId);
         if(personId.HasValue)
         {
            Dictionary<string, string> additionalFields = null;
            if (AdditionalFields != null)
            {
               additionalFields = new Dictionary<string, string>(AdditionalFields.Length);
               foreach (var additionalField in AdditionalFields)
               {
                  additionalFields.Add(additionalField.ToLower(), reader.GetString(additionalField));
               }
            }

            yield return new Person
                            {
                               ObservationPeriodGap = observationPeriodGap,
                               AdditionalFields = additionalFields,
                               PersonId = personId.Value,
                               StartDate = reader.GetDateTime(StartDate),
                               EndDate = reader.GetDateTime(EndDate),
                               PersonSourceValue = reader.GetString(PersonSourceValue),
                               GenderSourceValue = genderSource,
                               GenderConceptId = genderConceptId ?? 0,
                               LocationId = locationId,
                               YearOfBirth = reader.GetInt(YearOfBirth),
                               MonthOfBirth = reader.GetInt(MonthOfBirth),
                               DayOfBirth = reader.GetInt(DayOfBirth),
                               LocationSourceValue = locationSourceValue,
                               CareSiteId = reader.GetInt(CareSiteId),
                               EthnicitySourceValue = reader.GetString(Ethnicity),
                               EthnicityConceptId = reader.GetInt(EthnicityConceptId) ?? 0,
                               RaceSourceValue = reader.GetString(Race),
                               RaceConceptId = reader.GetInt(RaceConceptId) ?? 0,
                               ProviderId = reader.GetInt(ProviderId),
                               TimeOfBirth = reader.GetTime(TimeOfBirth),
                               GenderSourceConceptId = reader.GetInt(GenderSourceConceptId) ?? 0, // CCAE
                               RaceSourceConceptId = reader.GetInt(RaceSourceConceptId) ?? 0,
                               EthnicitySourceConceptId = reader.GetInt(EthnicitySourceConceptId) ?? 0,
                               TypeConceptId = reader.GetInt(PeriodTypeConceptId),
                            };
         }
      }
   }
}
