namespace Kursovaya19.Classes
{
    public class RequestMaterial : Material
    {
        public RequestMaterial(string nameOfMaterial, int quantity, string unit) : base(nameOfMaterial, quantity, unit) { }

        public RequestMaterial() { }

        public RequestMaterial(RequestMaterial other) : base(other) { }
    }
}
