using System;
using System.Collections.Generic;
using System.Text;

namespace Repart.Common.Commands.Formation
{
    public class CreateFormation : ICommand
    {
        public string Name { get; set; }
        public string Abreviation { get; set; }
    }
}
