using System;
using System.Text.Json;
using IUGOCare.Application.Common.Interfaces;
using IUGOCare.Infrastructure.Messaging.Mappers;
using IUGOCare.Messages.ClinicalToPatient.Common;
using IUGOCare.Messages.ClinicalToPatient.Observations;
using IUGOCare.Messages.ClinicalToPatient.Patients;
using IUGOCare.Messages.ClinicalToPatient.TargetRanges;
using MediatR;

namespace IUGOCare.Infrastructure.Messaging
{
    public class ExternalSystemEventName
    {
        public const string ObservationCreated = "ObservationCreated";
        public const string PatientCreated = "PatientCreated";
        public const string MessageAcknowledged = "MessageAcknowledged";
        public const string PatientInformationUpdated = "PatientInformationUpdated";
        public const string TargetRangesCreated = "TargetRangesCreated";
        public const string TargetRangesUpdated = "TargetRangesUpdated";
        public const string PatientEmergencyContactUpdated = "PatientEmergencyContactUpdated";
        public const string PatientCareProgramEnrollmentUpdated = "PatientCareProgramEnrollmentUpdated";
        public const string PatientEmailUpdated = "PatientEmailUpdated";
        public const string ObservationClassified = "ObservationClassified";
        public const string ObservationReviewed = "ObservationReviewed";
        public const string NewPatientEnrolledInCarePrograms = "NewPatientEnrolledInCarePrograms";
    }

    public class CommandFactory : ICommandFactory
    {
        public IRequest CreateCommand(string eventName, string messageBody)
        {
            switch (eventName)
            {
                case ExternalSystemEventName.ObservationCreated:
                    var observationCreatedEvent = JsonSerializer.Deserialize<ObservationCreatedDto>(messageBody);
                    return observationCreatedEvent.MapToCreateObservationCommand();
                case ExternalSystemEventName.PatientCreated:
                    var patientCreatedEvent = JsonSerializer.Deserialize<PatientCreatedDto>(messageBody);
                    return PatientCreatedDtoMappers.MapToRegisterPatientCommand(patientCreatedEvent);
                case ExternalSystemEventName.MessageAcknowledged:
                    var messageAcknowledgedEvent = JsonSerializer.Deserialize<MessageAcknowledgedDto>(messageBody);
                    return messageAcknowledgedEvent.MapToSetMessageAcknowledgedCommand();
                case ExternalSystemEventName.PatientInformationUpdated:
                    var patientInformationUpdatedEvent = JsonSerializer.Deserialize<PatientInformationUpdatedDto>(messageBody);
                    return PatientInformationUpdatedDtoMappers.MapToUpdatePatientInformationCommand(patientInformationUpdatedEvent);
                case ExternalSystemEventName.TargetRangesCreated:
                    var targetRangesCreatedEvent = JsonSerializer.Deserialize<TargetRangesCreatedDto>(messageBody);
                    return targetRangesCreatedEvent.MapToSetTargetRangesCommand();
                case ExternalSystemEventName.TargetRangesUpdated:
                    var targetRangesUpdatedEvent = JsonSerializer.Deserialize<TargetRangesUpdatedDto>(messageBody);
                    return targetRangesUpdatedEvent.MapToUpdateTargetRangesCommand();
                case ExternalSystemEventName.PatientEmergencyContactUpdated:
                    var patientEmergencyContactUpdatedEvent = JsonSerializer.Deserialize<PatientEmergencyContactUpdatedDto>(messageBody);
                    return patientEmergencyContactUpdatedEvent.MapToAddOrUpdateEmergencyContactCommand();
                case ExternalSystemEventName.PatientCareProgramEnrollmentUpdated:
                    var patientCareProgramEnrollmentUpdatedEvent = JsonSerializer.Deserialize<PatientCareProgramEnrollmentUpdatedDto>(messageBody);
                    return patientCareProgramEnrollmentUpdatedEvent.MapToSetPatientCareManagementEnrollmentCommand();
                case ExternalSystemEventName.PatientEmailUpdated:
                    var patientEmailUpdatedEvent = JsonSerializer.Deserialize<PatientEmailUpdatedDto>(messageBody);
                    return patientEmailUpdatedEvent.MapToUpdateEmailFromExternalSystemCommand();
                case ExternalSystemEventName.ObservationReviewed:
                    var observationReviewedEvent = JsonSerializer.Deserialize<ObservationReviewedDto>(messageBody);
                    return observationReviewedEvent.MapToReviewObservationCommand();
                case ExternalSystemEventName.ObservationClassified:
                    var observationClassifiedEvent = JsonSerializer.Deserialize<ObservationClassifiedDto>(messageBody);
                    return observationClassifiedEvent.MapToClassifyObservationCommand();
                case ExternalSystemEventName.NewPatientEnrolledInCarePrograms:
                    var newPatientEnrolledInCareProgramsEvent = JsonSerializer.Deserialize<NewPatientEnrolledInCareProgramsDto>(messageBody);
                    return newPatientEnrolledInCareProgramsEvent.MapToEnrollNewPatientFromExternalSystemCommand();
                default:
                    throw new ArgumentException($"Invalid event name {eventName} passed to CommandFactory.");
            }
        }
    }
}
