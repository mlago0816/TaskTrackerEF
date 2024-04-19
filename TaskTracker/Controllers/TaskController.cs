using Microsoft.AspNetCore.Mvc;
using System;
using System.Collections.Generic;
using System.Linq;
using TaskTracker.Models;

namespace TaskTracker.Controllers
{
    public class TaskController : Controller
    {
        private readonly TaskDbContext _context;

        public TaskController(TaskDbContext context)
        {
            _context = context;
        }

        public IActionResult Index()
        {
            var tasks = _context.Tasks.ToList();
            return View(tasks);
        }

        [HttpGet]
        public IActionResult Create()
        {
            return View();
        }

        [HttpPost]
        public IActionResult Create(TaskTracker.Models.Task task)
        {
            if (ModelState.IsValid)
            {
                if (task.DueDate == null)
                {
                    task.DueDate = DateTime.Today.AddHours(12);
                }

                _context.Tasks.Add(task);
                _context.SaveChanges();
                return RedirectToAction("Index");
            }
            return View(task);
        }

        [HttpGet]
        public IActionResult Edit(int id)
        {
            var task = _context.Tasks.Find(id);
            if (task == null)
            {
                return NotFound();
            }
            return View(task);
        }

        [HttpPost]
        public IActionResult Edit(TaskTracker.Models.Task updatedTask)
        {
            if (ModelState.IsValid)
            {
                var existingTask = _context.Tasks.Find(updatedTask.Id);
                if (existingTask != null)
                {
                    existingTask.Title = updatedTask.Title;
                    existingTask.Description = updatedTask.Description;

                    if (updatedTask.DueDate == null)
                    {
                        updatedTask.DueDate = DateTime.Today.AddHours(12);
                    }

                    existingTask.DueDate = updatedTask.DueDate;
                    existingTask.IsDone = updatedTask.IsDone;

                    _context.SaveChanges();
                }
                return RedirectToAction("Index");
            }
            return View(updatedTask);
        }

        public IActionResult Details(int id)
        {
            var task = _context.Tasks.Find(id);
            if (task == null)
            {
                return NotFound();
            }
            return View(task);
        }

        public IActionResult UpdateStatus(int id)
        {
            var task = _context.Tasks.Find(id);
            if (task != null)
            {
                task.IsDone = !task.IsDone;
                _context.SaveChanges();
            }
            return RedirectToAction("Index");
        }

        public IActionResult Delete(int id)
        {
            var task = _context.Tasks.Find(id);
            if (task != null)
            {
                _context.Tasks.Remove(task);
                _context.SaveChanges();
            }
            return RedirectToAction("Index");
        }
    }
}