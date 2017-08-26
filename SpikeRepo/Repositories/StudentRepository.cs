using EFCoreSpike5.Models;
using SpikeRepo.Abstract;
using System;
using System.Collections.Generic;
using System.Text;

namespace SpikeRepo.Repositories
{
    public class StudentRepository: EntityBaseRepository<Student>, IStudentRepository
    {
        public StudentRepository(EFCoreSpikeContext context): base(context)
        {
        }
    }
}
