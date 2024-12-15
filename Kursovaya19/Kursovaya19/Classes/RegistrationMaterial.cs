namespace Kursovaya19.Classes
{
    public class RegistrationMaterial : Material
    {
        public double Price { get; set; }

        public RegistrationMaterial(string nameOfMaterial, double price, int quantity, string unit) : base(nameOfMaterial, quantity, unit)
        {
            Price = price;
        }

        public RegistrationMaterial() { }

        public RegistrationMaterial(RegistrationMaterial other) : base(other) { }
    }
}
