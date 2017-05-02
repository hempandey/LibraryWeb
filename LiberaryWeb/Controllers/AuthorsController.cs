using System;
using System.Collections.Generic;
using System.Data;
using System.Data.Entity;
using System.Linq;
using System.Net;
using System.Web;
using System.Web.Mvc;
using LibraryWeb;
using System.Globalization;
using LiberaryWeb.ViewModels;
using Kendo.Mvc;
using Kendo.Mvc.Extensions;
using Kendo.Mvc.UI;
using Kendo.Mvc.Infrastructure;
using System.Collections;
using System.Linq.Expressions;
using System.ComponentModel;

namespace LiberaryWeb.Controllers
{
    
    //class Car
    //{
    //    public string Name { get; set; }
    //    public string Model { get; set; }
    //    public string Color { get; set; }
    //}

        

    public class AuthorsController : Controller
    {
               
        private Library db = new Library();


        // GET: Authors
        public ActionResult Index()
        {
                       
            ////return View(db.Authors.ToList());
            return View();
        }


// for Author index, we created new function 
        public ActionResult GetAllAuthors()
        {
           List<AuthorViewModel> authorVM = new List<AuthorViewModel>();
            foreach (var item in db.Authors.ToList())
            {
                authorVM.Add(new AuthorViewModel() { Id = item.Id, Name = item.Name, DoB = String.Format("{0:MM/dd/yyyy}",item.DoB), Email = item.Email, Phone = item.Phone });
            }



            return Json(authorVM, JsonRequestBehavior.AllowGet);            

        }


        public ActionResult CustomAjaxBinding_Read([DataSourceRequest] DataSourceRequest request)
        {
            //List<AuthorViewModel> authorVM = new List<AuthorViewModel>();
            //foreach (var item in db.Authors.ToList())
            //{
            //    authorVM.Add(new AuthorViewModel() { Id = item.Id, Name = item.Name, DoB = String.Format("{0:MM/dd/yyyy}", item.DoB), Email = item.Email, Phone = item.Phone });
            //}

            //var result = authorVM.ToDataSourceResult(request);
            //return Json(result,JsonRequestBehavior.AllowGet);

            //var dataContext = new SampleEntities();

            // Convert to view model to avoid JSON serialization problems due to circular references.
            IQueryable<AuthorViewModel> orders = db.Authors.Select(o => new AuthorViewModel
            {
                Id = o.Id,
                Name = o.Name,
                Email = o.Email,
                DoB = String.Format("{0:MM/dd/yyyy}", o.DoB),
                Phone=o.Phone
            });

            orders = orders.ApplyOrdersFiltering(request.Filters);

            var total = orders.Count();

            orders = orders.ApplyOrdersSorting(request.Groups, request.Sorts);

            if (!request.Sorts.Any())
            {
                // Entity Framework doesn't support paging on unsorted data.
                orders = orders.OrderBy(o => o.Id);
            }

            orders = orders.ApplyOrdersPaging(request.Page, request.PageSize);

            IEnumerable data = orders.ApplyOrdersGrouping(request.Groups);

            var result = new DataSourceResult()
            {
                Data = data,
                Total = total
            };

            return Json(result);




        }



        // GET: Authors/Details/5
        public ActionResult Details(int? id)
        {
            if (id == null)
            {
                return new HttpStatusCodeResult(HttpStatusCode.BadRequest);
            }
            Author author = db.Authors.Find(id);
            if (author == null)
            {
                return HttpNotFound();
            }
            return View(author);
        }


        // GET: Authors/Details/ for Modal pop up window 
        public ActionResult GetAuthorDetailsForModal(int? id)
        {
            if (id == null)
            {
                return new HttpStatusCodeResult(HttpStatusCode.BadRequest);
            }
            Author author = db.Authors.Find(id);
            if (author == null)
            {
                return HttpNotFound();
            }

            AuthorViewModel authVM = new AuthorViewModel() { Id = author.Id, Name = author.Name, Email = author.Email, DoB = String.Format("{0:MM/dd/yyyy}", author.DoB), Phone = author.Phone };

            
            return Json(authVM, JsonRequestBehavior.AllowGet);
        }




