

namespace Ams.Api.Dto.Requests
{
    public class CreateAppointmentRequestsDto
    {
        public string PatientName { get; set; }
        public string PatientContact { get; set; }
        public DateTime AppointmentDateTime { get; set; }
        public int DoctorId { get; set; }
    }
    public class UpdateAppointmentRequestsDto
    {
        public int Id { get; set; }
        public string PatientName { get; set; }
        public string PatientContact { get; set; }
        public DateTime AppointmentDateTime { get; set; }
        public int DoctorId { get; set; }
    }
    public class GetAppointmentDto
    {
        public int Id { get; set; }
        public string PatientName { get; set; }
        public string PatientContact { get; set; }
        public DateTime AppointmentDateTime { get; set; }
        public int DoctorId { get; set; }
        public string DoctorName { get; set; }
    }
}
