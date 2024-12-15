using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Kursovaya19.Classes
{
    public class Material
    {
        public string NameOfMaterial { get; set; }
        public int Quantity { get; set; }
        public string Unit { get; set; }

        public Material(string nameOfMaterial, int quantity, string unit)
        {
            NameOfMaterial = nameOfMaterial;
            Quantity = quantity;
            Unit = unit;
        }

        public Material() { }

        public Material(Material other)
        {
            NameOfMaterial = other.NameOfMaterial;
            Quantity = other.Quantity;
            Unit = other.Unit;
        }
    }
}