        // GET: Authors/Create
        public ActionResult Create()
        {
            return View();
        }

        
        [HttpPost]
        //POST: Authors/Create
        //To protect from overposting attacks, please enable the specific properties you want to bind to, for 
        // more details see http://go.microsoft.com/fwlink/?LinkId=317598.   
        //[ValidateAntiForgeryToken]
        public ActionResult Create(Author author)
        {
            ResponseMessage responseMessage;
            if (string.IsNullOrEmpty(author.Name))
            {
                responseMessage = new ResponseMessage()
                {
                    Success = false,
                    Message = "Name is required."
                };

                //responseMessage = new ResponseMessage();
                //responseMessage.Success = false;
                //responseMessage.Message = "";



                return Json(responseMessage, JsonRequestBehavior.AllowGet);
            }
            if (string.IsNullOrEmpty(author.Email))
            {
                responseMessage = new ResponseMessage()
                {
                    Success = false,
                    Message = "Email Name is required."
                };
                return Json(responseMessage, JsonRequestBehavior.AllowGet);
            }


           

            //var checkName = db.Authors.Where(e => e.Name == author.Name).ToList();

            ////===== Lambda Expression
            var checkEmail = db.Authors.Where(e => e.Email == author.Email).ToList();

            ////===== LINQ Query
            var queryCheckEmail = from auth in db.Authors
                                  where auth.Email == author.Email
                                  select auth;


            //if (checkName.Count != 0)
            //{
            //    responseMessage = new ResponseMessage()
            //    {
            //        Success = false,
            //        Message = "Name " + author.Name + " already exists."
            //    };
            //    return Json(responseMessage, JsonRequestBehavior.AllowGet);
            //}

            if (checkEmail.Count != 0)
            {
                responseMessage = new ResponseMessage()
                {
                    Success = false,
                    Message = "Email " + author.Email + " already exists."
                };
                return Json(responseMessage, JsonRequestBehavior.AllowGet);
            }
            try
            {
                db.Authors.Add(author);
                db.SaveChanges();
                responseMessage = new ResponseMessage()
                {
                    Success = true,
                    Message = "Author information successfully saved."
                };

            }
            catch (Exception)
            {

                responseMessage = new ResponseMessage()
                {
                    Success = false,
                    Message = "Something went wrong."
                };
            }
            return Json(responseMessage, JsonRequestBehavior.AllowGet);

        }
          
    // GET: Authors/Edit/5
    public ActionResult Edit(int? id)
        {
            if (id == null)
            {
                return new HttpStatusCodeResult(HttpStatusCode.BadRequest);
            }
            Author author = db.Authors.Find(id);
                       
            if (author == null)
            {
                return HttpNotFound();
            }
            return View(author);
        }

        // POST: Authors/Edit/5
        // To protect from overposting attacks, please enable the specific properties you want to bind to, for 
        // more details see http://go.microsoft.com/fwlink/?LinkId=317598.
        [HttpPost]
        //[ValidateAntiForgeryToken]     
        public ActionResult Edit(Author author)
        {
            //validation for firsat name, email, login-this validation will make sure textbox is not null 
            ResponseMessage responseMessage;


            if (author.Id <= 0)
            {
                responseMessage = new ResponseMessage()
                {
                    Success = false,
                    Message = "Author id is empty."
                };
                return Json(responseMessage, JsonRequestBehavior.AllowGet);
            }

            if (string.IsNullOrEmpty(author.Name))
            {
                responseMessage = new ResponseMessage()
                {
                    Success = false,
                    Message = "Name is required."
                };
                return Json(responseMessage, JsonRequestBehavior.AllowGet);
            }
            if (string.IsNullOrEmpty(author.Email))
            {
                responseMessage = new ResponseMessage()
                {
                    Success = false,
                    Message = "Email is required."
                };
                return Json(responseMessage, JsonRequestBehavior.AllowGet);
            }
            
            try
            {
                db.Entry(author).State = EntityState.Modified;
                db.SaveChanges();

                responseMessage = new ResponseMessage()
                {
                    Success = true,
                    Message = "Done"
                };
            }
            catch (Exception)
            {

                responseMessage = new ResponseMessage()
                {
                    Success = false,
                    Message = "Something went wrong."
                };
            }

            return Json(responseMessage, JsonRequestBehavior.AllowGet);
        }




        // GET: Authors/Delete/5
        public ActionResult Delete(int? id)
        {
            if (id == null)
            {
                return new HttpStatusCodeResult(HttpStatusCode.BadRequest);
            }
            Author author = db.Authors.Find(id);
            if (author == null)
            {
                return HttpNotFound();
            }
            return View(author);
        }

        // POST: Authors/Delete/5
        [HttpPost, ActionName("Delete")]
        [ValidateAntiForgeryToken]
        public ActionResult DeleteConfirmed(int id)
        {
            Author author = db.Authors.Find(id);
            db.Authors.Remove(author);
            db.SaveChanges();
            return RedirectToAction("Index");
        }

        protected override void Dispose(bool disposing)
        {
            if (disposing)
            {
                db.Dispose();
            }
            base.Dispose(disposing);
        }
    }


    


}
public static class AjaxCustomBindingExtensions
{
    public static IQueryable<AuthorViewModel> ApplyOrdersPaging(this IQueryable<AuthorViewModel> data, int page, int pageSize)
    {
        if (pageSize > 0 && page > 0)
        {
            data = data.Skip((page - 1) * pageSize);
        }

        data = data.Take(pageSize);

        return data;
    }

