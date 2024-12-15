namespace Kursovaya19.Classes
{
    public class EstimateMaterial : Material
    {
        public EstimateMaterial(string nameOfMaterial, int quantity, string unit) : base(nameOfMaterial, quantity, unit) { }

        public EstimateMaterial() { }

        public EstimateMaterial(EstimateMaterial other) : base(other) { }
    }
}
