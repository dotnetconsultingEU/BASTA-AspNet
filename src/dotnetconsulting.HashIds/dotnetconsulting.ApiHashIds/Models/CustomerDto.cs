#nullable disable

using AspNetCore.Hashids.Json;
using System.Text.Json.Serialization;

namespace dotnetconsulting.ApiHashIds.Models;

public class CustomerDto
{
    [JsonConverter(typeof(HashidsJsonConverter))]
    public int Id { get; set; }
    public int NonHashid { get; set; }
    public string FirstName { get; set; }
    public string LastName { get; set; }
}