    public static IEnumerable ApplyOrdersGrouping(this IQueryable<AuthorViewModel> data, IList<GroupDescriptor>groupDescriptors)
    {
        if (groupDescriptors != null && groupDescriptors.Any())
        {
            Func<IEnumerable<AuthorViewModel>, IEnumerable<AggregateFunctionsGroup>> selector = null;
            foreach (var group in groupDescriptors.Reverse())
            {
                if (selector == null)
                {
                    if (group.Member == "Id")
                    {
                        selector = Orders => BuildInnerGroup(Orders, o => o.Id);
                    }
                    else if (group.Member == "Name")
                    {
                        selector = Orders => BuildInnerGroup(Orders, o => o.Name);
                    }
                    else if (group.Member == "Email")
                    {
                        selector = Orders => BuildInnerGroup(Orders, o => o.Email);
                    }
                    else if (group.Member == "DoB")
                    {
                        selector = Orders => BuildInnerGroup(Orders, o => o.DoB);
                    }
                    else if (group.Member == "Phone")
                    {
                        selector = Orders => BuildInnerGroup(Orders, o => o.Phone);
                    }
                }
                else
                {
                    if (group.Member == "Id")
                    {
                        selector = BuildGroup(o => o.Id, selector);
                    }
                    else if (group.Member == "Name")
                    {
                        selector = BuildGroup(o => o.Name, selector);
                    }
                    else if (group.Member == "Email")
                    {
                        selector = BuildGroup(o => o.Email, selector);
                    }
                    else if (group.Member == "DoB")
                    {
                        selector = BuildGroup(o => o.DoB, selector);
                    }
                    else if (group.Member == "Phone")
                    {
                        selector = BuildGroup(o => o.Phone, selector);
                    }
                }
            }

            return selector.Invoke(data).ToList();
        }

        return data.ToList();
    }

    private static Func<IEnumerable<AuthorViewModel>, IEnumerable<AggregateFunctionsGroup>>
        BuildGroup<T>(Expression<Func<AuthorViewModel, T>> groupSelector, Func<IEnumerable<AuthorViewModel>,
        IEnumerable<AggregateFunctionsGroup>> selectorBuilder)
    {
        var tempSelector = selectorBuilder;
        return g => g.GroupBy(groupSelector.Compile())
                     .Select(c => new AggregateFunctionsGroup
                     {
                         Key = c.Key,
                         HasSubgroups = true,
                         Member = groupSelector.MemberWithoutInstance(),
                         Items = tempSelector.Invoke(c).ToList()
                     });
    }

    private static IEnumerable<AggregateFunctionsGroup> BuildInnerGroup<T>(IEnumerable<AuthorViewModel>
        group, Expression<Func<AuthorViewModel, T>> groupSelector)
    {
        return group.GroupBy(groupSelector.Compile())
                .Select(i => new AggregateFunctionsGroup
                {
                    Key = i.Key,
                    Member = groupSelector.MemberWithoutInstance(),
                    Items = i.ToList()
                });
    }

    public static IQueryable<AuthorViewModel> ApplyOrdersSorting(this IQueryable<AuthorViewModel> data,
                IList<GroupDescriptor> groupDescriptors, IList<SortDescriptor> sortDescriptors)
    {
        if (groupDescriptors != null && groupDescriptors.Any())
        {
            foreach (var groupDescriptor in groupDescriptors.Reverse())
            {
                data = AddSortExpression(data, groupDescriptor.SortDirection, groupDescriptor.Member);
            }
        }

        if (sortDescriptors != null && sortDescriptors.Any())
        {
            foreach (SortDescriptor sortDescriptor in sortDescriptors)
            {
                data = AddSortExpression(data, sortDescriptor.SortDirection, sortDescriptor.Member);
            }
        }

        return data;
    }

    private static IQueryable<AuthorViewModel> AddSortExpression(IQueryable<AuthorViewModel> data, ListSortDirection
                sortDirection, string memberName)
    {
        if (sortDirection == ListSortDirection.Ascending)
        {
            switch (memberName)
            {
                case "Id":
                    data = data.OrderBy(order => order.Id);
                    break;
                case "Name":
                    data = data.OrderBy(order => order.Name);
                    break;
                case "Email":
                    data = data.OrderBy(order => order.Email);
                    break;
                case "DoB":
                    data = data.OrderBy(order => order.DoB);
                    break;
                case "Phone":
                    data = data.OrderBy(order => order.Phone);
                    break;
            }
        }
        else
        {
            switch (memberName)
            {
                case "Id":
                    data = data.OrderBy(order => order.Id);
                    break;
                case "Name":
                    data = data.OrderBy(order => order.Name);
                    break;
                case "Email":
                    data = data.OrderBy(order => order.Email);
                    break;
                case "DoB":
                    data = data.OrderBy(order => order.DoB);
                    break;
                case "Phone":
                    data = data.OrderBy(order => order.Phone);
                    break;
            }
        }
        return data;
    }

    public static IQueryable<AuthorViewModel> ApplyOrdersFiltering(this IQueryable<AuthorViewModel> data,
       IList<IFilterDescriptor> filterDescriptors)
    {
        if (filterDescriptors.Any())
        {
            data = data.Where(ExpressionBuilder.Expression<AuthorViewModel>(filterDescriptors, false));
        }
        return data;
    }
}

