using System;
using CIS.HR.Models.DTO;

namespace CIS.HR.Models
{
    namespace DTO
    {
        public class ContactPreferencesDTO : DTO
        {
            public int ContactPreferencesId { get; set; }
            public int EmployeeId { get; set; }
            public DateTime DateEffective { get; set; }
            public string Phone1 { get; set; }
            public string Phone2 { get; set; }
            public string Email1 { get; set; }
            public string Email2 { get; set; }
            public string Extension { get; set; }
        }
    }

    public partial class Mapper
    {
        public virtual void MapToDTO(ContactPreferences model, ContactPreferencesDTO dto)
        {
            dto.EmployeeId = model.EmployeeId;
            dto.ContactPreferencesId = model.Id;
            dto.Phone1 = model.Phone1;
            dto.Phone2 = model.Phone2;
            dto.Email1 = model.Email1;
            dto.Email2 = model.Email2;
            dto.Extension = model.Extension;
            dto.DateEffective = model.DateEffective;
        }

        public virtual void MapToModel(ContactPreferencesDTO dto, ContactPreferences model)
        {
            model.Id = dto.ContactPreferencesId;
            model.EmployeeId = dto.EmployeeId;
            model.Phone1 = dto.Phone1;
            model.Phone2 = dto.Phone2;
            model.Email1 = dto.Email1;
            model.Email2 = dto.Email2;
            model.Extension = dto.Extension;
            model.DateEffective = dto.DateEffective;
        }
    }
}