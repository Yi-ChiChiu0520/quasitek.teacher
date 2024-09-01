using Microsoft.AspNetCore.Mvc;
using quasitekWeb.Models;
using quasitekWeb.Data;
using System;
namespace quasitekWeb.Controllers;

public class DeviceController : Controller
{
    private ApplicationDbContext _db;

    public DeviceController(ApplicationDbContext db){
        _db = db;
    }
public IActionResult Index(string searchTerm)
{
    List<Device> devices;

    // If no search term is provided, show all devices
    if (string.IsNullOrEmpty(searchTerm))
    {
        devices = _db.Device.ToList(); // Load all devices
    }
    else
    {
        // Filter devices by search term
        devices = _db.Device
            .Where(d => d.DeviceName==searchTerm || d.DeviceModel==searchTerm|| d.DeviceId.ToString()==searchTerm)
            .ToList();

        if (devices.Count == 0)
        {
            ViewBag.ErrorMessage = $"Device: {searchTerm} not found, please enter the correct device ID, name, or model.";
            
        }
    }

    return View(devices);
}


    [HttpPost]
    public IActionResult SignUpDevice(Device device){
        _db.Device.Add(device);
        _db.SaveChanges();

        return RedirectToAction("Index");
    }
    public IActionResult SignUp(){

        return View();
    }

    [HttpPost]
    public IActionResult DeleteDevice(string searchTerm){
        var devices = string.IsNullOrEmpty(searchTerm) 
                ? _db.Device.ToList() 
                : _db.Device.Where(s => s.DeviceName.Contains(searchTerm) || s.DeviceId.ToString().Contains(searchTerm)).ToList();
         if (devices.Any())
        {
            _db.Device.RemoveRange(devices);
            _db.SaveChanges();
        }
        return RedirectToAction("Index");
    }
    
    public IActionResult Delete(){
        return View();
    }

    [HttpPost]
    public IActionResult UpdateDevice(int deviceId, string deviceName, string deviceModel, DateTime purchaseDate, DateTime expireDate){
        var device = _db.Device.FirstOrDefault(s => s.DeviceId == deviceId);
        if (device != null) 
        {
            device.DeviceName = deviceName;
            device.DeviceModel = deviceModel;
            device.PurchaseDate = purchaseDate;
            device.ExpireDate = expireDate;
            _db.SaveChanges();
        }
        return RedirectToAction("Index");
    }

    [HttpGet]
    public IActionResult Update()
    {
        return View();
    }
    // [HttpPost]
    // public IActionResult Search(string searchTerm)
    // {
    //     var devices = _db.Device
    //                     .Where(d => d.DeviceName.Contains(searchTerm) || d.DeviceId.ToString() == searchTerm)
    //                     .ToList();

    //     return View(devices); // Passing the filtered devices to the view
    // }
    // public IActionResult Search()
    // {
    //     return View();
    // }

}