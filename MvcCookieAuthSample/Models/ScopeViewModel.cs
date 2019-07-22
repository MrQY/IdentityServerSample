namespace MvcCookieAuthSample.Models
{
    /// <summary>
    /// https://identityserver4.readthedocs.io/en/latest/reference/identity_resource.html
    /// </summary>
    public class ScopeViewModel
    {
        public string Name { get; set; }
        public string DisplayName { get; set; }
        public string Description { get; set; }
        public bool Required { get; set; }
        public bool Checked { get; set; }
        public bool Emphasize { get; set; }
    }
}