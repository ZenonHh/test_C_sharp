using Microsoft.AspNetCore.Mvc;
using DoAnCSharp.AdminWeb.Models;
using DoAnCSharp.AdminWeb.Services;

namespace DoAnCSharp.AdminWeb.Controllers;

[ApiController]
[Route("api/[controller]")]
public class POIsController : ControllerBase
{
    private readonly DatabaseService _db;

    public POIsController(DatabaseService db)
    {
        _db = db;
    }

    [HttpGet]
    public async Task<ActionResult<List<AudioPOI>>> GetAll()
    {
        try
        {
            var pois = await _db.GetAllPOIsAsync();
            return Ok(pois);
        }
        catch (Exception ex)
        {
            return BadRequest(new { error = ex.Message });
        }
    }

    [HttpGet("{id}")]
    public async Task<ActionResult<AudioPOI>> GetById(int id)
    {
        try
        {
            var poi = await _db.GetPOIByIdAsync(id);
            if (poi == null)
                return NotFound(new { error = "POI not found" });
            return Ok(poi);
        }
        catch (Exception ex)
        {
            return BadRequest(new { error = ex.Message });
        }
    }

    [HttpPost]
    public async Task<ActionResult> Create([FromBody] AudioPOI poi)
    {
        try
        {
            if (string.IsNullOrWhiteSpace(poi.Name))
                return BadRequest(new { error = "Name is required" });

            await _db.InsertPOIAsync(poi);
            return CreatedAtAction(nameof(GetById), new { id = poi.Id }, poi);
        }
        catch (Exception ex)
        {
            return BadRequest(new { error = ex.Message });
        }
    }

    [HttpPut("{id}")]
    public async Task<ActionResult> Update(int id, [FromBody] AudioPOI poi)
    {
        try
        {
            var existing = await _db.GetPOIByIdAsync(id);
            if (existing == null)
                return NotFound(new { error = "POI not found" });

            poi.Id = id;
            await _db.UpdatePOIAsync(poi);
            return Ok(new { message = "POI updated successfully" });
        }
        catch (Exception ex)
        {
            return BadRequest(new { error = ex.Message });
        }
    }

    [HttpDelete("{id}")]
    public async Task<ActionResult> Delete(int id)
    {
        try
        {
            var existing = await _db.GetPOIByIdAsync(id);
            if (existing == null)
                return NotFound(new { error = "POI not found" });

            await _db.DeletePOIAsync(id);
            return Ok(new { message = "POI deleted successfully" });
        }
        catch (Exception ex)
        {
            return BadRequest(new { error = ex.Message });
        }
    }
}
