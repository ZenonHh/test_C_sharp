using Microsoft.AspNetCore.Mvc;
using DoAnCSharp.AdminWeb.Models;
using DoAnCSharp.AdminWeb.Services;

namespace DoAnCSharp.AdminWeb.Controllers;

[ApiController]
[Route("api/[controller]")]
public class QRScansController : ControllerBase
{
    private readonly DatabaseService _db;

    public QRScansController(DatabaseService db)
    {
        _db = db;
    }

    [HttpGet("check/{userId}/{isPaid}")]
    public async Task<ActionResult> CheckQRScanLimit(int userId, bool isPaid)
    {
        try
        {
            var canScan = await _db.CanUserScanQRAsync(userId, isPaid);
            
            var limit = await _db.GetQRScanLimitByUserIdAsync(userId);
            
            return Ok(new
            {
                canScan = canScan,
                scanCount = limit?.ScanCount ?? 0,
                maxScans = isPaid ? "Unlimited" : "5",
                remainingScans = isPaid ? 0 : (5 - (limit?.ScanCount ?? 0)),
                isPaidUser = isPaid
            });
        }
        catch (Exception ex)
        {
            return BadRequest(new { error = ex.Message });
        }
    }

    [HttpPost("increment/{userId}")]
    public async Task<ActionResult> IncrementScanCount(int userId)
    {
        try
        {
            var limit = await _db.GetQRScanLimitByUserIdAsync(userId);
            if (limit == null)
                return NotFound(new { error = "QR Scan limit not found" });

            await _db.IncrementQRScanCountAsync(userId);
            return Ok(new { message = "Scan count incremented" });
        }
        catch (Exception ex)
        {
            return BadRequest(new { error = ex.Message });
        }
    }

    [HttpGet("user/{userId}")]
    public async Task<ActionResult<QRScanLimit>> GetQRScanLimit(int userId)
    {
        try
        {
            var limit = await _db.GetQRScanLimitByUserIdAsync(userId);
            if (limit == null)
                return NotFound(new { error = "QR Scan limit not found" });
            return Ok(limit);
        }
        catch (Exception ex)
        {
            return BadRequest(new { error = ex.Message });
        }
    }
}
