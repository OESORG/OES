using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace OES.Model
{
    public class BaseEntity
    {

        protected string GenerateKey()
        {
            return Guid.NewGuid().ToString();
        }
    }
}
