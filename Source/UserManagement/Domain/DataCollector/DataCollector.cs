using System;
using System.Collections.Generic;
using System.Linq;
using Concepts;
using Dolittle.Domain;
using Domain.DataCollector.Changing;
using Domain.DataCollector.Registering;
using Events.DataCollector;

namespace Domain.DataCollector
{
    public class DataCollector : AggregateRoot
    {
        private readonly List<string> _numbers = new List<string>();
        private bool _isRegistered;

        public DataCollector(Guid id) : base(id)
        {
        }

        #region VisibleCommands

        public void RegisterDataCollector(
            string fullName, string displayName,
            int yearOfBirth, Sex sex, Language preferredLanguage,
            Location gpsLocation, IEnumerable<string> phoneNumbers, DateTimeOffset registeredAt
            )
        {
            if (_isRegistered)
            {
                //TODO: We might want to Apply an event here that signals that a new data collector has been registered
                throw new DataCollectorAlreadyRegistered($"DataCollector '{EventSourceId} {fullName} {displayName} is already registered'");
            }

            Apply(new DataCollectorRegistered
            (
                EventSourceId,
                fullName,
                displayName,
                yearOfBirth,
                (int)sex,
                (int)preferredLanguage,
                gpsLocation.Longitude,
                gpsLocation.Latitude,
                registeredAt
            ));

            foreach (var phoneNumber in phoneNumbers)
            {
                AddPhoneNumber(phoneNumber);
            }
        }

        public void ChangeLocation(Location location)
        {
            //if (!_isRegistered) //TODO: State is not persisted at the moment it seems
            //{
            //    throw new Exception("Datacollector not registered");
            //}

            Apply(new DataCollectorLocationChanged(EventSourceId, location.Latitude, location.Longitude));
        }

        public void ChangePreferredLanguage(Language language)
        {
            //if (!_isRegistered) //TODO: State is not persisted at the moment it seems
            //{
            //    throw new Exception("Datacollector not registered");
            //}

            Apply(new DataCollectorPrefferedLanguageChanged(EventSourceId, (int)language));
        }

        public void ChangeBaseInformation(string fullName, string displayName, int yearOfBirth, Sex sex)
        {
            //if (!_isRegistered) //TODO: State is not persisted at the moment it seems
            //{
            //    throw new Exception("Datacollector not registered");
            //}

            Apply(new DataCollectorUserInformationChanged(EventSourceId, fullName, displayName, yearOfBirth, (int)sex));
        }

        public void DeleteDataCollector()
        {
            Apply(new DataCollectorRemoved(
                EventSourceId
            ));
        }

        public void AddPhoneNumber(string number)
        {
            
            Apply(new PhoneNumberAddedToDataCollector(
                EventSourceId,
                number));

        }

        public void RemovePhoneNumbers(string number)
        {
            Apply(new PhoneNumberRemovedFromDataCollector(
                EventSourceId,
                number
            ));

        }

        #endregion



        #region On-methods

        private void On(DataCollectorRegistered @event)
        {
            _isRegistered = true;
        }

        private void On(PhoneNumberAddedToDataCollector @event)
        {
            _numbers.Add(@event.PhoneNumber);
        }

        private void On(PhoneNumberRemovedFromDataCollector @event)
        {
            _numbers.Remove(@event.PhoneNumber);
        }

        #endregion
    }
}

