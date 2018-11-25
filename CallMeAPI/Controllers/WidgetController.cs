﻿using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using Microsoft.AspNetCore.Mvc;
using CallMeAPI.Models;
using CallMeAPI.DTO;
using Microsoft.EntityFrameworkCore;

// For more information on enabling Web API for empty projects, visit https://go.microsoft.com/fwlink/?LinkID=397860

namespace CallMeAPI.Controllers
{
    [Route("api/widget")]
    public class WidgetController : Controller
    {

        private readonly MyDBContext context;

        public WidgetController(MyDBContext _context)
        {
            this.context = _context;
        }

        // GET: api/widget/new
        [HttpGet("new")]
        public async Task<IEnumerable<WidgetDTO>> GetAllWidgetsNew()
        {
            AuthController.ValidateAndGetCurrentUserName(this.HttpContext.Request);

            List<Widget> widgetList = new List<Widget>();
            widgetList = await context.Widgets.Include(widget => widget.User)
                                      .OrderBy(widget => widget.CreationDateTime)
                                      .Where(widget => string.IsNullOrEmpty(widget.AuthKey) 
                                             || string.IsNullOrEmpty(widget.Extension))
                                      .ToListAsync();

            List<WidgetDTO> widgetDTOList = new List<WidgetDTO>();
            foreach (Widget widget in widgetList)
            {
                widgetDTOList.Add(new WidgetDTO(widget));
            }

            return widgetDTOList;
        }

        // GET: api/widget/history
        [HttpGet("history")]
        public async Task<IEnumerable<WidgetDTO>> GetAllWidgetsHistory()
        {
            AuthController.ValidateAndGetCurrentUserName(this.HttpContext.Request);

            List<Widget> widgetList = new List<Widget>();
            widgetList = await context.Widgets.Include(widget => widget.User)
                                      .OrderBy(widget => widget.CreationDateTime)
                                      .Where(widget => !string.IsNullOrEmpty(widget.AuthKey)
                                             &&  !string.IsNullOrEmpty(widget.Extension))
                                      .ToListAsync();

            List<WidgetDTO> widgetDTOList = new List<WidgetDTO>();
            foreach (Widget widget in widgetList)
            {
                widgetDTOList.Add(new WidgetDTO(widget));
            }

            return widgetDTOList;
        }



        // GET: api/widget/{email}
        [HttpGet("user/{email}")]
        public async Task<IEnumerable<WidgetDTO>> GetAllWidgetsForUser(string email)
        {
            try
            {

                AuthController.ValidateAndGetCurrentUserName(this.HttpContext.Request);

                email = email.Replace("'", "");

                List<Widget> widgetList = new List<Widget>();
                widgetList = await context.Widgets.Include(widget => widget.User)
                                    .Where(widget => widget.UserID == email)
                                    .OrderBy(widget => widget.CreationDateTime)
                                    .ToListAsync();

                List<WidgetDTO> widgetDTOList = new List<WidgetDTO>();
                foreach (Widget widget in widgetList)
                {
                    widgetDTOList.Add(new WidgetDTO(widget));
                }

                return widgetDTOList;
            }catch(Exception ex)
            {
                throw ex;
            }
        }


        // GET: api/employee/{id}
        [HttpGet("{id}")]
        public async Task<ActionResult<WidgetDTO>> FindWidget(string id)
        {
            AuthController.ValidateAndGetCurrentUserName(this.HttpContext.Request);

            Widget widget = await context.Widgets
                                         .Include(e => e.User)
                                         .FirstOrDefaultAsync(e => e.ID.ToString() == id);

            return new WidgetDTO(widget);
        }



        [HttpPost]
        public async Task<IActionResult> Post([FromBody]WidgetDTO widget)
        {
            AuthController.ValidateAndGetCurrentUserName(this.HttpContext.Request);

            if (widget == null)
                return NotFound();

            string email = widget.Email;

            try
            {

                Widget oldwgt = await context.Widgets.FirstOrDefaultAsync(w => w.UserID == email
                                                                      && w.WidgetName == widget.WidgetName);

                if (oldwgt != null)
                    return BadRequest("DuplicateName");

                Widget wgt = new Widget(widget, context);
                context.Widgets.Add(wgt);
                await context.SaveChangesAsync();

                return Ok(new { Token = wgt.ID });
            }catch (Exception ex)
            {
                throw ex;
            }


        }

        // Put api/widget/5
        [HttpPut("{id}")]
        public async Task<IActionResult> Put(string id,[FromBody]WidgetDTO widgetDTO)
        {
            AuthController.ValidateAndGetCurrentUserName(this.HttpContext.Request);

            Widget widget = context.Widgets.Find(Guid.Parse(id));
            if (widget == null)
                return NotFound();

            Widget oldwgt = await context.Widgets.FirstOrDefaultAsync(w => w.UserID == widgetDTO.Email
                                                                      && w.WidgetName == widgetDTO.WidgetName
                                                                      && w.ID.ToString() != widgetDTO.ID.ToString()
                                                                     );

            if (oldwgt != null)
                return BadRequest("DuplicateName");



            widget.updateFromWidgetDTO(widgetDTO);

            await context.SaveChangesAsync();
            return Ok();
        }


        // Put api/widget/status/5
        [HttpPut("status/{id}")]
        public async Task<IActionResult> Disable(string id, [FromBody]WidgetDTO widgetDTO)
        {
            AuthController.ValidateAndGetCurrentUserName(this.HttpContext.Request);

            Widget widget = context.Widgets.Find(Guid.Parse(id));
            if (widget == null)
                return NotFound();

            widget.Status = widgetDTO.Status;

            await context.SaveChangesAsync();
            return Ok();
        }

        // Put api/widget/extension/5
        [HttpPut("extension/{id}")]
        public async Task<IActionResult> UpdateExtension(string id, [FromBody]WidgetDTO widgetDTO)
        {
            AuthController.ValidateAndGetCurrentUserName(this.HttpContext.Request);

            Widget widget = await context.Widgets
                                         .Include(e => e.User)
                                         .FirstOrDefaultAsync(e => e.ID.ToString() == id);

            if (widget == null)
                return NotFound();

            widget.AuthKey = widgetDTO.AuthKey;
            widget.Extension = widgetDTO.Extension;

            try{
                
                EmailManager.SendActivationWidgetNotification(widget);
            }
            catch(Exception)
            {
                
            }



            await context.SaveChangesAsync();
            return Ok();
        }

        // DELETE api/widget/5
        [HttpDelete("{id}")]
        public async Task<IActionResult> Delete(string id)
        {
            try
            {
                AuthController.ValidateAndGetCurrentUserName(this.HttpContext.Request);


                List<CallbackSchedule> callbackSchedules = await context.CallbackSchedules.Where(cs => cs.widgetID == Guid.Parse(id)).ToListAsync();

                context.CallbackSchedules.RemoveRange(callbackSchedules);
                context.Widgets.Remove(context.Widgets.Find(Guid.Parse(id)));

                await context.SaveChangesAsync();
                return Ok();
            }catch (Exception ex)
            {
                throw ex;
            }
        }

    }
}
