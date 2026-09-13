using System;

namespace Cpa.Application.Exceptions
{
    public class NotFoundException : Exception
    {
        public NotFoundException()
            : base("ไม่พบข้อมูล")
        {

        }
        public NotFoundException(string name, object key) : base($"Entity {name} with Id ({key}) was not found.")
        {

        }
    }
}
