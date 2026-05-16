namespace Linka.DtoLayer.CatalogDtos.ContactDtos

{
    public class UpdateContactDto
    {
        public string ContactId { get; set; }
        public string NameSurname { get; set; }
        public string Email { get; set; }
        public string Subject { get; set; }
        public string Message { get; set; }
        public string IsRead { get; set; }
        public DateTime SendDate { get; set; }
    }
}
