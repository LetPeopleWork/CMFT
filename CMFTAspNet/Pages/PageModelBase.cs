﻿using CMFTAspNet.Services.Interfaces;
using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.Mvc.RazorPages;

namespace CMFTAspNet.Pages
{
    public abstract class PageModelBase<T> : PageModel where T : class
    {
        protected PageModelBase(IRepository<T> repository)
        {
            Repository = repository;
        }

        [BindProperty]
        public T Entity { get; set; } = default!;

        protected IRepository<T> Repository { get; }

        public IActionResult OnGet(int? id)
        {
            var entity = GetById(id);

            if (entity == null)
            {
                return NotFound();
            }

            Entity = entity;

            return Page();
        }

        protected T? GetById(int? id)
        {
            return id.HasValue ? Repository.GetById(id.Value) : null;
        }
    }
}
