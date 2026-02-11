using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace MAShop.DAL.Validation
{
    public class MinValue : ValidationAttribute
    {
        private readonly int _length;
        public MinValue(int length = 10)
        {
            _length = length;
        }

        public override bool IsValid(object? value)
        {
            if(value is int val)
            {
                if(val > _length)
                {
                    return true;
                }
            }
            return false;
        }

        public override string FormatErrorMessage(string name)
        {
            return $"{name} is Invalid";
        }
    }
}
