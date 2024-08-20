﻿using HospitalManagementSystem.BusinessObject.Models;

namespace HospitalManagementSystem.DataAccess.Repositories.IRepository
{
    public interface IAppointmentRepository
    {
        Task<IEnumerable<Appointment>> GetAllAppointmentsAsync();
        Task<Appointment> GetAppointmentByIdAsync(int appointmentId);
        Task AddAppointmentAsync(Appointment appointment);
        Task UpdateAppointmentAsync(Appointment appointment);
        Task DeleteAppointmentAsync(int appointmentId);
    }
}