namespace Nabunassar.Entities
{
    internal class PersonaBinded
    {
        protected Persona Persona { get; set; }

        public virtual void BindPersona(Persona persona)
        {
            Persona = persona;
        }
    }
}
