using Microsoft.AspNetCore.Mvc;
using Microsoft.EntityFrameworkCore;
using RunPlanner.Models;

public class GpxTrackController : Controller
{
    private readonly GpxDbContext _dbContext;

    public GpxTrackController(GpxDbContext dbContext)
    {
        _dbContext = dbContext;
    }



}
